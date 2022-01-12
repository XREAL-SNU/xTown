using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class TestConnect : MonoBehaviourPunCallbacks
{
    public static TestConnect Instance = null;

    private void Awake()
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

        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {

            Debug.Log("Connecting to Photon...", this);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
            PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
            PhotonNetwork.ConnectUsingSettings();
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
        LoadCharacter.Instance.PlayerControl.enabled = false;
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