using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class NickNameEditMenu : MonoBehaviour
{
    [SerializeField]
    private Text _playerNickName;
    private RoomsCanvases _roomCanvases;
    const string playerNamePrefKey = "PlayerName";
    


    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
    }

    public void SetNickName()
    {
        _playerNickName.text = PlayerPrefs.GetString(playerNamePrefKey);
    }
    public void OnEditClick()
    {
        _roomCanvases.AvatarConfirmCanvas.Hide();
        _roomCanvases.PlayerNameInputCanvas.Show();
    }
}
