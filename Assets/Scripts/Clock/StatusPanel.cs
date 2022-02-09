using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StatusPanel : MonoBehaviour
{
    private static Text _timeText;


    void Awake()
    {
        _timeText = GameObject.Find("Time").GetComponent<Text>();
    }

    public void UpdateTime()
    {
        if(_timeText!=null){
            _timeText.text = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
        }
    }
    
}
