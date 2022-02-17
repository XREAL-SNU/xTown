using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [Header("UI")]
    public Text RoomNameText;
    public Text RoomPlayersText;
    public Button JoinRoomButton;

    private string _roomName;
    
    public void SetItemContent(string roomName, int currentPlayers, int maxPlayers)
    {
        RoomNameText.text = roomName;
        RoomPlayersText.text = currentPlayers + "/" + maxPlayers;
    }

    public void OnClick_JoinRoom()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        PhotonNetwork.JoinRoom(_roomName);
    }
}
