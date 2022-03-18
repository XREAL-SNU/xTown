using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
// all UI canvases should derive from this.
namespace XReal.XTown.UI
{
    public abstract class UIBase : MonoBehaviour
    {

        // storage for UIelements, type refers to enum
        protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
        public abstract void Init();

        // T must contain an enum definition for UI element names.
        protected void Bind<T>(Type type) where T : UnityEngine.Object
        {
            string[] names = Enum.GetNames(type);
            UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
            _objects.Add(typeof(T), objects);

            for (int i = 0; i < names.Length; ++i)
            {
                if (typeof(T) == typeof(GameObject))
                { // if T is a gameObject
                    objects[i] = UIUtils.FindUIChild(gameObject, names[i], true);
                }
                else
                { // if T is a component (Button, Image etc)
                    objects[i] = UIUtils.FindUIChild<T>(gameObject, names[i], true);
                }
            }
        }

        // idx is usually an ENUM casted to int. don't worry about memorizing numbers..
        // T is UIStuff,, like Button,Image,, etc.
        // similar to GetComponent, except we give extra search ENUM idx.
        protected T GetUIComponent<T>(int idx) where T: UnityEngine.Object
        {
            UnityEngine.Object[] objects = null;
            if (!_objects.TryGetValue(typeof(T), out objects))
            {
                Debug.LogError($"UIBase/ Cannot find component of type {typeof(T)}");
                return null;
            }

            return objects[idx] as T;
        }

        // Binding event to gameObject. note this is static!!!
        public static void BindEvent(GameObject go, Action<PointerEventData> action, UIEvents.UIEvent type = UIEvents.UIEvent.Click)
        {
            UIEventHandler evt = go.GetComponent<UIEventHandler>();
            if(evt is null)
            {
                Debug.Log("<color = red> Deprecation warning: XReal UI convention requires you to attach UIEventHandler script to " +
                    "interactable UI elements. But we will attach it for you for now. </color>");
                evt = go.AddComponent<UIEventHandler>();
            }

            switch (type)
            {
                case UIEvents.UIEvent.Enter:
                    evt.OnEnterHandler -= action;
                    evt.OnEnterHandler += action;
                    break;
                case UIEvents.UIEvent.Click:
                    evt.OnClickHandler -= action;
                    evt.OnClickHandler += action;
                    break;
                case UIEvents.UIEvent.Exit:
                    evt.OnExitHandler -= action;
                    evt.OnExitHandler += action;
                    break;
                case UIEvents.UIEvent.Drag:
                    evt.OnDragHandler -= action;
                    evt.OnDragHandler += action;
                    break;
            }
        }
    }

}

