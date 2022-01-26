using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlarmEditorPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _deleteButton;
    [SerializeField]
    private Button _confirmButton;
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

    private bool _isEditing = false;
    private int _currentAlarmIndex;

    // 새로운 알람을 만들 때 입력필드 초기화하는 함수
    public void Initialize()
    {
        _deleteButton.SetActive(false);
        _confirmButton.interactable = false;
        _isEditing = false;
        _privateToggle.isOn = true;

        ResetInputField(_nameInputField);
        ResetInputField(_hourInputField);
        ResetInputField(_minuteInputField);
    }

    // 기존의 알람을 편집할 때 입력필드 초기화하는 함수. 파라미터로 받은 알람 정보를 활용해 입력필드를 채워넣음
    public void Initialize(int i, Alarm alarm)
    {
        _deleteButton.SetActive(true);
        _confirmButton.interactable = true;
        _currentAlarmIndex = i;
        _isEditing = true;

        _privateToggle.isOn = alarm.isPrivate;
        _amToggle.isOn = alarm.isAM;
        _nameInputField.text = alarm.name;
        _hourInputField.text = alarm.hour.ToString();
        _minuteInputField.text = alarm.minute.ToString();
    }

    #region ButtonCallbacks
    
    // Confirm 버튼을 눌렀을 때 
    public void OnClick_Confirm()
    {
        // 기존의 알람을 편집할 때. alarmList 리스트의 기존 알람 정보를 수정함
        if (_isEditing)
        {
            Alarm alarm = AlarmManager.alarmList[_currentAlarmIndex];

            alarm.name = _nameInputField.text;
            alarm.isPrivate = _privateToggle.isOn;
            alarm.isAM = _amToggle.isOn;
            alarm.hour = int.Parse(_hourInputField.text);
            alarm.minute = int.Parse(_minuteInputField.text);

            AlarmScript.Instance.AlarmCanvas.AlarmListPanel.InvokeChangedEvent();

            Hide();
        }
        // 새로운 알람을 만들 때. 새로운 알람을 alarmList 리스트에 추가함
        else
        {
            Alarm newAlarm = new Alarm();

            newAlarm.name = _nameInputField.text;
            newAlarm.isPrivate = _privateToggle.isOn;
            newAlarm.isAM = _amToggle.isOn;
            newAlarm.hour = int.Parse(_hourInputField.text);
            newAlarm.minute = int.Parse(_minuteInputField.text);

            AlarmManager.AddAlarm(newAlarm);
            AlarmScript.Instance.AlarmCanvas.AlarmListPanel.InvokeChangedEvent();

            Hide();
        }
    }

    // Delete 버튼을 눌렀을 때 
    public void OnClick_Delete()
    {
        AlarmManager.RemoveAlarm(_currentAlarmIndex);
        AlarmScript.Instance.AlarmCanvas.AlarmListPanel.InvokeChangedEvent();
        Hide();
    }

    // Close 버튼을 눌렀을 때 
    public void OnClick_Exit()
    {
        Hide();
    }

    // + 버튼을 눌렀을 때 
    public void OnClick_Plus(int i)
    {
        if (i == 0) // For hour inputfield
        {
            string temp = _hourInputField.text;
            if (temp != "")
            {
                _hourInputField.text = Mathf.Clamp(int.Parse(temp) + 1, 0, 12).ToString();
            }
            else
            {
                _hourInputField.text = 0.ToString();
            }
        }
        else // For minute inpurfield
        {
            string temp = _minuteInputField.text;
            if (temp != "")
            {
                _minuteInputField.text = Mathf.Clamp(int.Parse(temp) + 1, 0, 59).ToString();
            }
            else
            {
                _minuteInputField.text = 0.ToString();
            }
        }
    }

    // - 버튼을 눌렀을 때 
    public void OnClick_Minus(int i)
    {
        if (i == 0) // For hour inputfield
        {
            string temp = _hourInputField.text;
            if (temp != "")
            {
                _hourInputField.text = Mathf.Clamp(int.Parse(temp) - 1, 0, 12).ToString();
            }
            else
            {
                _hourInputField.text = 0.ToString();
            }
        }
        else // For minute inpurfield
        {
            string temp = _minuteInputField.text;
            if (temp != "")
            {
                _minuteInputField.text = Mathf.Clamp(int.Parse(temp) - 1, 0, 59).ToString();
            }
            else
            {
                _minuteInputField.text = 0.ToString();
            }
        }
    }
    #endregion

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ResetInputField(TMP_InputField inputField)
    {
        _nameInputField.text = "";
        _hourInputField.text = "";
        _minuteInputField.text = "";
    }

    // 시간 인풋필드의 입력값을 0~12의 값으로 클램핑
    public void ClampHourInput(string value)
    {
        int clampedNumber = Mathf.Clamp(int.Parse(value), 0, 12);

        _hourInputField.text = clampedNumber.ToString();
    }

    // 분 인풋필드의 입력값을 0~59의 값으로 클램핑
    public void ClampMinuteInput(string value)
    {
        if (value != "")
        {
            int clampedNumber = Mathf.Clamp(int.Parse(value), 0, 59);

            _minuteInputField.text = clampedNumber.ToString();
        }

    }

    public void CheckInputValid()
    {
        // 비어있는 인풋필드가 있으면 false를 반환
        if (CheckIfEmpty(_nameInputField) || CheckIfEmpty(_hourInputField) || CheckIfEmpty(_minuteInputField))
        {
            _confirmButton.interactable = false;
            return;
        }

        // 비어있는 인풋필드가 없으면 true를 반환
        _confirmButton.interactable = true;
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
