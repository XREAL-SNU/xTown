using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [Header("UI")]
    public Button JoinRandomBtn;
    public Button CreateRoomBtn;
    public Button ShowRoomListBtn;

    private void Awake()
    {
        // cannot yet create or show room lists.
        CreateRoomBtn.interactable = false;
        ShowRoomListBtn.interactable = false;
    }

    /* UI Callbacks */
    public void OnClick_JoinRandom()
    {
        RoomsManager.Instance.JoinRandomRoom();
    }
}
