using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWatch : MonoBehaviour
{
  public float time;
  public void StopWatchStart()
  {
    StartCoroutine("StopWatchCoroutine");
  }

  public void StopWatchStop()
  {
    StopCoroutine("StopWatchCoroutine");
  }

  public void StopWatchReset()
  {
    time=0;
  }

  IEnumerator StopWatchCoroutine()
  {
    while(true)
    {
      time+= Time.deltaTime;
      yield return null;
    }
  }
}
