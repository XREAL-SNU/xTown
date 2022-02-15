using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class PlayerNameInputMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _playerName;

    private RoomsCanvases _roomCanvases;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
    }
    const string playerNamePrefKey = "PlayerName";
  

    private void Start()
    {
        if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
            PhotonNetwork.NickName = PlayerPrefs.GetString(playerNamePrefKey);
        }       
    }

    public void OnClick_SetPlayerName()
    {
        if (string.IsNullOrEmpty(_playerName.text))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = _playerName.text;
        Debug.Log("Player Name is "+ _playerName.text, this);

        PlayerPrefs.SetString(playerNamePrefKey, _playerName.text);

        _roomCanvases.AvatarSelectionCanvas.Show();
        _roomCanvases.PlayerNameInputCanvas.Hide();
    }
/*
    public void OnClick_JoinLobby()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        Debug.Log("Joining to Lobby...", this);
        Debug.Log(PhotonNetwork.LocalPlayer.NickName, this);
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        _roomCanvases.CreateOrJoinRoomCanvas.Show();
    }
*/
}
