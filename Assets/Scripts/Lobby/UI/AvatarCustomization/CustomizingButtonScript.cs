using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingButtonScript : MonoBehaviour
{
    private ColorElement _btnColor = new ColorElement();

    public void Select()
    {
        this.GetComponent<Image>().color = _btnColor.ButtonColorP["Select2"];
    }

    public void Deselect()
    {
        this.GetComponent<Image>().color = _btnColor.ButtonColorP["base"];
    }
}
