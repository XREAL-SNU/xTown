using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlarmEditorPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _nameInputField;
    [SerializeField]
    private TMP_InputField _hourInputField;
    [SerializeField]
    private TMP_InputField _minuteInputField;
    [SerializeField]
    private Toggle _privateToggle;
    [SerializeField]
    private Toggle _amToggle;

    private AlarmCanvas _alarmCanvas;

    public void OnClick_Exit()
    {
        Hide();
    }

    public void OnClick_Confirm()
    {
        bool isValid = CheckInputValid();

        if (isValid)
        {
            // Save new alarm to alarm list and close this panel.
            Alarm newAlarm = new Alarm();

            newAlarm.name = _nameInputField.text;
            newAlarm.isPrivate = _privateToggle.isOn;
            newAlarm.isAM = _amToggle.isOn;
            newAlarm.hour = int.Parse(_hourInputField.text);
            newAlarm.minute = int.Parse(_minuteInputField.text);

            AlarmCanvas.AddAlarm(newAlarm);
            AlarmCanvas.Instance.AlarmListPanel.InvokeChangedEvent();

            // Fetch new alarmList to AlarmListPanel.


            Hide();
        }
        else
        {
            // Do nothing or display warning popup.
            Debug.Log("inputs are not valid");
        }
    }

    public void OnClick_Plus()
    {

    }
    public void OnClick_Minus()
    {

    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    public void InitializeInputs()
    {
        _privateToggle.isOn = true;
        ResetInputField(_nameInputField);
        ResetInputField(_hourInputField);
        ResetInputField(_minuteInputField);
    }

    public void ResetInputField(TMP_InputField inputField)
    {
        _nameInputField.text = "";
        _hourInputField.text = "";
        _minuteInputField.text = "";
    }

    public void FirstInitialize(AlarmCanvas alarmCanvas)
    {
        _alarmCanvas = alarmCanvas;
    }

    public void ClampHourInput(string value)
    {
        int clampedNumber = Mathf.Clamp(int.Parse(value), 0, 12);

        _hourInputField.text = clampedNumber.ToString();
    }

    public void ClampMinuteInput(string value)
    {
        int clampedNumber = Mathf.Clamp(int.Parse(value), 0, 59);

        _minuteInputField.text = clampedNumber.ToString();
    }

    private bool CheckInputValid()
    {
        if (CheckIfEmpty(_nameInputField)) return false;
        if (CheckIfEmpty(_hourInputField)) return false;
        if (CheckIfEmpty(_minuteInputField)) return false;

        return true;
    }

    private bool CheckIfEmpty(TMP_InputField inputField)
    {
        if (inputField.text.Length == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
