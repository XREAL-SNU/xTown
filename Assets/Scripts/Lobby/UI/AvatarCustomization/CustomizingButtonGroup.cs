using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XReal.XTown.UI;

public class CustomizingButtonGroup : UIBase
{
    enum GameObjects
    {
        GridPanel
    }

    private string _partName;
    private string _propertyName;
    private int _partsIndex;
    private int _componentCount;

    private GridLayoutGroup _gridLayout;
    private Vector2 _cellSize;

    public int SelectedPick = 0;

    private void Start()
    {
        Init();
        _panelSize();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        GameObject gridPanel = GetUIComponent<GameObject>((int)GameObjects.GridPanel);
        gridPanel.GetComponent<Text>().text = _propertyName;
        gridPanel.GetComponent<Text>().fontSize = 45;
        _gridLayout = gridPanel.GetComponent<GridLayoutGroup>();

        foreach(ObjectPartProperty props in PlayerManager.Players.LocalAvatarAppearance.Descriptor.Parts[_partsIndex].Properties)
        {
            if (props.PropertyName == _propertyName)
            {
                AvatarAppearanceNew.AppearancePropertyTypes type = (AvatarAppearanceNew.AppearancePropertyTypes)Enum.Parse(typeof(AvatarAppearanceNew.AppearancePropertyTypes), props.PropertyType, true);
                string paletteName = props.PaletteName;
                int pick = 0;
                switch (type)
                {
                    case AvatarAppearanceNew.AppearancePropertyTypes.BaseColor:
                        SetColGrid();
                        foreach (XTownColor col in ColorPalette.GetXrealPalette(paletteName).ColorsSet)
                        {
                            GameObject buttons = UIManager.UI.MakeSubItem<CustomizingButton>(gridPanel.transform).gameObject;
                            CustomizingButton button = buttons.GetOrAddComponent<CustomizingButton>();
                            button.SetInfo(_partName, _propertyName, col.colorName, paletteName, pick, _partsIndex, type, _cellSize);
                            pick++;
                        }
                        GameObject fcps = UIManager.UI.MakeSubItem<FCPButton>(gridPanel.transform).gameObject;
                        FCPButton fcp = fcps.GetOrAddComponent<FCPButton>();
                        fcp.SetInfo(_partName, _propertyName, pick, type, _cellSize);
                        break;
                    case AvatarAppearanceNew.AppearancePropertyTypes.Metallic:
                    case AvatarAppearanceNew.AppearancePropertyTypes.Emission:
                    case AvatarAppearanceNew.AppearancePropertyTypes.Transparency:
                        SetColGrid();
                        foreach (float val in LinearPalette.GetXrealPalette(paletteName).ValuesSet)
                        {
                            GameObject buttons = UIManager.UI.MakeSubItem<CustomizingButton>(gridPanel.transform).gameObject;
                            CustomizingButton button = buttons.GetOrAddComponent<CustomizingButton>();
                            button.SetInfo(_partName, _propertyName, val.ToString(), paletteName, pick, _partsIndex, type, _cellSize);
                            pick++;
                        }
                        break;
                }
                _componentCount = pick - 1;
                break;
            }
        }
        
    }

    public void SetInfo(string part, string property, int index)
    {
        _partName = part;
        _propertyName = property;
        this.name = property;
        _partsIndex = index;
    }

    public void SetColGrid()
    {
        _gridLayout.padding = new RectOffset(0, 0, 70, 15);
        _gridLayout.cellSize = new Vector2(97, 97);
        _cellSize = _gridLayout.cellSize;
        _gridLayout.spacing = new Vector2(15, 15);
        _gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
        _gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
        _gridLayout.childAlignment = TextAnchor.UpperLeft;
        _gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayout.constraintCount = 8;
    }

    public void SetTexGrid()
    {
        _gridLayout.padding = new RectOffset(15, 15, 70, 15);
        _gridLayout.cellSize = new Vector2(178, 178);
        _cellSize = _gridLayout.cellSize;
        _gridLayout.spacing = new Vector2(45, 45);
        _gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
        _gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
        _gridLayout.childAlignment = TextAnchor.UpperLeft;
        _gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayout.constraintCount = 4;
    }

    private void _panelSize()
    {
        GameObject gridPanel = GetUIComponent<GameObject>((int)GameObjects.GridPanel);
        int d = (int)_gridLayout.cellSize.y + (int)_gridLayout.spacing.y;
        int a0 = (int)gridPanel.GetComponent<RectTransform>().sizeDelta.y + (int)_gridLayout.padding.bottom;
        int panelHeight = Mathf.CeilToInt((float)gridPanel.transform.childCount / _gridLayout.constraintCount) * d + a0;
        gridPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(gridPanel.GetComponent<RectTransform>().sizeDelta.x, panelHeight);
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(gridPanel.GetComponent<RectTransform>().sizeDelta.x, panelHeight);

        this.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = true;
    }

    public void RandomCustomizing(PointerEventData data)
    {
        int rand = UnityEngine.Random.Range(0, _componentCount);
        GetUIComponent<GameObject>((int)GameObjects.GridPanel).transform.GetChild(rand).GetComponent<CustomizingButton>().Select();
        GetUIComponent<GameObject>((int)GameObjects.GridPanel).transform.GetChild(rand).GetComponent<CustomizingButton>().SetProperty();
    }
}
