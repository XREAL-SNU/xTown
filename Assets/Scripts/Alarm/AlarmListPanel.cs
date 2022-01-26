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

    public delegate void OnChangedEvent();
    // 알람 리스트가 변경 및 수정되었을 때 발생시킬 함수들을 담는 이벤트핸들러
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

    // 알람 리스트 UI를 업데이트하는 함수
    private void OnUpdateAlarmList()
    {
        foreach (Transform t in _alarmListTransform)
        {
            Destroy(t.gameObject);
        }

        DrawAlarmList();
        DrawAlarmNumber();
    }

    // 알람 리스트 스크롤 뷰에서 전체 알람 목록을 그리는 함수
    private void DrawAlarmList()
    {
        foreach (Alarm alarm in AlarmManager.alarmList)
        {
            DrawAlarm(alarm);
        }
    }

    // 알람 리스트 스크롤 뷰에서 단일 알람 목록을 그리는 함수
    private void DrawAlarm(Alarm alarm)
    {
        // Instantiate single alarm UI element.
        GameObject obj = Instantiate(_alarmUITemplate);
        obj.transform.SetParent(_alarmListTransform, false);
        obj.SetActive(true);

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

    // 하단에 전체 알람 갯수를 업데이트하는 함수
    private void DrawAlarmNumber()
    {
        _alarmNumberText.text = AlarmManager.alarmList.Count.ToString() + " / " + AlarmManager.maxNumber.ToString();
    }

    #region ButtonCallbacks
    // Add alarm 버튼을 눌렀을 때 
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
    
    // 알람 목록에서 기존 알람을 눌렀을 때
    public void OnClick_Edit()
    {
        GameObject obj = EventSystem.current.currentSelectedGameObject;

        int alarmIndex = obj.transform.GetSiblingIndex();

        Alarm selectedAlarm = AlarmManager.alarmList[alarmIndex];
        AlarmScript.Instance.AlarmCanvas.AlarmEditorPanel.Initialize(alarmIndex, selectedAlarm);
        AlarmScript.Instance.AlarmCanvas.AlarmEditorPanel.Show();
    }

    // Close 버튼을 눌렀을 때
    public void OnClick_Exit()
    {
        Hide();
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
}
