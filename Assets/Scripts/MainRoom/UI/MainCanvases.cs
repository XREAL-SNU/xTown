using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvases : MonoBehaviour
{
    [SerializeField]
    private MainCanvas _mainCanvas;
    public MainCanvas MainCanvas { get { return _mainCanvas; } }

    [SerializeField]
    private PlayerInfoCanvas _playerInfoCanvas;
    public PlayerInfoCanvas PlayerInfoCanvas { get { return _playerInfoCanvas; } }

    [SerializeField]
    private SettingCanvas _settingCanvas;
    public SettingCanvas SettingCanvas { get { return _settingCanvas; } }

    [SerializeField]
    private AvatarInteractionCanvas _avatarInteractionCanvas;
    public AvatarInteractionCanvas AvatarInteractionCanvas { get { return _avatarInteractionCanvas; } }

    public static MainCanvases Instance = null;

    private void Awake()
    {
        Debug.Log("Awake called on singleton MainCanvases:");
        // singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        //DontDestroyOnLoad(this.gameObject);

        FirstInitialize();
    }

    private void FirstInitialize()
    {
        MainCanvas.FirstInitialize(this);
        PlayerInfoCanvas.FirstInitialize(this);
        SettingCanvas.FirstInitialize(this);
        AvatarInteractionCanvas.FirstInitialize(this);
    }
}
