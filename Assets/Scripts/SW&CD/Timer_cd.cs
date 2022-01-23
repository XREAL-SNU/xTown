using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer_cd : MonoBehaviour
{
    public float timeValue = 4;
    public Text TimerText;
    public InputField InputField_Sec;
    //public InputField InputField_Sec_tmp;
    //private bool timerisActive=true;
    //public string time_tmp;

    //InputField_Sec_tmp = InputField_Sec;
    //timeValue = float.Parse(InputField_Sec.text);
    public void countdownstart()
    {
        //timeValue = float.Parse(InputField_Sec.text);
        if(InputField_Sec.text is null) return;
        timeValue = float.Parse(InputField_Sec.text);
    }

    // Update is called once per frame
    void Update()
    {

        //InputField_Sec_tmp = InputField_Sec;

        if (timeValue>0)
        {
            timeValue-=Time.deltaTime;
        }

        else
        {
            timeValue = 0;
        }
        DisplayTime(timeValue);
    }
    void DisplayTime(float timeToDisplay)
    {

        if(timeToDisplay < 0)
        {
            timeToDisplay=0;
            //timerisActive=false;
        }


        float minutes= Mathf.FloorToInt(timeToDisplay / 60);
        float seconds= Mathf.FloorToInt(timeToDisplay % 60);

        TimerText.text= string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
