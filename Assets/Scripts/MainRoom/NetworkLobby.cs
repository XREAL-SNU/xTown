using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;

public class NetworkLobby : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();

        

    }

    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("NetworkLobby/Joining lobby");
            PhotonNetwork.JoinLobby();
        }
    }
}
