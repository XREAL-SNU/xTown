using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class AvatarSetMenu : MonoBehaviourPunCallbacks
{
    private RoomsCanvases _roomCanvases;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
    }
    public void OnClick_Yes()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        _roomCanvases.AvatarConfirmCanvas.Hide();
        Debug.Log(PhotonNetwork.LocalPlayer.NickName, this);
        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true;
        options.MaxPlayers = 20;
        PhotonNetwork.JoinOrCreateRoom("MainWorld", options, TypedLobby.Default); // Access MainWorld Room  
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("AvatarSelectionMenu/Joined MainWorld!!!");
        //SceneManager.LoadScene("MainRoom", LoadSceneMode.Single);
        gameObject.SetActive(false);
        _roomCanvases.gameObject.SetActive(false);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("AvatarSetMenu/Created MainWorld!!!");
        PhotonNetwork.LoadLevel("MainRoom");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Failed.. because " + message);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed.. because " + message);
    }

    public void OnClick_No()
    {
        _roomCanvases.AvatarConfirmCanvas.Hide();
        _roomCanvases.AvatarSelectionCanvas.Show();
    }
}
