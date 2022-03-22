using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : MonoBehaviour
{
    [SerializeField] List<Sprite> _buttonIcons;
    [SerializeField] List<QuickSlotButton> _quickSlots;

    public Transform GridLayout;

    public static QuickSlotButton CurrentlySelected;

    void Start()
    {
        QuickSlotButton buttonProp;

        for(int i = 0; i < GridLayout.childCount; i++)
        {
            buttonProp = GridLayout.GetChild(i).GetComponent<QuickSlotButton>();
            buttonProp.ButtonImage.sprite = _buttonIcons[i];
            buttonProp.ButtonText.text = _buttonIcons[i].name;
            buttonProp.fid = i;
        }
    }

    public static void AddToQuickSlot(QuickSlotButton quickslotButton)
    {
        quickslotButton.ButtonImage.sprite = CurrentlySelected.ButtonImage.sprite;
        quickslotButton.ButtonText.text = CurrentlySelected.ButtonText.text;
        quickslotButton.fid = CurrentlySelected.fid;

        CurrentlySelected = null;
    }

    bool CheckDistinct()
    {
        return false;
    }
}
