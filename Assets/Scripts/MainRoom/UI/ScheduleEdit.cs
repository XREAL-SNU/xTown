using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleEdit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
