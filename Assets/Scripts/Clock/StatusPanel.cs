using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StatusPanel : MonoBehaviour
{
    public GameObject _alarmPrefab;
    public Transform SpawnPoint;
    private int _numAlarm;
    private static Text _timeText;
    private static Text _alarmText;
    //private GameObject[] _alarms;
    public GameObject _alarmSlot;


    void Awake()
    {
        _timeText = GameObject.Find("Time").GetComponent<Text>();
    }

    public void Show()
    {
        UpdateTime();
    }

    public void UpdateTime()
    {
        if(_timeText!=null){
            _timeText.text = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
        }
    }
    
    public void SetAlarm(string hour,string minute)
    {
        SpawnPoint.Translate(new Vector3(0,_numAlarm*(-8),0));
        _alarmSlot = Instantiate(_alarmPrefab, SpawnPoint.position, Quaternion.identity,this.transform);
        _alarmText=_alarmSlot.GetComponent<Text>();
        _alarmText.text= "Alarm - " + hour + ":" + minute;
        _numAlarm++;

    }

    public void EraseAlarm()
    {

    }
}
