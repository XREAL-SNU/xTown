using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AlarmListPanel : MonoBehaviour
{
    [SerializeField]
    private Transform _alarmListTransform;

    [SerializeField]
    private TMP_Text _alarmNumberText;

    [SerializeField]
    private GameObject _alarmUITemplate;
    [SerializeField]
    private GameObject _alarmEditorPanel;

    public delegate void OnChangedEvent();
    public static event OnChangedEvent OnChanged;

    public void Start()
    {
        OnChanged += OnUpdateAlarmList;

        OnUpdateAlarmList();
    }

    public void InvokeChangedEvent()
    {
        OnChanged();
    }

    private void OnUpdateAlarmList()
    {
        foreach (Transform t in _alarmListTransform)
        {
            Destroy(t.gameObject);
        }

        DrawAlarmList();
        DrawAlarmNumber();
    }

    private void DrawAlarmList()
    {
        foreach (Alarm alarm in AlarmManager.alarmList)
        {
            DrawAlarm(alarm);
        }
    }

    private void DrawAlarm(Alarm alarm)
    {
        GameObject obj = Instantiate(_alarmUITemplate);
        obj.transform.SetParent(_alarmListTransform, false);
        obj.SetActive(true);

        Color typeColor;
        if (alarm.isPrivate) typeColor = Color.red;
        else typeColor = Color.green;
        obj.transform.GetChild(0).GetComponent<Image>().color = typeColor;

        obj.transform.GetChild(1).GetComponent<TMP_Text>().text = alarm.name;

        string time;

        string amOrPm;
        if (alarm.isAM) amOrPm = "AM";
        else amOrPm = "PM";

        time = amOrPm  + " " + alarm.hour.ToString("D2") + ":" + alarm.minute.ToString("D2");
        obj.transform.GetChild(2).GetComponent<TMP_Text>().text = time;
    }

    private void DrawAlarmNumber()
    {
        _alarmNumberText.text = AlarmManager.alarmList.Count.ToString() + " / " + AlarmManager.maxNumber.ToString();
    }

    #region ButtonCallbacks
    public void OnClick_Add()
    {
        if (AlarmManager.alarmList != null)
        {
            if (AlarmManager.alarmList.Count < AlarmManager.maxNumber)
            {
                AlarmScript.Instance.AlarmCanvas.AlarmEditorPanel.Initialize();
                AlarmScript.Instance.AlarmCanvas.AlarmEditorPanel.Show();
            }
        }
    }
    public void OnClick_Edit()
    {
        GameObject obj = EventSystem.current.currentSelectedGameObject;

        int alarmIndex = obj.transform.GetSiblingIndex();

        Alarm selectedAlarm = AlarmManager.alarmList[alarmIndex];
        AlarmScript.Instance.AlarmCanvas.AlarmEditorPanel.Initialize(alarmIndex, selectedAlarm);
        AlarmScript.Instance.AlarmCanvas.AlarmEditorPanel.Show();
    }
    #endregion

    public void OnClcikShow()
    {
        gameObject.SetActive(!this.gameObject.activeSelf);
        if(_alarmEditorPanel.activeSelf)
        {
            _alarmEditorPanel.SetActive(false);
        }
    }

}
