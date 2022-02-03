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
    private void Start()
    {
        if (!PhotonNetwork.InRoom)
        {
            LocalPlayerGo = gameObject;
            return;
        }
        // photonView is meaningful only inside a room
        PhotonView = GetComponent<PhotonView>();
        if (PhotonView.IsMine) LocalPlayerGo = gameObject;
    }


    public void OnAvatarInstantiate()
    {
        Debug.Log("PlayerAvatar/ OnAvatarInstantiate callback");
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom)
        {
            // no room, no sync. just set.
            AvatarAppearance.LocalAvatarAppearance.ApplyAppearance(this);
            return;
        }
        if (PhotonView.IsMine)
        {
            // apply the persistent appearance to the newly instantiated avatar.
            AvatarAppearance.LocalAvatarAppearance.SyncAppearance(this);
        }
    }
    ~PlayerAvatar()
    {
        Debug.Log("PlayerAvatar/Destructor");
    }
}
