using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 알람을 관리 및 실행시키는 클래스
/// </summary>
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

    /// <summary>
    /// 설정된 알람들 중 현재 시간에 해당하는 알람이 있는지 확인하는 함수
    /// </summary>
    private void OnTickHandler(object sender, TimeManager.OnTickEventArgs e)
    {
        if (alarmList.Count > 0)
        {
            foreach (Alarm alarm in alarmList)
            {
                int hour = alarm.hour;

                // 24시 형식으로 알람의 설정 시간을 변환해야 함. TimeManager의 시간은 24시 형식이기 때문.
                if (!alarm.isAM) 
                {
                    hour += 12;
                }
                if (hour != e.hour) continue;
                if (alarm.minute != e.minute) continue;

                // AlarmCanvas에서 알람 패널 띄우기
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
