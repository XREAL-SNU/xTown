using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LeaveRoomMenu : MonoBehaviour
{
    private RoomsCanvases _roomsCanvas;

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvas = canvases;
    }

    public void OnClick_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        _roomsCanvas.CurrentRoomCanvas.Hide();
    }
}
