using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingButtonScript : MonoBehaviour
{
    private Color _normal;
    private Color _selected;

    private void Awake()
    {
        _normal = new Color(255 / 255, 255 / 255, 255 / 255, 255 / 255);
        _selected = new Color(255 / 255, 255 / 255, 255 / 255, 100 / 255f);
    }

    public void Select()
    {
        this.gameObject.tag = "Selected";
        this.gameObject.GetComponent<Image>().color = _selected;
    }

    public void Deselect()
    {
        this.gameObject.tag = "Unselected";
        this.gameObject.GetComponent<Image>().color = _normal;
    }
}
