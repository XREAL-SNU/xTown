using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomsManager : MonoBehaviourPunCallbacks
{
    /* singleton */
    public static RoomsManager Instance { get; private set; }
    private RoomsManager() { }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    /* public methods */

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    /* pun callbacks */
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);

        // load room if first player to join. other players will be synced automatically.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("RoomsManager/ loading scene ");
            PhotonNetwork.LoadLevel("YachtScene");
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning("RoomsManager/room failed with exit code: " + returnCode + ":" + message);
        Debug.Log("Creating a new room automatically");
        string roomName = "Room " + Random.Range(0, 1000);

        RoomOptions opt = new RoomOptions();
        opt.MaxPlayers = 8;
        PhotonNetwork.CreateRoom(roomName, opt);
        // PanelsManager.Instance.SetActivePanel("MenuPanel");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("RoomsManager/room creation faild with exit code: " + returnCode + ":" + message);
        PanelsManager.Instance.SetActivePanel("MenuPanel");
    }
}
