using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlarmUI : MonoBehaviour
{
    [SerializeField]
    private GameObject settingPanel;
    [SerializeField]
    private InputField hourInputField;
    [SerializeField]
    private InputField minuteInputField;
    [SerializeField]
    private GameObject alertPanel;

    [SerializeField]
    private StatusPanel _statusPanel;
    public AlarmManager _alarmManager;


    private bool isPopupOpen;

    // Start is called before the first frame update
    void Start()
    {
        isPopupOpen = settingPanel.activeSelf;
    }

    // Update is called once per frame

    public void SettingClicked()
    {
        if (!isPopupOpen)
        {
            settingPanel.SetActive(true);
            isPopupOpen = true;
        }
        else
        {
            settingPanel.SetActive(false);
            isPopupOpen = false;
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
            string hour = hourInputField.text;
            string minute = minuteInputField.text;

            _alarmManager.SetAlarm(int.Parse(hour), int.Parse(minute));
            _statusPanel.SetAlarm(hour,minute);
            settingPanel.SetActive(false);
            isPopupOpen = false;
        }
    }

    private bool CheckInputValid()
    {
        string hour = hourInputField.text;
        string minute = minuteInputField.text;

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
        alertPanel.SetActive(true);
        Text alertText = alertPanel.GetComponentInChildren<Text>();
        alertText.text = message;
        StartCoroutine(ShowForSeconds());
    }

    IEnumerator ShowForSeconds()
    {
        yield return new WaitForSeconds(2);
        alertPanel.SetActive(false);
    }
}
