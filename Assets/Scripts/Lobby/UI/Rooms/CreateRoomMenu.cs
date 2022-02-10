using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _roomName;

    private RoomsCanvases _roomCanvases;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
    }

    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true;
        options.MaxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, null);
    }

    public void OnClick_BackToLobby()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        RoomsCanvases.Instance.CreateOrJoinRoomCanvas.Hide();
        /*if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("CreateRoomMenu/back to lobby, reload scene Mainroom");
        }*/ 


    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " Created room successfully.");
        RoomsCanvases.Instance.CreateOrJoinRoomCanvas.Hide();
        /* moved to TestConnect.OnJoinedRoom
        RoomsCanvases.Instance.CurrentRoomCanvas.Show();
        RoomsCanvases.Instance.CurrentRoomCanvas.LinkedSceneName = RoomsCanvases.Instance.CreateOrJoinRoomCanvas.LinkedSceneName;
        */
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Room creation failed: " + message.ToString());
    }
}
