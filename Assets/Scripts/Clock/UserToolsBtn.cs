using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserToolsBtn : MonoBehaviour
{
    public GameObject AlarmBtn;
    public GameObject CountDownBtn;
    public GameObject StopWatchBtn;
    public void OnClick()
    {
        SetBtnSwitch(AlarmBtn);
        SetBtnSwitch(CountDownBtn);
        SetBtnSwitch(StopWatchBtn);        
    }
    void SetBtnSwitch(GameObject btn)
    {
        bool isActive = btn.activeSelf;
        btn.SetActive(!isActive);
    }
}
