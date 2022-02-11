using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XReal.XTown.UI;

public class CustomizingButton : UIBase
{
    enum GameObjects
    {
        ButtonImage,
        ButtonText
    }

    private string _partName;
    private string _propertyName;
    private string _paletteName;
    private string _colorName;
    private int _partsIndex;
    public int Pick;
    private int _componentCount;

    private AvatarAppearanceNew.AppearancePropertyTypes _type;
    private Vector2 _cellSize;
    private CustomizingButtonGroup _cbtnGroup;


    private void Start()
    {
        Init();
        ButtonSize();
        Initiallize();
    }

    public override void Init()
    {
        if (_cbtnGroup is null) _cbtnGroup = this.transform.parent.parent.GetComponent<CustomizingButtonGroup>();
        _componentCount = this.transform.parent.childCount;
        Bind<GameObject>(typeof(GameObjects));

        GetUIComponent<GameObject>((int)GameObjects.ButtonText).GetComponent<Text>().text = _colorName;
        GetUIComponent<GameObject>((int)GameObjects.ButtonText).GetComponent<Text>().fontSize = 20;

        GetUIComponent<GameObject>((int)GameObjects.ButtonImage).gameObject.BindEvent(OnButtonSelect);
        GetUIComponent<GameObject>((int)GameObjects.ButtonImage).gameObject.BindEvent(OnButtonEnter, UIEvents.UIEvent.Enter);
        GetUIComponent<GameObject>((int)GameObjects.ButtonImage).gameObject.BindEvent(OnButtonExit, UIEvents.UIEvent.Exit);

        GetUIComponent<GameObject>((int)GameObjects.ButtonImage).BindEvent(SetPropertyButton);
        
        switch (_type)
        {
            case AvatarAppearanceNew.AppearancePropertyTypes.BaseColor:
                GetUIComponent<GameObject>((int)GameObjects.ButtonImage).GetComponent<Image>().color = ColorPalette.GetXrealPalette(_paletteName)[Pick].ToColor();
                break;
            case AvatarAppearanceNew.AppearancePropertyTypes.Metallic:
            case AvatarAppearanceNew.AppearancePropertyTypes.Emission:
            case AvatarAppearanceNew.AppearancePropertyTypes.Transparency:
                GetUIComponent<GameObject>((int)GameObjects.ButtonImage).GetComponent<Image>().color = new XTownColor(LinearPalette.GetXrealPalette(_paletteName)[Pick]).ToColor();
                GetUIComponent<GameObject>((int)GameObjects.ButtonText).GetComponent<Text>().fontSize = 20;
                break;
        }

        foreach (Transform parent in this.transform.GetComponentsInParent<Transform>())
        {
            if (parent.name.Equals("CustomizeAvatarCanvas"))
            {
                Transform backbtn = parent.transform.Find("CustomizingOther").Find("BackButton").GetComponent<Transform>();
                backbtn.gameObject.BindEvent(InitiallizeButton);
                if (this.Pick.Equals(0)) backbtn.gameObject.BindEvent(SetPropertyButton);

                Transform page = parent.transform.Find("CustomizingTabGroup").Find("PagePanel").GetChild(_partsIndex).GetComponent<Transform>();
                page.GetChild(0).Find("ResetButton").gameObject.BindEvent(InitiallizeButton);
                if (this.Pick.Equals(0)) page.GetChild(0).Find("ResetButton").gameObject.BindEvent(SetPropertyButton);

                page.GetChild(0).Find("RandomButton").gameObject.BindEvent(RandomCustomizingButton);
                break;
            }
        }
    }

    public void SetInfo(string part, string property, string name, string palette, int pick, int index, AvatarAppearanceNew.AppearancePropertyTypes type, Vector2 size)
    {
        _partName = part;
        _propertyName = property;
        this.name = name;
        _colorName = name;
        _paletteName = palette;
        Pick = pick;
        _partsIndex = index;
        _type = type;
        _cellSize = size;
    }

    public void ButtonSize()
    {
        foreach(Transform child in this.transform.GetComponentsInChildren<Transform>())
        {
            child.GetComponent<RectTransform>().sizeDelta = _cellSize;
        }
    }

    public void SetPropertyButton(PointerEventData data)
    {
        SetProperty();
    }

    public void SetProperty()
    {
        AvatarAppearanceNew _appearance = PlayerManager.Players.LocalAvatarAppearance;
        GameObject go = _appearance.CustomParts[_partName];
        _appearance.SetProperty(go, _propertyName, _type, _paletteName, Pick);
        //Debug.Log("propertyName: " + _propertyName + ", paletteName: " + _paletteName + ", pick: " + Pick);

    }

    public void OnButtonEnter(PointerEventData data)
    {
        if (Pick != _cbtnGroup.SelectedPick)
        {
            GetUIComponent<GameObject>((int)GameObjects.ButtonImage).GetComponent<Outline>().effectColor = XTownColor.XTownBlue.ToColor();
            GetUIComponent<GameObject>((int)GameObjects.ButtonImage).GetComponent<Outline>().effectDistance = new Vector2(4.5f, -4.5f);
        }
    }

    public void OnButtonExit(PointerEventData data)
    {
        ResetCustomizing();
    }

    public void OnButtonSelect(PointerEventData data)
    {
        Select();
    }

    public void Select()
    {
        _cbtnGroup.SelectedPick = Pick;
        ResetCustomizing();
    }

    public void ResetCustomizing()
    {

        for (int i = 0; i < _componentCount; i++)
        {
            GameObject btn = this.transform.parent.GetChild(i).gameObject;
            btn.transform.Find("ButtonImage").GetComponent<Outline>().effectColor = XTownColor.XTownBlack.ToColor();
            btn.transform.Find("ButtonImage").GetComponent<Outline>().effectDistance = new Vector2(3, -3);
            if (btn.GetComponent<CustomizingButton>() != null && btn.GetComponent<CustomizingButton>().Pick.Equals(_cbtnGroup.SelectedPick))
            {
                btn.transform.Find("ButtonImage").GetComponent<Outline>().effectColor = XTownColor.XTownGreen.ToColor();
                btn.transform.Find("ButtonImage").GetComponent<Outline>().effectDistance = new Vector2(4.5f, -4.5f);
            }
            if (btn.GetComponent<FCPButton>() != null && btn.GetComponent<FCPButton>().Pick.Equals(_cbtnGroup.SelectedPick))
            {
                btn.transform.Find("ButtonImage").GetComponent<Outline>().effectColor = XTownColor.XTownGreen.ToColor();
                btn.transform.Find("ButtonImage").GetComponent<Outline>().effectDistance = new Vector2(4.5f, -4.5f);
            }
        }
    }

    public void Initiallize()
    {
        _cbtnGroup.SelectedPick = 0;
        ResetCustomizing();
    }

    public void InitiallizeButton(PointerEventData data)
    {
        Initiallize();
    }

    public void RandomCustomizingButton(PointerEventData data)
    {
        int rand = UnityEngine.Random.Range(0, _componentCount);
        //this.transform.parent.parent.GetComponent<CustomizingButtonGroup>().SelectedPick = rand;
    }
}
