using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingButtonScript : MonoBehaviour
{
    private Color Normal;
    private Color Selected;

    private void Awake()
    {
        Normal = new Color(255, 255, 255, 255f);
        Selected = new Color(255, 255, 255, 100 / 255f);
    }

    public void Select()
    {
        this.gameObject.tag = "Selected";
        this.gameObject.GetComponent<Image>().color = Selected;
    }

    public void Deselect()
    {
        this.gameObject.tag = "Unselected";
        this.gameObject.GetComponent<Image>().color = Normal;
    }
}
