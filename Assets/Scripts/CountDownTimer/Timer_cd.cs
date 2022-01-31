using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer_cd : MonoBehaviour
{
    //public float timeValue = 90;
    public Text TimerText;
    public InputField InputField_Sec;
    public InputField InputField_Sec_tmp;


    public void countdownstart()
    {
        //timeValue = float.Parse(InputField_Sec.text);
       // (1) Staic varialbe case
       //Panel2Opener.timeValue = float.Parse(InputField_Sec_tmp.text);
       // (2) GetComponent ccase
        Panel2Opener P2O = GameObject.Find("Panelopenbtn2").GetComponent<Panel2Opener>();
        P2O.timeValue = float.Parse(InputField_Sec_tmp.text);
    }



    // Update is called once per frame
    void Update()
    {

        InputField_Sec_tmp = InputField_Sec;

        // (1) Staic varialbe case
        //DisplayTime(Panel2Opener.timeValue);
        // (2) GetComponent ccase
        Panel2Opener P2O = GameObject.Find("Panelopenbtn2").GetComponent<Panel2Opener>();
        DisplayTime(P2O.timeValue);
    }
    void DisplayTime(float timeToDisplay)
    {

        if(timeToDisplay < 0)
        {
            timeToDisplay=0;

        }


        float minutes= Mathf.FloorToInt(timeToDisplay / 60);
        float seconds= Mathf.FloorToInt(timeToDisplay % 60);

        TimerText.text= string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
