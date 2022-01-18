using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmManager : MonoBehaviour
{
    [SerializeField]
    private int targetHour;
    [SerializeField]
    private int targetMinute;

    private int hour;
    private int minute;

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
        hour = e.hour;
        minute = e.minute;

        if (targetHour != hour) return;
        if (targetMinute != minute) return;

    }

    public void SetAlarm(int hour, int minute)
    {
        targetHour = hour;
        targetMinute = minute;
        Debug.Log("alarm is set");
    }
}
