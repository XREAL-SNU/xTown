using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlarmUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _settingPanel;
    [SerializeField]
    private InputField _hourInputField;
    [SerializeField]
    private InputField _minuteInputField;
    [SerializeField]
    private GameObject _alertPanel;

    private bool _isPopupOpen;

    // Start is called before the first frame update
    void Start()
    {
        _isPopupOpen = _settingPanel.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SettingClicked()
    {
        if (!_isPopupOpen)
        {
            _settingPanel.SetActive(true);
            _isPopupOpen = true;
        }
        else
        {
            _settingPanel.SetActive(false);
            _isPopupOpen = false;
        }
    }

    public void SaveClicked()
    {
        bool isInputValid = CheckInputValid();
        if (CheckInputValid() != true)
        {
            return;
        }
        else
        {
            string hour = _hourInputField.text;
            string minute = _minuteInputField.text;

            AlarmManager.SetAlarm(int.Parse(hour), int.Parse(minute));
            _settingPanel.SetActive(false);
            _isPopupOpen = false;
        }
    }

    private bool CheckInputValid()
    {
        string hour = _hourInputField.text;
        string minute = _minuteInputField.text;

        if (hour.Length == 0 || minute.Length == 0)
        {
            ShowAlert("Please fill in all fields");
            return false;
        }
        else
        {
            return true;
        }
    }

    private void ShowAlert(string message)
    {
        _alertPanel.SetActive(true);
        Text alertText = _alertPanel.GetComponentInChildren<Text>();
        alertText.text = message;
        StartCoroutine(ShowForSeconds());
    }

    IEnumerator ShowForSeconds()
    {
        yield return new WaitForSeconds(2);
        _alertPanel.SetActive(false);
    }
}
