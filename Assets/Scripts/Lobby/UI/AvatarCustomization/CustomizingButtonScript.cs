using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingButtonScript : MonoBehaviour
{
    private ColorElement _btnColor = new ColorElement();
    public string ParentName;
    public int ButtonID;

    public void IsClickButton()
    {
        switch (ParentName)
        {
            case "TextureGroup":
                this.transform.parent.parent.GetComponent<CustomizingPanelScript>()._selectedTex = ButtonID;
                break;
            case "ColorGroup":
                this.transform.parent.parent.GetComponent<CustomizingPanelScript>()._selectedCol = ButtonID;
                break;
        }
    }

    public void Select()
    {
        switch (ParentName)
        {
            case "TextureGroup":
                for (int i = 0; i < this.transform.parent.childCount; i++)
                {
                    Button btn = this.transform.parent.GetChild(i).GetComponent<Button>();
                    btn.GetComponent<Image>().color = _btnColor.ButtonColorP["Base"];
                    btn.enabled = true;
                }
                this.GetComponent<Image>().color = _btnColor.ButtonColorP["Select2"];
                this.GetComponent<Button>().enabled = false;
                break;
            case "ColorGroup":
                for (int i = 0; i < this.transform.parent.childCount; i++)
                {
                    Button btn = this.transform.parent.GetChild(i).GetComponent<Button>();
                    Image img = btn.GetComponent<Image>();
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 255 / 255f);
                    btn.enabled = true;
                }
                this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, 100 / 255f);
                if (ButtonID != 7)
                {
                    this.GetComponent<Button>().enabled = false;
                    if(this.transform.parent.GetChild(7).GetComponent<fcpButtonScript>().IsEnter == true)
                    {
                        this.transform.parent.GetChild(7).GetComponent<fcpButtonScript>().FcpOn();
                    }
                }
                break;
        }
    }

}
