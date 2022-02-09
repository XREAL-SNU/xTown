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

    private string _name;
    private CustomizingTabGroup TabGroup;

    void Start()
    {
        Init();
        TabGroup = this.transform.parent.parent.GetComponent<CustomizingTabGroup>();
        Initiallize();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        GetUIComponent<GameObject>((int)GameObjects.CustomizingTabText).GetComponent<Text>().text = _name;
        GetUIComponent<GameObject>((int)GameObjects.CustomizingTabText).GetComponent<Text>().fontSize = 25;
        GetUIComponent<GameObject>((int)GameObjects.CustomizingTabImage).gameObject.BindEvent(OnTabSelect);
        GetUIComponent<GameObject>((int)GameObjects.CustomizingTabImage).gameObject.BindEvent(OnTabEnter, UIEvents.UIEvent.Enter);
        GetUIComponent<GameObject>((int)GameObjects.CustomizingTabImage).gameObject.BindEvent(OnTabExit, UIEvents.UIEvent.Exit);

        GetUIComponent<GameObject>((int)GameObjects.CustomizingTabImage).BindEvent((PointerEventData) => { Debug.Log($"Click! {_name}"); });
    }

    public void SetInfo(string name)
    {
        _name = name;
        this.name = name;
    }

    public void OnTabEnter(PointerEventData data)
    {   
        if (_name != TabGroup.SelectedTab)
        {
            GetUIComponent<GameObject>((int)GameObjects.CustomizingTabImage).GetComponent<Image>().color = XTownColor.XTownBlue.ToColor();
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
        TabGroup.SelectedTab = _name;
        ResetTabs();
        TabGroup.OpenPage();
    }

    public void ResetTabs()
    {
        for (int i = 0; i < this.transform.parent.childCount; i++)
        {
            GameObject btn = this.transform.parent.GetChild(i).gameObject;
            btn.transform.GetChild(0).GetComponent<Image>().color = XTownColor.XTownWhite.ToColor();
            if (btn.GetComponent<CustomizingTab>()._name == TabGroup.SelectedTab) btn.transform.GetChild(0).GetComponent<Image>().color = XTownColor.XTownGreen.ToColor();
        }
    }

    public void Initiallize()
    {
        TabGroup.SelectedTab = this.transform.parent.GetChild(0).name;
        ResetTabs();
        TabGroup.OpenPage();
    }

    public void InitiallizeButton(PointerEventData data)
    {
        Initiallize();
    }

    /*
    public TabGroup TabGroup;
    public Image Background;




    public void OnPointerClick(PointerEventData eventData)
    {
        TabGroup.OnTabSelect(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TabGroup.OnTabExit(this);
    }

    private void Start()
    {
        Background = GetComponent<Image>();
        TabGroup.Subscribe(this);
    }*/
}
