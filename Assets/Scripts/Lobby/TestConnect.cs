using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class TestConnect : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (!PlayerPrefs.HasKey("PastScene"))
        {
            Debug.Log("Connecting to Photon...", this);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
            PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
            PlayerPrefs.DeleteKey("PastScene");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master.", this);

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Failed to connect to Photon: " + cause.ToString(), this);
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Player Left Lobby.", this);
        PlayerPrefs.DeleteAll();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Player Joined Lobby. (TestConnect)");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Player Joined Room, room_name:" + PhotonNetwork.CurrentRoom.Name);
        Debug.Log(PhotonNetwork.InLobby ? "in lobby" : "not in lobby");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Player Left Room");

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("new player entered room! #" + newPlayer.ActorNumber + "name:" + newPlayer.NickName);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("player left room! #" + otherPlayer.ActorNumber + "name:" + otherPlayer.NickName);
    }
}