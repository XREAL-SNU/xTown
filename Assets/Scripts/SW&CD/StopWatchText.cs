using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopWatchText : MonoBehaviour
{
  public Text _timer;
  public StopWatch _stopwatch;
  float _time;
  float _msec;
  float _sec;
  float _min;
  public void StopWatchTextStart()
  {
    _stopwatch.StopWatchStart();
  }

  public void StopWatchTextStop()
  {
    _stopwatch.StopWatchStop();
  }

  public void StopWatchTextReset()
  {
    _stopwatch.StopWatchReset();
  }
  public void Update()
  {
    _time = _stopwatch.time;
    _msec=(int)((_time-(int)_time)*100);
    _sec=(int)(_time % 60);
    _min=(int)(_time / 60 % 60);
    _timer.text=string.Format("{0:00}:{1:00}:{2:00}",_min,_sec,_msec);
  }
}
