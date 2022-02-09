using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButtonScript : MonoBehaviour
{
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
        int id = 0;
        foreach (KeyValuePair<string, Color> pair in _btnColor.AvatarColorP)
        {
            GameObject button = Instantiate(Resources.Load<GameObject>("CustomizingButton"));
            button.transform.SetParent(this.transform);
            button.GetComponentInChildren<Text>().text = pair.Key;
            button.GetComponentInChildren<Text>().fontSize = 20;
            button.GetComponent<CustomizingButtonScript>().ButtonID = id;
            button.GetComponent<CustomizingButtonScript>().ParentName = this.name;
            Image img = button.GetComponent<Image>();
            img.color = pair.Value;
            if (id == 0) img.color = new Color(img.color.r, img.color.g, img.color.b, 100 / 255f);
            id++;
        }
        GameObject fcpbutton = Instantiate(Resources.Load<GameObject>("fcpButton"));
        fcpbutton.transform.SetParent(this.transform);
        fcpbutton.GetComponentInChildren<Text>().text = "Color\nPicker";
        fcpbutton.GetComponentInChildren<Text>().fontSize = 20;
        fcpbutton.GetComponent<CustomizingButtonScript>().ButtonID = 7;
        fcpbutton.GetComponent<CustomizingButtonScript>().ParentName = this.name;
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
