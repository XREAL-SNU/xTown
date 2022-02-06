using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using XReal.Xtown.PhotonChat;

public class LeaveRoomMenu : MonoBehaviour
{
    private RoomsCanvases _roomsCanvas;
    private PhotonChatManager _photonChatManager;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvas = canvases;
    }

    public void OnClick_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        RoomsCanvases.Instance.CurrentRoomCanvas.Hide();
    }
}
