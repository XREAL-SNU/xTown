using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleSection : MonoBehaviour
{
    [SerializeField]
    private ScheduleList _scheduleList;
    [SerializeField]
    private ScheduleEdit _scheduleEdit;
    [SerializeField]
    private Button _addButton;
    [SerializeField]
    private Button _backButton;
    [SerializeField]
    private Button _confirmButton;

    // Start is called before the first frame update
    void Start()
    {
        _addButton.onClick.AddListener(OnClick_Add);
        _backButton.onClick.AddListener(OnClick_Back);
        _confirmButton.onClick.AddListener(OnClick_Confirm);
    }

    private void OnClick_Add()
    {
        _scheduleList.Hide();
        _scheduleEdit.Show();
    }
    private void OnClick_Back()
    {
        _scheduleEdit.Hide();
        _scheduleList.Show();
    }
    private void OnClick_Confirm()
    {
        _scheduleEdit.Hide();
        _scheduleList.Show();
    }
}
