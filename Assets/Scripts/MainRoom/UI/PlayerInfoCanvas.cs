using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoCanvas : MonoBehaviour
{
    private MainCanvases _mainCanvases;

    [SerializeField]
    private ProfileDisplayPanel _profileDisplayPanel;
    [SerializeField]
    private ProfileEditPanel _profileEditPanel;
    [SerializeField]
    private Button _exitButton;
    [SerializeField]
    private Button _editProfileButton;

    [SerializeField]
    private string[] _playerInfoData;

    public void FirstInitialize(MainCanvases canvases)
    {
        _mainCanvases = canvases;
        _profileDisplayPanel.Update_PlayerInfo(_playerInfoData);
    }

    private void Start()
    {
        _exitButton.onClick.AddListener(OnClick_Exit);
        _editProfileButton.onClick.AddListener(OnClick_EditProfile);
    }

    private void OnClick_Exit()
    {
        Hide();
    }

    private void OnClick_EditProfile()
    {
        _profileDisplayPanel.Hide();
        _profileEditPanel.Show(_playerInfoData);
    }

    public void SetPlayerInfoData(string[] playerInfoData)
    {
        _playerInfoData = playerInfoData;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
