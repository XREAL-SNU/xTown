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
        if (!PhotonNetwork.IsConnected)
            return;

        Debug.Log(PhotonNetwork.LocalPlayer.NickName, this);
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("MainRoom", LoadSceneMode.Single);
        gameObject.SetActive(false);
    }

}
