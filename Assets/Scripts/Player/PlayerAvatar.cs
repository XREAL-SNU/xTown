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
            // if my character, fetch my persistent data.
            Appearance = PlayerManager.Players.LocalAvatarAppearance;

            // apply once more.. redundant?
            Appearance.Apply(gameObject);
            
            Debug.Log($"<color=red> [view{PhotonView.ViewID}] PlayerAvatar/ setting my appearance: actor#{PhotonNetwork.LocalPlayer.ActorNumber} </color>");
            // automatic application? Appearance.Apply(PlayerManager.Players.LocalPlayerGo);

            // also sync for others (instantiation time)
            SyncAvatarProperties();

            // and register for future
            RoomManager.BindEvent(gameObject, SyncAvatarProperties, RoomManager.RoomEvent.PlayerJoined);
        }
        else
        {
            // if not mine, create blank appearance to be synced later.

            if (Appearance is null)
            {
                Appearance = new AvatarAppearanceNew(AvatarAppearanceNew.XRealSpaceSuitAppearanceDescriptor, gameObject);
            }
            
        }
    }
    
    ~PlayerAvatar()
    {
        //Debug.Log("PlayerAvatar/Destructor");
    }



    #region PhotonRPC
    // Photon Avatar Sync
    public void SyncAvatarProperties(PlayerInfo playerInfo)
    {
        Debug.Log($"<color=blue> PlayerAvatar/Sending my avatar with viewId {PhotonView.ViewID} to {playerInfo.PlayerName} </color>");

        // call only on the newly joined player.
        SyncAvatarProperties(playerInfo.ActorNr);
    }

    public void SyncAvatarProperties(int actorNr = -1)
    {
        ObjectPartsInfo info = Appearance.Descriptor;
        PhotonView photonView = GetComponent<PhotonView>();
        foreach (ObjectPart part in info.Parts)
        {
            GameObject go = Appearance.GetCustomPartGo(part.PartName);
            //Debug.Log($"<color=green> Sync apperance part {part.PartName} of player {PhotonView.OwnerActorNr} </color>");
            //_logger.Log($"<color=green> Sync apperance part {part.PartName} of player {PhotonView.OwnerActorNr} </color>");
            foreach (ObjectPartProperty prop in part.Properties)
            {
                // call on everyone but me.
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

        PhotonView photonView = GetComponent<PhotonView>();

        // depending on timing, Appearance may be null at time of player join.
        if (Appearance is null)
        {
            Debug.LogWarning("Appearance null at sync stage, creating new: ");
            // create from default feature
            Appearance = new AvatarAppearanceNew(AvatarAppearanceNew.XRealSpaceSuitAppearanceDescriptor, gameObject);
        }
        //debug
        Debug.Log($"<color=green> sync sender {info.Sender.NickName} == owner {info.photonView.OwnerActorNr}, sender viewID {info.photonView.ViewID} == {photonView.ViewID}: {partName}'s {propName} = {palette}.{pick} </color>");

        GameObject partGO = Appearance.GetCustomPartGo(partName);
        ObjectPartProperty prop = Appearance.GetCustomPart(partName)[propName];
        prop = prop.SetProperty(palette, pick);
        /*
        AvatarAppearanceNew.AppearancePropertyTypes type = (AvatarAppearanceNew.AppearancePropertyTypes)
            Enum.Parse(typeof(AvatarAppearanceNew.AppearancePropertyTypes), prop.PropertyType);
        Appearance.GetCustomPart(partName).SetProperty(propName, type, palette, pick);
        */
        // RPC called on each small part, so we apply one by one, not by Apperance.Apply(gameObject).
        Appearance.ApplyProperty(partGO, prop);
    }

    #endregion
}
