using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// let's define all extensions here.
public static class CommonExtensions
{
    public static void BindEvent(this GameObject go, Action<PointerEventData> action, 
        XReal.XTown.UI.UIEvents.UIEvent eventType = XReal.XTown.UI.UIEvents.UIEvent.Click)
    {
        XReal.XTown.UI.UIBase.BindEvent(go, action, eventType);
    }

    public static void BindEvent(this GameObject go, Action<PlayerInfo> action, RoomManager.RoomEvent eventType)
    {
        RoomManager.BindEvent(go, action, eventType);
    }
}
