using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmScript : MonoBehaviour
{
    [SerializeField]
    private TimeManager _timeManager;
    public TimeManager TimeManager { get { return _timeManager; } }

    [SerializeField]
    private AlarmCanvas _alarmCanvas;
    public AlarmCanvas AlarmCanvas { get { return _alarmCanvas; } }

    public static AlarmScript Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
