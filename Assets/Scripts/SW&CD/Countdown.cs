using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour 
{
	public float _time = 0;

  	public void CountDownStart(float startTime)
  	{
		_time = startTime; 
    	StartCoroutine("CountDownCoroutine");
  	}
	public void CountDownResume()
	{
		StopCoroutine("CountDownCoroutine");
		StartCoroutine("CountDownCoroutine");
	}

  	public void CountDownStop()
  	{
    	StopCoroutine("CountDownCoroutine");
  	}
  	IEnumerator CountDownCoroutine()
  	{
    	while(_time>=0)
    	{
      		_time-= Time.deltaTime;
      		yield return null;
    	}
  	}
}
