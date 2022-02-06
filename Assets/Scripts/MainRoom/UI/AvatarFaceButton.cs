using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarFaceButton : MonoBehaviour
{
    // Reference in Editor
    public Text  ButtonText;
    public Image ButtonImage;

    Image _buttonBorder;

    public bool Selected { get; set; }

    // Start is called before the first frame update
    void Start() 
    { 
        Selected = false;
        _buttonBorder = GetComponent<Image>();
    }

    public void SetButtonText(Text buttonText)    { ButtonText.text = buttonText.text; }
    public void SetButtonImage(Image buttonImage) { ButtonImage.sprite = buttonImage.sprite; }
    public Text  GetButtonText()  { return ButtonText; }
    public Image GetButtonImage() { return ButtonImage; }

    public void SelectButton()
    {
        Selected = true;
        _buttonBorder.color = Color.yellow;
    }

    public void DeselectButton()
    {
        Selected = false;
        _buttonBorder.color = Color.white;
    }
}
