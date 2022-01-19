using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmCanvas : MonoBehaviour
{
    [SerializeField]
    private AlarmListPanel _alarmListPanel;
    public AlarmListPanel AlarmListPanel { get { return _alarmListPanel; } }

    [SerializeField]
    private AlarmEditorPanel _alarmEditorPanel;
    public AlarmEditorPanel AlarmEditorPanel { get { return _alarmEditorPanel; } }

    public static AlarmCanvas Instance = null;

    private void Awake()
    {
        Debug.Log("Awake called on singleton AlarmCanvas:");
        // singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        FirstInitialize();
    }

    private void FirstInitialize()
    {
        AlarmListPanel.FirstInitialize(this);
        AlarmEditorPanel.FirstInitialize(this);
    }
}
