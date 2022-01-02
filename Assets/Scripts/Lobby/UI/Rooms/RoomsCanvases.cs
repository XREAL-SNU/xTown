using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsCanvases : MonoBehaviour
{
    [SerializeField]
    private CreateOrJoinRoomCanvas _createOrJoinRoomCanvas;
    public CreateOrJoinRoomCanvas CreateOrJoinRoomCanvas { get { return _createOrJoinRoomCanvas; } }

    [SerializeField]
    private CurrentRoomCanvas _currentRoomCanvas;
    public CurrentRoomCanvas CurrentRoomCanvas { get { return _currentRoomCanvas; } }

    [SerializeField]
    private PlayerNameInputCanvas _playerNameInputCanvas;
    public PlayerNameInputCanvas PlayerNameInputCanvas { get { return _playerNameInputCanvas; } }

    [SerializeField]
    private AvatarSelectionCanvas _avatarSelectionCanvas;
    public AvatarSelectionCanvas AvatarSelectionCanvas { get { return _avatarSelectionCanvas; } }

    private void Awake()
    {
        FirstInitialize();
        if(PlayerPrefs.GetString("PastScene") == "MainRoom")
        {
            CreateOrJoinRoomCanvas.Show();
        }
    }

    private void FirstInitialize()
    {
        CreateOrJoinRoomCanvas.FirstInitialize(this);
        CurrentRoomCanvas.FirstInitialize(this);
        PlayerNameInputCanvas.FirstInitialize(this);
        AvatarSelectionCanvas.FirstInitialize(this);
    }
}
