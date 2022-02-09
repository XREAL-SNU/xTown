using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{

    [SerializeField]
    private PlayerListingsMenu _playerListingsMenu;
    [SerializeField]
    private LeaveRoomMenu _leaveRoomMenu;
    public LeaveRoomMenu LeaveRoomMenu { get { return _leaveRoomMenu; } }

    private RoomsCanvases _roomCanvases;

    private string _linkedSceneName;
    public string LinkedSceneName
    {
        get => _linkedSceneName;
        set
        {
            Debug.Log("linked current game scene " + value);
            _linkedSceneName = value;
        }
    }
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
        _playerListingsMenu.FirstInitialize(canvases);
        _leaveRoomMenu.FirstInitialize(canvases);
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
