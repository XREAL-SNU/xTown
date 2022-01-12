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
        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("NetworkLobbyAwake/Join lobby");
            PhotonNetwork.JoinLobby();
        }

    }
       

    public override void OnConnectedToMaster()
    {
        Debug.Log("NetworkLobby/Connected to Master.", this);
    }

}
