using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : MonoBehaviour
{
    [SerializeField] List<Sprite> _buttonIcons;

    public Transform GridLayout;

    // Start is called before the first frame update
    void Start()
    {
        QuickSlotButton buttonProp;

        for(int i = 0; i < GridLayout.childCount; i++)
        {
            buttonProp = GridLayout.GetChild(i).GetComponent<QuickSlotButton>();
            buttonProp.ButtonImage.sprite = _buttonIcons[i];
            buttonProp.ButtonText.text = _buttonIcons[i].name;
        }
    }
}
