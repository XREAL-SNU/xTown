using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmManager : MonoBehaviour
{
    public static List<Alarm> alarmList;
    public static int maxNumber = 10;

    private void Awake()
    {
        alarmList = new List<Alarm>();
    }

    private void OnEnable()
    {
        TimeManager.OnTick += OnTickHandler;
    }

    private void OnDisable()
    {
        TimeManager.OnTick -= OnTickHandler;
    }


    private void OnTickHandler(object sender, TimeManager.OnTickEventArgs e)
    {
        if (alarmList.Count > 0)
        {
            foreach (Alarm alarm in alarmList)
            {
                if (alarm.hour != e.hour) continue;
                if (alarm.minute != e.minute) continue;

                AlarmScript.Instance.AlarmCanvas.ShowAlarmAlert(alarm);
            }
        }
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
    }
}
