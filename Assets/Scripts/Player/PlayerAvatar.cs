using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar: MonoBehaviour
{

    /* moved to PLayerManager
    static GameObject _localPlayerGo;
    static PlayerAvatar _localPlayerAvatar;


    public static GameObject LocalPlayerGo
    {
        get => _localPlayerGo;
        set
        {
            Debug.Log("PlayerAvatar/ LocalPlayer found");
            _localPlayerGo = value;
            if (LocalPlayerAvatar is null)
            {
                LocalPlayerAvatar = _localPlayerGo.AddComponent<PlayerAvatar>();
            }
        }
    }
    public static PlayerAvatar LocalPlayerAvatar
    {
        get => _localPlayerAvatar;
        private set
        {
            _localPlayerAvatar = value;
        }
    }
    */
    [HideInInspector]
    public PhotonView PhotonView;

    [HideInInspector]
    public AvatarAppearanceNew Appearance;

    
    private void Start()
    {
        
        PhotonView = GetComponent<PhotonView>();
        
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom)
        {
            Appearance = PlayerManager.Players.LocalAvatarAppearance;
            return;
        }

        // add gameObject to managed list
        RoomManager.Room.AddPlayerGameObject(PhotonView.OwnerActorNr, gameObject);

        if (PhotonView.IsMine)
        {
            PlayerManager.Players.LocalPlayerGo = gameObject;
            Appearance = PlayerManager.Players.LocalAvatarAppearance;

            // Test code
            Appearance.SetProperty(Appearance.GetCustomPartGo("Helmet"), "Color" ,AvatarAppearanceNew.AppearancePropertyTypes.BaseColor,
                ColorPalette.DefaultColorPalette.PaletteName, PhotonNetwork.LocalPlayer.ActorNumber % 3);
            Appearance.Apply(gameObject);

            Debug.Log($"<color=red> PlayerAvatar/ setting my appearance: actor#{PhotonNetwork.LocalPlayer.ActorNumber} </color>");
            // automatic application? Appearance.Apply(PlayerManager.Players.LocalPlayerGo);

            // also sync for others (instantiation time)
            SyncAvatarProperties();

            // and register for future
            RoomManager.BindEvent(gameObject, SyncAvatarProperties, RoomManager.RoomEvent.PlayerJoined);
        }
        else
        {   
            // if not mine, create blank appearance to be synced later.
            if(Appearance is null) Appearance = new AvatarAppearanceNew(AvatarAppearanceNew.XRealSpaceSuitAppearanceDescriptor, gameObject);
        }

        if (Appearance is null) Debug.LogError("PlayerAvatar/ could not fetch or create appearance for this avatar");
    }
    
    ~PlayerAvatar()
    {
        Debug.Log("PlayerAvatar/Destructor");
    }




    #region PhotonRPC
    // Photon Avatar Sync
    public void SyncAvatarProperties(PlayerInfo playerInfo)
    {
        Debug.Log($"<color = blue> PlayerAvatar/Begin Sync {playerInfo.PlayerName}'s avatar properties </blue>");
        SyncAvatarProperties(playerInfo.ActorNr);
    }

    public void SyncAvatarProperties(int actorNr = -1)
    {
        ObjectPartsInfo info = Appearance.Descriptor;
        foreach (ObjectPart part in info.Parts)
        {
            GameObject go = Appearance.GetCustomPartGo(part.PartName);
            foreach(ObjectPartProperty prop in part.Properties)
            {
                PhotonView.RPC("SetAndApplyObjectPartPropertyRPC", RpcTarget.Others, part.PartName, prop.PropertyName,
                    prop.PaletteName, prop.Pick, actorNr);
            }
        }
    }



    // wish this were in AvatarAppearanceNew...
    [PunRPC]
    public void SetAndApplyObjectPartPropertyRPC
        (string partName, string propName, string palette, int pick, int actorNr, PhotonMessageInfo info)
    {
        // if an actor number to execute the sync is specified, skip if I'm not that one.
        if (actorNr > 0 && PhotonNetwork.LocalPlayer.ActorNumber != actorNr) return;

        // depending on timing, Appearance may be null at time of player join.
        if (Appearance is null)
        {
            Debug.LogWarning("Appearance null at sync stage, creating new");
            Appearance = new AvatarAppearanceNew(AvatarAppearanceNew.XRealSpaceSuitAppearanceDescriptor, gameObject);
        }
        Debug.Log($"<color=green> PlayerAvatar/ setting other's appearance: actor#{info.Sender.ActorNumber} </color>");


        GameObject partGO = Appearance.GetCustomPartGo(partName);
        ObjectPartProperty prop = Appearance.GetCustomPart(partName)[propName];
        prop = prop.SetProperty(palette, pick);
        AvatarAppearanceNew.AppearancePropertyTypes type = (AvatarAppearanceNew.AppearancePropertyTypes)
            Enum.Parse(typeof(AvatarAppearanceNew.AppearancePropertyTypes), prop.PropertyType);
        Appearance.GetCustomPart(partName).SetProperty(propName, type, palette, pick);
        Appearance.ApplyProperty(partGO, prop);
    }

    #endregion
}
