using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar: MonoBehaviour
{

    /*
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
        
        if (PhotonView.IsMine)
        {

            Appearance = PlayerManager.Players.LocalAvatarAppearance;
            Debug.Log($"<color=red> CustomizableElement/ setting my appearance: #{PhotonNetwork.LocalPlayer.ActorNumber} </color>");
            
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
}
