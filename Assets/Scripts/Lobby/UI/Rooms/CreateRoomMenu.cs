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

        SceneManager.LoadScene("MainRoom",LoadSceneMode.Single);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " Created room successfully.");
        _roomCanvases.CurrentRoomCanvas.Show();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Room creation failed: " + message.ToString());
    }
}
