using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlarmListPanel : MonoBehaviour
{
    private AlarmCanvas _alarmCanvas;

    [SerializeField]
    private Transform _alarmListTransform;

    [SerializeField]
    private TMP_Text _alarmNumberText;

    [SerializeField]
    private GameObject _alarmUIPrefab;

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
        foreach (Alarm alarm in AlarmCanvas.alarmList)
        {
            DrawAlarm(alarm);
        }
    }

    private void DrawAlarmNumber()
    {
        _alarmNumberText.text = AlarmCanvas.alarmList.Count.ToString() + " / " + AlarmCanvas.maxNumber.ToString();
    }

    private void DrawAlarm(Alarm alarm)
    {
        // Instantiate single alarm UI element.
        GameObject obj = Instantiate(_alarmUIPrefab);
        obj.transform.SetParent(_alarmListTransform, false);

        // Set alarm type image color.
        Color typeColor;
        if (alarm.isPrivate) typeColor = Color.red;
        else typeColor = Color.green;
        obj.transform.GetChild(0).GetComponent<Image>().color = typeColor;

        // Set alarm name.
        obj.transform.GetChild(1).GetComponent<TMP_Text>().text = alarm.name;

        // Set alarm time.
        string time;

        string amOrPm;
        if (alarm.isAM) amOrPm = "AM";
        else amOrPm = "PM";

        time = amOrPm  + " " + alarm.hour.ToString("D2") + ":" + alarm.minute.ToString("D2");
        obj.transform.GetChild(2).GetComponent<TMP_Text>().text = time;
    }



    public void OnClick_Exit()
    {
        Hide();
    }

    public void OnClick_Add()
    {

        if (AlarmCanvas.alarmList != null)
        {
            if (AlarmCanvas.alarmList.Count < AlarmCanvas.maxNumber)
            {
                AlarmCanvas.Instance.AlarmEditorPanel.InitializeInputs();
                AlarmCanvas.Instance.AlarmEditorPanel.Show();
            }
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

    public void FirstInitialize(AlarmCanvas alarmCanvas)
    {
        _alarmCanvas = alarmCanvas;
    }

}
