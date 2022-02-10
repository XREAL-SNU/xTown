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

    [SerializeField]
    private AvatarConfirmCanvas _avatarConfirmCanvas;
    public AvatarConfirmCanvas AvatarConfirmCanvas { get { return _avatarConfirmCanvas; } }

    public static RoomsCanvases Instance = null;
    private void Awake()
    {
        Debug.Log("Awake called on singleton RoomsCanvases:");
        // singleton
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        FirstInitialize();
        /*
        if(PlayerPrefs.GetString("PastScene") == "MainRoom")
        {
            CreateOrJoinRoomCanvas.Show();
        }
        */
    }

    private void FirstInitialize()
    {
        CreateOrJoinRoomCanvas.FirstInitialize(this);
        CurrentRoomCanvas.FirstInitialize(this);
        PlayerNameInputCanvas.FirstInitialize(this);
        AvatarSelectionCanvas.FirstInitialize(this);
        AvatarConfirmCanvas.FirstInitialize(this);
    }
}
