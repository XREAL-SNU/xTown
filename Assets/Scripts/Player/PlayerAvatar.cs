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
        PhotonView = GetComponent<PhotonView>();
        if (PhotonView.IsMine) LocalPlayerGo = gameObject;
    }

    public void ChangeMaterialColor()
    {
        Debug.Log("PlayerAvatar/ Change Material");
        var renderer = LocalPlayerGo.transform.Find("Space_Suit/Tpose_/Man_Suit/Body").GetComponent<SkinnedMeshRenderer>();
        renderer.materials[0].SetColor("_Color", Color.red);
    }
}
