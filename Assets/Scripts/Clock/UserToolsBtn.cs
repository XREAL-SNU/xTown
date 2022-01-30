using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UserToolsBtn : MonoBehaviour
{
    public GameObject AlarmBtn;
    public GameObject CountDownBtn;
    public GameObject StopWatchBtn;
    [Range(0.1f,1f),SerializeField]
    private float _moveDuration = 1.0f;
    [SerializeField]
    private Ease _moveEase = Ease.Linear;
    [SerializeField]
    private Vector3 _originLocation;
    [SerializeField]
    private Vector3 _alarmLocation;
    [SerializeField]
    private Vector3 _countdownLocation;
    [SerializeField]
    private Vector3 _stopwatchLocation;
    private bool _switch = false;
    public void OnClick()
    {
        if(!_switch)
        {
            AlarmBtn.SetActive(true);
            CountDownBtn.SetActive(true);
            StopWatchBtn.SetActive(true);
            AlarmBtn.transform.DOMove(_alarmLocation,_moveDuration).SetEase(_moveEase);
            CountDownBtn.transform.DOMove(_countdownLocation,_moveDuration).SetEase(_moveEase);
            StopWatchBtn.transform.DOMove(_stopwatchLocation,_moveDuration).SetEase(_moveEase);
        }
        else
        {
            AlarmBtn.transform.DOMove(_originLocation,_moveDuration).SetEase(_moveEase);
            CountDownBtn.transform.DOMove(_originLocation,_moveDuration).SetEase(_moveEase);
            StopWatchBtn.transform.DOMove(_originLocation,_moveDuration).SetEase(_moveEase);
            AlarmBtn.SetActive(false);
            CountDownBtn.SetActive(false);
            StopWatchBtn.SetActive(false);
        }
        _switch = !_switch;     
    }
}
