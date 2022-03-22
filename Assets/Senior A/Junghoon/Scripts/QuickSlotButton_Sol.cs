using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuickSlotButton_Sol : MonoBehaviour, ISelectHandler, IPointerClickHandler
{
    public enum _ButtonType { Quickslot, Viewport }

    public _ButtonType ButtonType;
    public Image ButtonImage;
    public Text ButtonText;
    public int fid;

    public void OnSelect(BaseEventData eventData)
    {
        if (ButtonType == _ButtonType.Viewport)
            QuickSlotManager_Sol.CurrentlySelected = this;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ButtonType == _ButtonType.Quickslot && QuickSlotManager_Sol.CurrentlySelected != null)
        {
            QuickSlotManager_Sol.AddToQuickSlot(this);
        }
    }
}
