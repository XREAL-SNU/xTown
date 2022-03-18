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
    private string _colorRename;
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
        Bind<GameObject>(typeof(GameObjects));

        GetUIComponent<GameObject>((int)GameObjects.ButtonText).GetComponent<Text>().text = Rename(_colorName);
        GetUIComponent<GameObject>((int)GameObjects.ButtonText).GetComponent<Text>().fontSize = 10;

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
                GetUIComponent<GameObject>((int)GameObjects.ButtonText).GetComponent<Text>().fontSize = 10;
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
                if (this.Pick.Equals(0)) page.GetChild(0).Find("RandomButton").gameObject.BindEvent(RandomCustomizingButton);
                break;
            }
        }
    }

    public void SetInfo(string part, string property, string name, string palette, int pick, int index, int count, AvatarAppearanceNew.AppearancePropertyTypes type, Vector2 size)
    {
        _partName = part;
        _propertyName = property;
        this.name = name;
        _colorName = name;
        _paletteName = palette;
        Pick = pick;
        _partsIndex = index;
        _componentCount = count;
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
            GetUIComponent<GameObject>((int)GameObjects.ButtonImage).GetComponent<Outline>().effectColor = XTownColor.ButtonOutlineEnter.ToColor();
            GetUIComponent<GameObject>((int)GameObjects.ButtonImage).GetComponent<Outline>().effectDistance = new Vector2(4.5f, -4.5f);
        }
    }

    public void OnButtonExit(PointerEventData data)
    {
        GetUIComponent<GameObject>((int)GameObjects.ButtonImage).GetComponent<Outline>().effectColor = new Color(0, 0, 0, 0);
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
            btn.transform.Find("ButtonImage").GetComponent<Outline>().effectColor = XTownColor.ButtonOutlineDefault.ToColor();
            btn.transform.Find("ButtonImage").GetComponent<Outline>().effectDistance = new Vector2(3, -3);
            if (btn.GetComponent<CustomizingButton>() != null && btn.GetComponent<CustomizingButton>().Pick.Equals(_cbtnGroup.SelectedPick))
            {
                btn.transform.Find("ButtonImage").GetComponent<Outline>().effectColor = XTownColor.ButtonOutlineClick.ToColor();
                btn.transform.Find("ButtonImage").GetComponent<Outline>().effectDistance = new Vector2(4.5f, -4.5f);
            }
            /*
            if (btn.GetComponent<FCPButton>() != null && btn.GetComponent<FCPButton>().Pick.Equals(_cbtnGroup.SelectedPick))
            {
                btn.transform.Find("ButtonImage").GetComponent<Outline>().effectColor = XTownColor.XTownGreen.ToColor();
                btn.transform.Find("ButtonImage").GetComponent<Outline>().effectDistance = new Vector2(4.5f, -4.5f);
            }*/
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
        if (this.Pick.Equals(0)) SetProperty();
    }

    public void RandomCustomizingButton(PointerEventData data)
    {
        int rand = UnityEngine.Random.Range(0, _componentCount);
        _cbtnGroup.SelectedPick = rand;
        foreach (Transform btn in this.transform.parent.GetComponentsInChildren<Transform>())
        {
            CustomizingButton script = btn.GetComponent<CustomizingButton>();
            if (script != null && script.Pick.Equals(_cbtnGroup.SelectedPick))
            {
                script.ResetCustomizing();
                script.SetProperty();
            }
        }
    }

    public string Rename(string name)
    {
        if (_propertyName.Equals("Color") && name.Length != 1)
        {
            string upper = name.ToUpper();
            List<string> rename = new List<string>();
            int flag = 0;
            int len = 1;
            for (int i = 1; i < name.Length - 1; i++)
            {
                len++;
                if (!name[i].Equals(upper[i]) && name[i + 1].Equals(upper[i + 1]))
                {
                    rename.Add(name.Substring(flag, len));
                    rename.Add("\n");
                    flag = i + 1;
                    len = 0;
                }
                if (i.Equals(name.Length - 2))
                {
                    rename.Add(name.Substring(flag, len + 1));
                }
            }
            if (rename.Count != 0)
            {
                foreach (string sub in rename)
                {
                    _colorRename += sub;
                }
                return _colorRename;
            }
            else return _colorName;
        }
        else return _colorName;
    }
}
