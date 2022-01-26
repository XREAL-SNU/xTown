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

    // ���ο� �˶��� ���� �� �Է��ʵ� �ʱ�ȭ�ϴ� �Լ�
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

    // ������ �˶��� ������ �� �Է��ʵ� �ʱ�ȭ�ϴ� �Լ�. �Ķ���ͷ� ���� �˶� ������ Ȱ���� �Է��ʵ带 ä������
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
    
    // Confirm ��ư�� ������ �� 
    public void OnClick_Confirm()
    {
        // ������ �˶��� ������ ��. alarmList ����Ʈ�� ���� �˶� ������ ������
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
        // ���ο� �˶��� ���� ��. ���ο� �˶��� alarmList ����Ʈ�� �߰���
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

    // Delete ��ư�� ������ �� 
    public void OnClick_Delete()
    {
        AlarmManager.RemoveAlarm(_currentAlarmIndex);
        AlarmScript.Instance.AlarmCanvas.AlarmListPanel.InvokeChangedEvent();
        Hide();
    }

    // Close ��ư�� ������ �� 
    public void OnClick_Exit()
    {
        Hide();
    }

    // + ��ư�� ������ �� 
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

    // - ��ư�� ������ �� 
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

    // �ð� ��ǲ�ʵ��� �Է°��� 0~12�� ������ Ŭ����
    public void ClampHourInput(string value)
    {
        int clampedNumber = Mathf.Clamp(int.Parse(value), 0, 12);

        _hourInputField.text = clampedNumber.ToString();
    }

    // �� ��ǲ�ʵ��� �Է°��� 0~59�� ������ Ŭ����
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
        // ����ִ� ��ǲ�ʵ尡 ������ false�� ��ȯ
        if (CheckIfEmpty(_nameInputField) || CheckIfEmpty(_hourInputField) || CheckIfEmpty(_minuteInputField))
        {
            _confirmButton.interactable = false;
            return;
        }

        // ����ִ� ��ǲ�ʵ尡 ������ true�� ��ȯ
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
