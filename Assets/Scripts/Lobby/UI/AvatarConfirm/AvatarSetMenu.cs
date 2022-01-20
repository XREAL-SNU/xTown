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
        Debug.Log(PhotonNetwork.LocalPlayer.NickName, this);
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("MainRoom", LoadSceneMode.Single);
        _roomCanvases.gameObject.SetActive(false);
    }

    public void OnClick_No()
    {
        _roomCanvases.AvatarConfirmCanvas.Hide();
        _roomCanvases.AvatarSelectionCanvas.Show();
    }
}
