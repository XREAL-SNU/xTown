using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarFaceButton : MonoBehaviour
{
    [HideInInspector] public Text  ButtonText;
    [HideInInspector] public Image ButtonImage;
    Image _buttonBorder;

    public bool Selected { get; set; }

    // Start is called before the first frame update
    void Start() 
    { 
        Selected = false;
        _buttonBorder = GetComponent<Image>();
        ButtonText = gameObject.transform.GetChild(0).GetComponent<Text>();
        ButtonImage = gameObject.transform.GetChild(1).GetComponent<Image>();
    }

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
