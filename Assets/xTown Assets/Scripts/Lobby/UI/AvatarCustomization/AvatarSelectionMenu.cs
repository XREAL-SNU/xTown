using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class AvatarSelectionMenu : MonoBehaviourPunCallbacks
{
    private RoomsCanvases _roomCanvases;
    public GameObject[] Characters;
    public int selectedCharacter = 0;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
    }


    public void OnClick_JoinWorld()
    {

        if (!PhotonNetwork.IsConnected)
            return;
        _roomCanvases.AvatarSelectionCanvas.Hide();
        _roomCanvases.AvatarConfirmCanvas.Show();

        /*
        Debug.Log(PhotonNetwork.LocalPlayer.NickName, this);
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);

        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true;
        options.MaxPlayers = 20;
        PhotonNetwork.JoinOrCreateRoom("MainWorld", options, TypedLobby.Default); // Access MainWorld Room
        */
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("AvatarSelectionMenu/Joined MainWorld!!!");
        //SceneManager.LoadScene("MainRoom", LoadSceneMode.Single);
        gameObject.SetActive(false);
    }

    public void OnClick_BackPlayerNameInputMenu()
    {
        _roomCanvases.AvatarSelectionCanvas.Hide();
        _roomCanvases.PlayerNameInputCanvas.Show();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("AvatarSelectionMenu/Created MainWorld!!!");
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
}
