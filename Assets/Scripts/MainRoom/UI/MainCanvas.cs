using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XReal.XTown.UI;
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
        //_mainCanvases.PlayerInfoCanvas.Show();
        UIManager.UI.ShowPopupUI<VoiceChatChannelsPopup>("VoiceChatChannelsPopup");
    }

    public void OnClick_Setting()
    {
        //_mainCanvases.SettingCanvas.Show();
        UIManager.UI.ShowPopupUI<SettingsPopup>("SettingsPopup");
    }

    public void OnClick_Exit()
    {
        // Exit game
    }

    public void OnClick_AvatarInteraction()
    {
        _mainCanvases.AvatarInteractionCanvas.Show();
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
