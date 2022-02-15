using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Portal : MonoBehaviourPunCallbacks
{
    public string SceneName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerPrefs.SetString("PastScene", "MainRoom");
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        MainCanvases.Instance.gameObject.SetActive(false);
        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true;
        options.MaxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom("Whiteboard", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Portal/Joined Whiteboard Room!!!");
        //SceneManager.LoadScene("MainRoom", LoadSceneMode.Single);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Portal/Created Whiteboard Room!!!");
        PhotonNetwork.LoadLevel("Whiteboard");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Failed.. because " + message);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed.. because " + message);
    }
}
