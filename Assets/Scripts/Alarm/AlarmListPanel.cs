using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmListPanel : MonoBehaviour
{
    private AlarmCanvas _alarmCanvas;

    public void OnClick_Exit()
    {
        Hide();
    }

    public void OnClick_Add()
    {
        AlarmCanvas.Instance.AlarmEditorPanel.InitializeInputs();
        AlarmCanvas.Instance.AlarmEditorPanel.Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void FirstInitialize(AlarmCanvas alarmCanvas)
    {
        _alarmCanvas = alarmCanvas;
    }

}
