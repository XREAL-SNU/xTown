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

    public void NextCharacter()
    {
        Characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % Characters.Length;
        Characters[selectedCharacter].SetActive(true);
    }

    public void PreviousCharacter()
    {
        Characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += Characters.Length;
        }
        Characters[selectedCharacter].SetActive(true);
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
        Debug.Log("Joined Lobby -- from Avatar selection menu");
        SceneManager.LoadScene("MainRoom", LoadSceneMode.Single);
        gameObject.SetActive(false);
    }

}
