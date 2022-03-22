using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickSlotManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] List<Sprite> _buttonIcons;
    [SerializeField] List<QuickSlotButton> _quickSlots;

    public Transform GridLayout;

    public static QuickSlotButton CurrentlySelected;
    static List<QuickSlotButton> s_quickSlots;

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

        s_quickSlots = new List<QuickSlotButton>();
        for (int i = 0; i < _quickSlots.Count; i++)
        {
            s_quickSlots.Add(_quickSlots[i]);
        }
    }

    public static void AddToQuickSlot(QuickSlotButton quickslotButton)
    {
        int index = CheckDistinct();

        if(index == -1)
        {
            quickslotButton.ButtonImage.sprite = CurrentlySelected.ButtonImage.sprite;
            quickslotButton.ButtonText.text = CurrentlySelected.ButtonText.text;
            quickslotButton.fid = CurrentlySelected.fid;
        }
        else
        {
            SwapButtons(quickslotButton, index);
        }
        
        CurrentlySelected = null;
    }

    static int CheckDistinct()
    {
        int retVal = -1;

        for(int i = 0; i < s_quickSlots.Count; i++)
        {
            if (s_quickSlots[i].fid == CurrentlySelected.fid)
                retVal = i;
        }

        return retVal;
    }

    static void SwapButtons(QuickSlotButton quickslotButton, int index)
    {
        Sprite spriteBuf = quickslotButton.ButtonImage.sprite;
        string textBuf = quickslotButton.ButtonText.text;
        int idBuf = quickslotButton.fid;

        quickslotButton.ButtonImage.sprite = s_quickSlots[index].ButtonImage.sprite;
        quickslotButton.ButtonText.text = s_quickSlots[index].ButtonText.text;
        quickslotButton.fid = s_quickSlots[index].fid;

        s_quickSlots[index].ButtonImage.sprite = spriteBuf;
        s_quickSlots[index].ButtonText.text = textBuf;
        s_quickSlots[index].fid = idBuf;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CurrentlySelected = null;
    }
}
