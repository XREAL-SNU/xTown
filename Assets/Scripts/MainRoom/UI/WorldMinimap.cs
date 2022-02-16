using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldMinimap : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData data)
    {
        PlayerMouse.InputLockAll(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerMouse.InputLockAll(false);
    }
}
