using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour 
{
	public float time;

  	public void CountDownStart(float startTime)
  	{
		time = startTime; 
    	StartCoroutine("StopWatch");
  	}

  	public void CountDownStop()
  	{
    	StopCoroutine("StopWatch");
  	}
  	IEnumerator StopWatch()
  	{
    	while(time>=0)
    	{
      		time-= Time.deltaTime;
      		yield return null;
    	}
  	}
}
