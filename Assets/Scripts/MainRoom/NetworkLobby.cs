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
        PhotonNetwork.JoinLobby();
    }
       
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby.", this);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon.", this);
    }

}
