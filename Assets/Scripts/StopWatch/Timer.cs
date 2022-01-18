using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
  public Text timer;
  public float time;
  float msec;
  float sec;
  float min;


  public void StopWatchStart()
  {
    StartCoroutine("StopWatch");

  }

  public void StopWatchStop()
  {
    StopCoroutine("StopWatch");

  }

  public void StopWatchReset()
  {
    time=0;
    timer.text="00:00:00";
  }

  IEnumerator StopWatch()
  {
    while(true)
    {
      time+= Time.deltaTime;
      msec=(int)((time-(int)time)*100);
      sec=(int)(time % 60);
      min=(int)(time / 60 % 60);

      timer.text=string.Format("{0:00}:{1:00}:{2:00}",min,sec,msec);
      yield return null;
    }
  }
}
