using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 시계 및 알람에 사용되는 시간을 관리하는 클래스
/// </summary>
public class TimeManager : MonoBehaviour
{
    /// <summary>
    /// 1분이 지날 때마다 발생시킬 이벤트핸들러에 전달하는 현재 시간에 대한 정보
    /// </summary>
    public class OnTickEventArgs : EventArgs
    {
        public int hour;
        public int minute;
    }

    private int secPerMin = 60;
    private int minPerHour = 60;
    private int hrPerDay = 24;

    private int _hour;
    private int _minute;
    private int _second;

    public int hour { get { return _hour; } }
    public int minute { get { return _minute; } }
    public int second { get { return _second; } }
    public float timer;

    // 1분이 지날 때마다 발생시킬 함수들을 담는 이벤트핸들러
    public static event EventHandler<OnTickEventArgs> OnTick;

    void Awake()
    {
        _hour = System.DateTime.Now.Hour;
        _minute = System.DateTime.Now.Minute;
        _second = System.DateTime.Now.Second;

        timer = _second;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= secPerMin)
        {
            timer -= secPerMin;
            _minute++;

            if (_minute >= minPerHour)
            {
                _minute -= minPerHour;
                _hour++;

                if (_hour >= hrPerDay)
                {
                    _hour -= hrPerDay;
                }
            }

            if (OnTick != null) OnTick(this, new OnTickEventArgs { hour = _hour, minute = _minute });
        }
    }
}
