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


    public void OnClick_JoinLobby()
    {
        _roomCanvases.AvatarSelectionCanvas.Hide();
        _roomCanvases.AvatarConfirmCanvas.Show();
    }

    public void OnClick_BackPlayerNameInputMenu()
    {
        _roomCanvases.AvatarSelectionCanvas.Hide();
        _roomCanvases.PlayerNameInputCanvas.Show();
    }

}
