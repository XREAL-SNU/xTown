using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public class OnTickEventArgs : EventArgs
    {
        public int second;
    }

    public static event EventHandler<OnTickEventArgs> OnTick;

    [SerializeField]
    private float _timer;

    private float _prevRemTime;
    private bool _timerOn;
    public bool timerOn { get { return _timerOn; } }

    public static TimerController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _timerOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_timerOn)
        {
            return;
        }
        else
        {
            _prevRemTime = Mathf.Floor(_timer);
            _timer -= Time.deltaTime;

            if (_prevRemTime != Mathf.Floor(_timer))
            {
                OnTick(this, new OnTickEventArgs { second = (int)Mathf.Floor(_timer) });

                if (_timer < 1)
                {
                    _timerOn = false;
                    GameManager.Instance.OnTimerFinished();
                    // 타이머 끝났다는 이벤트 날려
                }
            }
        }
    }

    public  void SetTimer(int i)
    {
        _timer = i;
    }

    public void StartTimer()
    {
        _timerOn = true;
    }
}
