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

    public static AlarmCanvas Instance = null;

    public static List<Alarm> alarmList;
    public static int maxNumber = 10;

    private void Awake()
    {
        Debug.Log("Awake called on singleton AlarmCanvas:");
        // singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        FirstInitialize();
    }

    private void FirstInitialize()
    {
        AlarmListPanel.FirstInitialize(this);
        AlarmEditorPanel.FirstInitialize(this);
        alarmList = new List<Alarm>();
    }

    public static void AddAlarm(Alarm alarm)
    {
        alarmList.Add(alarm);
    }

    public static void RemoveAlarm(int i)
    {
        if (alarmList != null)
        {
            alarmList.Remove(alarmList[i]);
        }

        Debug.Log(alarmList.Count);
    }
}
