using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickSlotManager_Sol : MonoBehaviour, IPointerClickHandler
{
    // Reference(s) in Scene
    [SerializeField] List<QuickSlotButton_Sol> _quickSlots;
    [SerializeField] List<Sprite> _buttonIcons;
    public Transform GridLayout;

    // Reference(s) in Directory    
    [SerializeField] GameObject _viewportButtonPrefab;

    public static QuickSlotButton_Sol CurrentlySelected;
    static List<QuickSlotButton_Sol> s_quickSlots;

    void Start()
    {
        GameObject buttonInstance;
        QuickSlotButton_Sol buttonProp;

        for(int i = 0; i < _buttonIcons.Count; i++)
        {
            // Instantiate at Start of Game
            buttonInstance = Instantiate(_viewportButtonPrefab, GridLayout);
            buttonProp = buttonInstance.GetComponent<QuickSlotButton_Sol>();

            // Set QuickSlotButton Properties
            buttonProp.ButtonImage.sprite = _buttonIcons[i];
            buttonProp.ButtonText.text = _buttonIcons[i].name;
            buttonProp.fid = i;
        }

        // Allocate Memory for s_quickSlots
        s_quickSlots = new List<QuickSlotButton_Sol>();

        // Add References of Quick Slot Buttons to Static List
        for (int i = 0; i < _quickSlots.Count; i++)
        {
            s_quickSlots.Add(_quickSlots[i]);
        }
    }

    // Function to Add Button to Quick Slots
    public static void AddToQuickSlot(QuickSlotButton_Sol quickslotButton)
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

    // Check if Button is already added
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

    // Change Location of Buttons. Should only be called if button type is QuickSlot
    static void SwapButtons(QuickSlotButton_Sol quickslotButton, int index)
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

    // Clear Buffer if Canvas is Clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        CurrentlySelected = null;
    }
}
