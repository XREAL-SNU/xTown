using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarFaceManagement : MonoBehaviour
{
    // This needs to be setup in Editor
    // To get favorites data, access it by first referencing this script and:
    //      {Reference of this Script}.GetFavButton({index of button (0 ~ 4)}).GetButtonImageAsSprite();
    //      {Reference of this Script}.GetFavButton({index of button (0 ~ 4)}).GetButtonTextAsString();
    //      {Reference of this Script}.GetFavButton({index of button (0 ~ 4)}).GetImageIndex();
    [SerializeField] List<Sprite> _avatarFaceList;
    [SerializeField] List<AvatarFaceButton> _favList;

    [SerializeField] Transform EmojiFavorites;
    [SerializeField] Transform EmojiList;

    bool _isSelected = false;
    AvatarFaceButton _currentlySelectedButton;

    public AvatarFaceButton GetFavButton(int index) { return _favList[index]; }

    public static List<AvatarFaceButton> s_favList;

    // Start is called before the first frame update
    void Start()
    {
        // Add any previously saved user data
        /*if()
        for (int i = 0; i < 4; i++)
        {
            EmojiFavorites.GetChild(i).gameObject.GetComponent<Image>().sprite = userSprite[index];
        }*/

        if (EmojiList.childCount != _avatarFaceList.Count) Debug.Log("Number of Child does not match number of Images");

        // Load Images and Names
        AvatarFaceButton childButton;
        for(int i = 0; i < EmojiList.childCount; i++)
        {
            childButton = EmojiList.GetChild(i).GetComponent<AvatarFaceButton>();
            childButton.SetButtonImage(_avatarFaceList[i]);
            childButton.SetButtonText(_avatarFaceList[i].name);
            childButton.SetImageIndex(i);
        }

        s_favList = _favList;
    }

    // Invoke this Function when Button in Viewport is Clicked
    public void SelectEmoji(GameObject clicked)
    {
        AvatarFaceButton clickedButton = clicked.GetComponent<AvatarFaceButton>();

        if(_currentlySelectedButton != clickedButton)
        {
            if (_isSelected) _currentlySelectedButton.DeselectButton();
            clickedButton.SelectButton();
            ChangeCurrentlySelected(clickedButton);
        }
        else DeselectCurrentlySelected();
    }

    // Update Buffer of _currentlySelectedButton
    void ChangeCurrentlySelected(AvatarFaceButton selected)
    {
        _currentlySelectedButton = selected;
        _isSelected = true;
    }

    // Clear Buffer of _currentlySelectedButton
    void DeselectCurrentlySelected()
    {
        _currentlySelectedButton.DeselectButton();
        _currentlySelectedButton = null;
        _isSelected = false;
    }

    // Invoke this Function when a FaceButton is Currently Selected & Favorites Face Button is Clicked
    public void AddToFavorites(AvatarFaceButton avatarFaceButton)
    {
        int index = IsAdded(avatarFaceButton);

        // If a button is selected and is NOT added to favorites
        if(_isSelected && index == -1)
        {
            avatarFaceButton.SetButtonText(_currentlySelectedButton.GetButtonText());
            avatarFaceButton.SetButtonImage(_currentlySelectedButton.GetButtonImage());

            // Replace Index
            for(int i = 0; i < _avatarFaceList.Count; i++) { if (_currentlySelectedButton.GetButtonText().text.Equals(_avatarFaceList[i].name)) avatarFaceButton.SetImageIndex(i); }
            
            DeselectCurrentlySelected();
        }
        // If a button is selected and is added to favorites
        else if (_isSelected && index != -1)
        {
            SwapButtons(avatarFaceButton, _favList[index]);
            DeselectCurrentlySelected();
        }
    }

    // Checks whether input parameter is added to Favorites List.
    // Returns -1 if not added, index number if added.
    int IsAdded(AvatarFaceButton avatarFaceButton)
    {
        int retVal = -1;
        for(int i = 0; i < 4; i++)
        {
            if (_currentlySelectedButton.GetButtonText().text.Equals(_favList[i].GetButtonText().text)) retVal = i;
        }
        return retVal;
    }

    // Swap Location of Buttons. Should be called ONLY in AddToFavorites().
    void SwapButtons(AvatarFaceButton button1, AvatarFaceButton button2)
    {
        button2.SetButtonText(button1.GetButtonText());
        button2.SetButtonImage(button1.GetButtonImage());

        button1.SetButtonText(_currentlySelectedButton.GetButtonText());
        button1.SetButtonImage(_currentlySelectedButton.GetButtonImage());
    }
}
