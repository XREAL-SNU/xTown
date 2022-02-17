using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Threading;

public class WhiteboardLeave : MonoBehaviourPunCallbacks
{
    public void OnClick_LeaveButton()
    {
        Debug.Log("WhiteboardLeave / Button Clicked!");
        PlayerPrefs.SetString("prevScene", "Whiteboard");
        TestConnect.Instance._isLeavingPortal = true;
        PhotonNetwork.LeaveRoom();

    }
    public override void OnJoinedLobby()
    {
        if(TestConnect.Instance._isLeavingPortal)
        {
            Debug.Log("WhiteboardLeave / Player Joined Lobby");
            RoomOptions options = new RoomOptions();
            options.BroadcastPropsChangeToAll = true;
            options.MaxPlayers = 20;
            options.EmptyRoomTtl = 20000;
            options.PlayerTtl = 30000;
            PhotonNetwork.JoinOrCreateRoom("MainWorld", options, TypedLobby.Default);
            TestConnect.Instance._isLeavingPortal = false;
        }
    }


    public override void OnCreatedRoom()
    {
        Debug.Log("WhiteboardLeave / Created MainWorld!!!");
        PhotonNetwork.LoadLevel("MainRoom");
    }
}
