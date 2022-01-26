using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �˶��� ���� �� �����Ű�� Ŭ����
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
    /// ������ �˶��� �� ���� �ð��� �ش��ϴ� �˶��� �ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    private void OnTickHandler(object sender, TimeManager.OnTickEventArgs e)
    {
        if (alarmList.Count > 0)
        {
            foreach (Alarm alarm in alarmList)
            {
                int hour = alarm.hour;

                // 24�� �������� �˶��� ���� �ð��� ��ȯ�ؾ� ��. TimeManager�� �ð��� 24�� �����̱� ����.
                if (!alarm.isAM) 
                {
                    hour += 12;
                }
                if (hour != e.hour) continue;
                if (alarm.minute != e.minute) continue;

                // AlarmCanvas���� �˶� �г� ����
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
