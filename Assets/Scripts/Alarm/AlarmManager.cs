using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmManager : MonoBehaviour
{
    [SerializeField]
    private static int _targetHour;
    [SerializeField]
    private static int _targetMinute;

    private int _hour;
    private int _minute;

    private void OnEnable()
    {
        TimeManager.OnTick += OnTickHandler;
    }

    private void OnDisable()
    {
        TimeManager.OnTick -= OnTickHandler;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTickHandler(object sender, TimeManager.OnTickEventArgs e)
    {
        _hour = e.hour;
        _minute = e.minute;

        if (_targetHour != _hour) return;
        if (_targetMinute != _minute) return;

        // 알람에 설정한 시간 도달
        Debug.Log("알람이 울립니다.");

    }

    public static void SetAlarm(int hour, int minute)
    {
        _targetHour = hour;
        _targetMinute = minute;
        Debug.Log("alarm is set");
    }
}
