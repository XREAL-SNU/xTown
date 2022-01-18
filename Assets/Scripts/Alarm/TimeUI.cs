using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    [SerializeField]
    private Text timeText;

    private void OnEnable()
    {
        TimeManager.OnTick += OnTickHandler;
    }

    private void OnDisable()
    {
        TimeManager.OnTick -= OnTickHandler;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateTime(TimeManager.hour, TimeManager.minute);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTickHandler(object sender, TimeManager.OnTickEventArgs e)
    {
        UpdateTime(e.hour, e.minute);
    }

    private void UpdateTime(int hour, int minute)
    {
        timeText.text = $"{hour:00}:{minute:00}";
    }
}
