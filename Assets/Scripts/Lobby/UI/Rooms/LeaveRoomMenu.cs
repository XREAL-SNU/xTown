using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

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
        RoomsCanvases.Instance.CurrentRoomCanvas.Hide();
        MainCanvases.Instance.MainCanvas.Show();
    }
}
