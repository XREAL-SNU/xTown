using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputLock : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData data)
    {
        PlayerMouse.WheelPlayerMouse.InputLock();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerMouse.WheelPlayerMouse.InputUnLock();
    }
}
