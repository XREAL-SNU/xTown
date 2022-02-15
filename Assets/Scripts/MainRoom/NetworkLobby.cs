using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;
using Photon.Realtime;
using Cinemachine;
using StarterAssets;

public class NetworkLobby : MonoBehaviourPunCallbacks
{
    public static NetworkLobby Instance = null;

    // Start is called before the first frame update
    void Awake()
    {
        // singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

}
