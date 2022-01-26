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
                LocalPlayerAvatar = new PlayerAvatar();
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

    PhotonView photonView;
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine) LocalPlayerGo = gameObject;
    }

    public void ChangeMaterialColor()
    {
        Debug.Log("PlayerAvatar/ Change Material");
        var renderer = LocalPlayerGo.GetComponent<Renderer>();

        renderer.material.SetColor("_Color", Color.red);
    }
}
