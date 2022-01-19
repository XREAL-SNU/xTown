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
            Hide();
        }
        else
        {
            // Do nothing or display warning popup.
            Debug.Log("inputs are not valid");
        }
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

    public void FirstInitialize(AlarmCanvas alarmCanvas)
    {
        _alarmCanvas = alarmCanvas;
    }
}
