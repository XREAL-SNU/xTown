using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureButtonScript : MonoBehaviour
{
    private TextureElement _txt = new TextureElement();
    private ColorElement _btnColor = new ColorElement();
    private GridLayoutGroup _gridLayout;
    /*
    private void Start()
    {
        _gridLayout = this.GetComponent<GridLayoutGroup>();
        _resetBtns();
        _panelSize();
    }

    private void _resetBtns()
    {
        for (int i = 0; i < _txt.TextureNum[CustomizingPanelScript.PartID]; i++)
        {
            GameObject button = Instantiate(Resources.Load<GameObject>("CustomizingButton"));
            button.transform.SetParent(this.transform);
            button.GetComponentInChildren<Text>().text = CustomizingPanelScript.PartID + (i + 1);
            button.GetComponentInChildren<Text>().fontSize = 25;
            button.GetComponent<CustomizingButtonScript>().ButtonID = i;
            button.GetComponent<CustomizingButtonScript>().ParentName = this.name;
            if (i == 0) button.GetComponent<Image>().color = _btnColor.ButtonColorP["Select2"];
            else button.GetComponent<Image>().color = _btnColor.ButtonColorP["Base"];
        }
    }
    */
    private void _panelSize()
    {
        int d = (int)_gridLayout.cellSize.y + (int)_gridLayout.spacing.y;
        int a0 = (int)this.GetComponent<RectTransform>().sizeDelta.y + (int)_gridLayout.padding.bottom;
        int panelHeight = Mathf.CeilToInt((float)this.transform.childCount / _gridLayout.constraintCount) * d + a0;
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, panelHeight);
    }
}
