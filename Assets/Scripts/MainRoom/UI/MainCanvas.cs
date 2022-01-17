using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    [SerializeField]
    private MinimapPanel _minimapPanel;

    private MainCanvases _mainCanvases;

    public void FirstInitialize(MainCanvases canvases)
    {
        _mainCanvases = canvases;
    }

    public void OnClick_PlayerInfo()
    {
        _mainCanvases.PlayerInfoCanvas.Show();
    }

    public void OnClick_Setting()
    {
        _mainCanvases.SettingCanvas.Show();
    }

    public void OnClick_Exit()
    {
        // Exit game
    }

    public void OnClick_AvatarInteraction()
    {
        _mainCanvases.AvatarInteractionCanvas.Show();
    }
}
