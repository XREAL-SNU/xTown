using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PanelsManager: MonoBehaviour
{
    [Header("UI")]
    public GameObject LoginPanel;
    public GameObject MenuPanel;
    public GameObject CreateRoomPanel;
    public GameObject RoomListPanel;

    /* singleton interface */
    public static PanelsManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SetActivePanel("LoginPanel");
    }
    public void SetActivePanel(string activePanel)
    {
        LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
        MenuPanel.SetActive(activePanel.Equals(MenuPanel.name));
        CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name)); 
        RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));  
    }
}
