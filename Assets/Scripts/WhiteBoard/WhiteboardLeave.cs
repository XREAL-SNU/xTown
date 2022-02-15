using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WhiteboardLeave : MonoBehaviourPunCallbacks
{
    public void OnClick_LeaveButton()
    {
        Debug.Log("WhiteboardLeave / Button Clicked!");
        PlayerPrefs.SetString("prevScene", "Whiteboard");
        if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
        else if(PhotonNetwork.InLobby) PhotonNetwork.RejoinRoom("MainWorld");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("WhiteboardLeave / Player Left Room");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("WhiteboardLeave / Player Joined Lobby");
        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true;
        options.MaxPlayers = 20;
        options.EmptyRoomTtl = 20000;
        options.PlayerTtl = 30000;
        PhotonNetwork.JoinOrCreateRoom("MainWorld", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("WhiteboardLeave / Joined MainWorld!!!");
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("WhiteboardLeave / Created MainWorld!!!");
        PhotonNetwork.LoadLevel("MainRoom");
    }
}
