using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Alarm
{
    public string name;
    public bool isPrivate;
    public bool isAM;
    public int type;
    public int hour;
    public int minute;
}

public class AlarmCanvas : MonoBehaviour
{
    [SerializeField]
    private AlarmListPanel _alarmListPanel;
    public AlarmListPanel AlarmListPanel { get { return _alarmListPanel; } }

    [SerializeField]
    private AlarmEditorPanel _alarmEditorPanel;
    public AlarmEditorPanel AlarmEditorPanel { get { return _alarmEditorPanel; } }

    [SerializeField]
    private GameObject _alarmAlertTemplate;

    /// <summary>
    /// 현재 시간에 해당하는 알람 창 띄우는 함수
    /// </summary>
    public void ShowAlarmAlert(Alarm alarm)
    {
        GameObject obj = Instantiate(_alarmAlertTemplate);
        AlarmAlertPanel alarmAlertPanel = obj.GetComponent<AlarmAlertPanel>();
        alarmAlertPanel.AlarmText.text = alarm.name;
        obj.transform.SetParent(transform, false);
        obj.SetActive(true);
    }
}
