using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
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
            }

            if (OnTick != null) OnTick(this, new OnTickEventArgs { hour = _hour, minute = _minute });
        }
    }
}
