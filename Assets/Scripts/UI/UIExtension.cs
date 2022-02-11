using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XReal.XTown.UI
{
    public static class Extension
    {
        // 확장 메서드
        public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
        {
            return UIUtils.GetOrAddComponent<T>(go);
        }

        // 확장메서드
        public static void BindEvent(this GameObject go, Action<PointerEventData> action, UIEvents.UIEvent type = UIEvents.UIEvent.Click)
        {
            UIBase.BindEvent(go, action, type);
        }
    }
}