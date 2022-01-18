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

    public static event EventHandler<OnTickEventArgs> OnTick;

    public int secPerMin = 60;
    public int minPerHour = 60;
    public int hrPerDay = 24;

    public static int hour;
    public static int minute;
    public static int second;

    public float timer;

    void Awake()
    {
        hour = System.DateTime.Now.Hour;
        minute = System.DateTime.Now.Minute;
        second = System.DateTime.Now.Second;

        timer = second;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= secPerMin)
        {
            timer -= secPerMin;
            minute++;

            if (minute >= minPerHour)
            {
                minute -= minPerHour;
                hour++;
            }

            if (OnTick != null) OnTick(this, new OnTickEventArgs { hour = hour, minute = minute });
        }
    }
}
