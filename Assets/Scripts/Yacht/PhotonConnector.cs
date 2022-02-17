using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PhotonConnector : MonoBehaviourPunCallbacks
{
    public static PhotonConnector Instance { get; private set; }
    private PhotonConnector() { }
    private void Awake()
    {
        Instance = this;
    }

    /* public methods */
    public void ConnectToPhoton()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    /* Pun callbacks */
    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master, player name: " + PhotonNetwork.LocalPlayer.NickName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("disconnected with reason: " + cause);
    }


}
