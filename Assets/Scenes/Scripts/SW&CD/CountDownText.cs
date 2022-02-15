using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownText : MonoBehaviour
{
    public Text _timer;
    public InputField _inputField_Sec;
    public CountDown _countdown;
    public GameObject _startButton;
    float _min;
    float _sec;
    //public InputField InputField_Sec_tmp;
    //private bool timerisActive=true;
    //public string time_tmp;

    //InputField_Sec_tmp = InputField_Sec;
    //timeValue = float.Parse(InputField_Sec.text);
    public void CountDownTextStart()
    {
        //timeValue = float.Parse(InputField_Sec.text);
        if(_inputField_Sec.text is null) return;
        _countdown.CountDownStart(float.Parse(_inputField_Sec.text));
    }
    public void CountDownTextResume()
    {
        _countdown.CountDownResume();
    }
    public void CountDownTextStop()
    {
        _countdown.CountDownStop();
    }

    // Update is called once per frame
    void Update()
    {
        if(!(_countdown is null)){
            DisplayTime(_countdown._time);
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay=0;
        }
        _min = Mathf.FloorToInt(timeToDisplay / 60);
        _sec = Mathf.FloorToInt(timeToDisplay % 60);
        _timer.text= string.Format("{0:00}:{1:00}", _min, _sec);
    }
}
