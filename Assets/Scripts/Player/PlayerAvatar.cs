using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar: MonoBehaviour
{
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

    public PhotonView PhotonView;

    [HideInInspector]
    public AvatarAppearance Appearance;

    private void Start()
    {
        PhotonView = GetComponent<PhotonView>();
        LocalPlayerGo = gameObject;

        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom)
        {
            // LocalAvatarAppearance was set in AWAKE of customization tab.
            // which means we are safe!
            Appearance = AvatarAppearance.LocalAvatarAppearance;
            Appearance.ApplyAppearance(this);
            return;
        }
        
        if (PhotonView.IsMine)
        {
            Debug.Log($"<color=red> CustomizableElement/ setting my appearance: #{PhotonNetwork.LocalPlayer.ActorNumber} </color>");

            Appearance = AvatarAppearance.LocalAvatarAppearance;
            Appearance.SyncAppearance(this);
        }
        else
        {   // if not mine, create blank appearance to be synced later.
            if(Appearance is null) Appearance = new AvatarAppearance();
        }
        

    }


    [PunRPC]
    public void SetMaterialBaseColor(string partsId, float r, float g, float b, float a, PhotonMessageInfo info)
    {
        Debug.Log($"<color=blue> CustomizableElement/ color PunRPC from actor #{info.Sender.ActorNumber} </color>");
        Color col = new Color();
        col.r = r; col.g = g; col.b = b; col.a = a;

        // before calling Appearance, make sure it exists!
        if (Appearance is null) Appearance = new AvatarAppearance();
        Appearance[partsId].SetMaterialBaseColor(col);
        Appearance.ApplyAppearance(this);
    }

    ~PlayerAvatar()
    {
        Debug.Log("PlayerAvatar/Destructor");
    }
}
