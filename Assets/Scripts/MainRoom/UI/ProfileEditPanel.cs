using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileEditPanel : MonoBehaviour
{
    [SerializeField]
    private PlayerInfoCanvas _playerInfoCanvas;
    [SerializeField]
    private ProfileDisplayPanel _profileDisplayPanel;
    [SerializeField]
    private Button _confirmButtom;
    [SerializeField]
    private InputField _nicknameInputField;
    [SerializeField]
    private InputField _teamInputField;
    [SerializeField]
    private InputField _fowInputField;
    [SerializeField]
    private InputField _introInputField;
    [SerializeField]
    private InputField _contactInputField;

    // Start is called before the first frame update
    void Start()
    {
        _confirmButtom.onClick.AddListener(OnClick_Confirm);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(string[] playerInfoData)
    {
        _nicknameInputField.text = playerInfoData[0];
        _teamInputField.text = playerInfoData[1];
        _fowInputField.text = playerInfoData[2];
        _introInputField.text = playerInfoData[3];
        _contactInputField.text = playerInfoData[4];
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private bool CheckInputValid()
    {
        if (CheckIfEmpty(_nicknameInputField))
        {
            return false;
        }
        if (CheckIfEmpty(_teamInputField))
        {
            return false;
        }
        if (CheckIfEmpty(_fowInputField))
        {
            return false;
        }
        if (CheckIfEmpty(_introInputField))
        {
            return false;
        }
        if (CheckIfEmpty(_contactInputField))
        {
            return false;
        }
        return true;
    }

    private bool CheckIfEmpty(InputField inputField)
    {
        if (inputField.text == "")
        {
            return true;
        }
        return false;
    }

    private void OnClick_Confirm()
    {
        if (CheckInputValid() == true)
        {
            string[] playerInfoData = new string[]{ 
                _nicknameInputField.text, _teamInputField.text, _fowInputField.text, _introInputField.text, _contactInputField.text 
            };
            _playerInfoCanvas.SetPlayerInfoData(playerInfoData);
            _profileDisplayPanel.Update_PlayerInfo(playerInfoData);
            _profileDisplayPanel.Show();
            Hide();
        }
    }
}
