using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XReal.XTown.UI;

public class FCPButton : UIBase
{
    enum GameObjects
    {
        ButtonImage,
        ButtonText
    }

    private string _partName;
    private string _propertyName;
    public int Pick;
    private AvatarAppearanceNew.AppearancePropertyTypes _type;
    private Vector2 _cellSize;
    private CustomizingButtonGroup _cbtnGroup;


    private void Start()
    {
        Init();
        ButtonSize();
    }

    public override void Init()
    {
        if (_cbtnGroup is null) _cbtnGroup = this.transform.parent.parent.GetComponent<CustomizingButtonGroup>();
        Bind<GameObject>(typeof(GameObjects));

        GetUIComponent<GameObject>((int)GameObjects.ButtonText).GetComponent<Text>().text = "FCP";
        GetUIComponent<GameObject>((int)GameObjects.ButtonText).GetComponent<Text>().fontSize = 20;

        GetUIComponent<GameObject>((int)GameObjects.ButtonImage).gameObject.BindEvent(OnButtonSelect);
        GetUIComponent<GameObject>((int)GameObjects.ButtonImage).gameObject.BindEvent(OnButtonEnter, UIEvents.UIEvent.Enter);
        GetUIComponent<GameObject>((int)GameObjects.ButtonImage).gameObject.BindEvent(OnButtonExit, UIEvents.UIEvent.Exit);

        GetUIComponent<GameObject>((int)GameObjects.ButtonImage).BindEvent(SetProperty);

    }

    public void SetInfo(string part, string property, int pick, AvatarAppearanceNew.AppearancePropertyTypes type, Vector2 size)
    {
        _partName = part;
        _propertyName = property;
        Pick = pick;
        _type = type;
        _cellSize = size;
        this.name = "FCPButton";
    }

    public void ButtonSize()
    {
        foreach (Transform child in this.transform.GetComponentsInChildren<Transform>())
        {
            child.GetComponent<RectTransform>().sizeDelta = _cellSize;
        }
    }

    public void SetProperty(PointerEventData data)
    {
        /*
        AvatarAppearanceNew _appearance = new AvatarAppearanceNew();
        GameObject go = AvatarAppearanceNew.CustomParts[_partName];
        //_appearance.SetProperty(go, _propertyName, _type, _paletteName, _pick);
        */
        Debug.Log("partName: " + _partName + "propertyName: " + _propertyName + ", FCP click");
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
        ResetButtons();
    }

    public void OnButtonSelect(PointerEventData data)
    {
        Select();
    }

    public void Select()
    {
        _cbtnGroup.SelectedPick = Pick;
        ResetButtons();
    }

    public void ResetButtons()
    {
        for (int i = 0; i < this.transform.parent.childCount - 1; i++)
        {
            GameObject btn = this.transform.parent.GetChild(i).gameObject;
            btn.transform.Find("ButtonImage").GetComponent<Outline>().effectColor = XTownColor.XTownBlack.ToColor();
            btn.transform.Find("ButtonImage").GetComponent<Outline>().effectDistance = new Vector2(3, -3);
            if (btn.GetComponent<CustomizingButton>() != null && btn.GetComponent<CustomizingButton>().Pick.Equals(_cbtnGroup.SelectedPick))
            {
                btn.transform.Find("ButtonImage").GetComponent<Outline>().effectColor = XTownColor.XTownGreen.ToColor();
                btn.transform.Find("ButtonImage").GetComponent<Outline>().effectDistance = new Vector2(4.5f, -4.5f);
            }
        }
        if (Pick.Equals(_cbtnGroup.SelectedPick))
        {
            GetUIComponent<GameObject>((int)GameObjects.ButtonImage).GetComponent<Outline>().effectColor = XTownColor.XTownGreen.ToColor();
            GetUIComponent<GameObject>((int)GameObjects.ButtonImage).GetComponent<Outline>().effectDistance = new Vector2(4.5f, -4.5f);
        }
        else
        {
            GetUIComponent<GameObject>((int)GameObjects.ButtonImage).GetComponent<Outline>().effectColor = XTownColor.XTownBlack.ToColor();
            GetUIComponent<GameObject>((int)GameObjects.ButtonImage).GetComponent<Outline>().effectDistance = new Vector2(3, -3);
        }
    }
}
