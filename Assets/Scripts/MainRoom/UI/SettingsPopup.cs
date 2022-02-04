using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XReal.XTown.UI;

public class SettingsPopup : UIPopup
{
    // define all the UI elements relevant.
    enum Buttons
    {
        CloseButton
    }

    private void Start()
    {
        Init(); 

    }

    // overriding Init is optional, but you want to do two things here.
    // 1. Bind the UI elements.
    // 2. Bind the events.
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        // to get a bound gameObject, use GetUIComponent and provide it with UIElementType and UIElementId.
        GetUIComponent<Button>((int)Buttons.CloseButton).gameObject.BindEvent(OnClick_Close);

    }

    // all callbacks registered via BindEvent must receive PointerEventData as the parameter.
    // that was specified in UIEvent's Action definiton.
    public void OnClick_Close(PointerEventData data)
    {
        ClosePopup();
    }
}
