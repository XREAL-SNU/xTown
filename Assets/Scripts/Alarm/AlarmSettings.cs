using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm
{
    public string name;
    public bool isAM;
    public int type;
    public int hour;
    public int minute;
}
public static class AlarmSettings
{
    public static List<Alarm> alarmlist;

    public static void AddAlarm(Alarm alarm)
    {
        alarmlist.Add(alarm);
    }
}
