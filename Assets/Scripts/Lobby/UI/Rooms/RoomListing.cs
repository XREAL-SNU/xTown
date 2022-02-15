using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _text.text = roomInfo.MaxPlayers + ", " + roomInfo.Name;
        Debug.Log("RoomListing / " + roomInfo.Name);
    }

    public void OnClick_Button()
    {
        // order matters!!!! Join room first.
        RoomsCanvases.Instance.CreateOrJoinRoomCanvas.Hide();
        if (RoomInfo is null)
        {
            Destroy(this.gameObject);
            return;
        }
        PhotonNetwork.JoinRoom(RoomInfo.Name);
        Debug.Log($"RoomListing/Join room called on : {RoomInfo.Name}");
        /* moved to TestConnect.OnJoinedRoom
        RoomsCanvases.Instance.CurrentRoomCanvas.Show();
        RoomsCanvases.Instance.CurrentRoomCanvas.LinkedSceneName = RoomsCanvases.Instance.CreateOrJoinRoomCanvas.LinkedSceneName;
        */
    }
   
}
