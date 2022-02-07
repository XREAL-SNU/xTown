using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarFaceButton : MonoBehaviour
{
    // Reference in Editor
    [SerializeField] Text  ButtonText;
    [SerializeField] Image ButtonImage;

    Image _buttonBorder;
    public int _imageIndex;

    public bool Selected { get; set; }

    // Start is called before the first frame update
    void Start() 
    { 
        Selected = false;
        _buttonBorder = GetComponent<Image>();
    }

    public Text GetButtonText() { return ButtonText; }
    public string GetButtonTextAsString() { return ButtonText.text; }
    public Image GetButtonImage() { return ButtonImage; }
    public Sprite GetButtonImageAsSprite() { return ButtonImage.sprite; }
    public void SetButtonText(Text buttonText)    { ButtonText.text = buttonText.text; }
    public void SetButtonText(string buttonText)  { ButtonText.text = buttonText; }                 // Overloaded Method (SetButtonText)
    public void SetButtonImage(Image buttonImage) { ButtonImage.sprite = buttonImage.sprite; }
    public void SetButtonImage(Sprite buttonImage) { ButtonImage.sprite = buttonImage; }            // Overloaded Method (SetButtonImage)
    public int GetImageIndex() { return _imageIndex; }
    public void SetImageIndex(int index) { _imageIndex = index; }

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
