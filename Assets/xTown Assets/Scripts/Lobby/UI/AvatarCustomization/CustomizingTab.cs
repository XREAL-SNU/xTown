using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XReal.XTown.UI;

public class CustomizingTab : UIBase
{
    enum GameObjects
    {
        CustomizingTabImage,
        CustomizingTabText,
    }

    private string _partName;
    private CustomizingTabGroup _tabGroup;

    void Start()
    {
        Init();
        Initiallize();
    }

    public override void Init()
    {
        if (_tabGroup is null) _tabGroup = this.transform.parent.parent.GetComponent<CustomizingTabGroup>();

        Bind<GameObject>(typeof(GameObjects));

        GetUIComponent<GameObject>((int)GameObjects.CustomizingTabText).GetComponent<Text>().text = _partName.ToUpper();
        GetUIComponent<GameObject>((int)GameObjects.CustomizingTabText).GetComponent<Text>().fontSize = 18;
        GetUIComponent<GameObject>((int)GameObjects.CustomizingTabImage).gameObject.BindEvent(OnTabSelect);
        GetUIComponent<GameObject>((int)GameObjects.CustomizingTabImage).gameObject.BindEvent(OnTabEnter, UIEvents.UIEvent.Enter);
        GetUIComponent<GameObject>((int)GameObjects.CustomizingTabImage).gameObject.BindEvent(OnTabExit, UIEvents.UIEvent.Exit);

        GetUIComponent<GameObject>((int)GameObjects.CustomizingTabImage).BindEvent((PointerEventData) => { Debug.Log($"Click! {_partName}"); });

        foreach(Transform parent in this.transform.GetComponentsInParent<Transform>())
        {
            if (parent.name.Equals("CustomizeAvatarCanvas"))
            {
                Transform backbtn = parent.transform.Find("CustomizingOther").Find("BackButton").GetComponent<Transform>();
                backbtn.gameObject.BindEvent(InitiallizeButton);
                break;
            }
        }
    }

    public void SetInfo(string name)
    {
        _partName = name;
        this.name = name;
    }

    public void OnTabEnter(PointerEventData data)
    {   
        if (_partName != _tabGroup.SelectedTab)
        {
            GetUIComponent<GameObject>((int)GameObjects.CustomizingTabImage).GetComponent<Image>().color = XTownColor.XTownBlue.ToColor();
            GetUIComponent<GameObject>((int)GameObjects.CustomizingTabImage).GetComponent<Image>().color = XTownColor.ButtonOutlineEnter.ToColor();
        }
    }

    public void OnTabExit(PointerEventData data)
    {
        ResetTabs();
    }

    public void OnTabSelect(PointerEventData data)
    {
        Select();
    }

    public void Select()
    {
        _tabGroup.SelectedTab = _partName;
        ResetTabs();
        _tabGroup.OpenPage();
    }

    public void ResetTabs()
    {
        for (int i = 0; i < this.transform.parent.childCount; i++)
        {
            GameObject btn = this.transform.parent.GetChild(i).gameObject;
            btn.transform.Find("CustomizingTabImage").GetComponent<Image>().color = XTownColor.XTownWhite.ToColor();
            if (btn.GetComponent<CustomizingTab>()._partName.Equals(_tabGroup.SelectedTab)) btn.transform.Find("CustomizingTabImage").GetComponent<Image>().color = XTownColor.ButtonOutlineClick.ToColor();
        }
    }

    public void Initiallize()
    {
        _tabGroup.SelectedTab = this.transform.parent.GetChild(0).name;
        ResetTabs();
        _tabGroup.OpenPage();
    }

    public void InitiallizeButton(PointerEventData data)
    {
        Initiallize();
    }
}
