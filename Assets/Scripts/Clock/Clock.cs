using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Clock : MonoBehaviour
{
    public Transform hoursTransform;
    public Transform minutesTransform;
    public Transform secondsTransform;
    
    void Update()
    {   
        hoursTransform.localRotation = Quaternion.Euler(0f, (float)DateTime.Now.TimeOfDay.TotalHours * 30f, 0f);
        minutesTransform.localRotation = Quaternion.Euler(0f, (float)DateTime.Now.TimeOfDay.TotalMinutes * 6f, 0f);
        secondsTransform.localRotation = Quaternion.Euler(0f, (float)DateTime.Now.TimeOfDay.TotalSeconds * 6f, 0f);
    }
}
