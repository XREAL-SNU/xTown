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
    private int _pick;
    private AvatarAppearanceNew.AppearancePropertyTypes _type;


    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        GetUIComponent<GameObject>((int)GameObjects.ButtonText).GetComponent<Text>().text = _colorName;
        GetUIComponent<GameObject>((int)GameObjects.ButtonImage).BindEvent(SetProperty);
        
    }

    public void SetInfo(string part, string property, string name, string palette, int pick, AvatarAppearanceNew.AppearancePropertyTypes type)
    {
        _partName = part;
        _propertyName = property;
        this.name = name;
        _colorName = name;
        _paletteName = palette;
        _pick = pick;
        _type = type;
    }

    public void SetProperty(PointerEventData data)
    {
        AvatarAppearanceNew _appearance = new AvatarAppearanceNew();
        GameObject go = AvatarAppearanceNew.CustomParts[_partName];
        _appearance.SetProperty(go, _propertyName, _type, _paletteName, _pick);
        Debug.Log("propertyName: " + _propertyName + ", paletteName: " + _paletteName + ", pick: " + _pick);
    }
}
