using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XReal.XTown.UI;

public class VoiceChatChannelsPopup : UIPopup
{

    List<PlayerInfo> _playerInfos;
    enum Buttons
    {
        CloseButton
    }



    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        // to get a bound gameObject, use GetUIComponent and provide it with UIElementType and UIElementId.
        GetUIComponent<Button>((int)Buttons.CloseButton).gameObject.BindEvent(OnClick_Close);
        gameObject.BindEvent(OnPlayerJoined_AddItem, RoomManager.RoomEvent.PlayerJoined);
        gameObject.BindEvent(OnPlayerLeft_RemoveItem, RoomManager.RoomEvent.PlayerLeft);
        _playerInfos = RoomManager.Room.GetPlayerInfoList();
        foreach(PlayerInfo info in _playerInfos)
        {
            Debug.Log($"player: {info.PlayerName}");
        }
    }

    public void OnPlayerJoined_AddItem(PlayerInfo info)
    {
        Debug.Log($"Player Joined {info.PlayerName}");
    }

    public void OnPlayerLeft_RemoveItem(PlayerInfo info)
    {
        Debug.Log($"Player Left {info.PlayerName}");
    }

    public void OnClick_Close(PointerEventData data)
    {
        ClosePopup();
    }
}

