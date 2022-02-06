using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarFaceManagement : MonoBehaviour
{
    // This needs to be setup in Editor
    [SerializeField] List<Sprite> _avatarFaceList;

    [SerializeField] Transform EmojiFavorites;
    [SerializeField] Transform EmojiList;

    bool _isSelected = false;
    AvatarFaceButton _currentlySelectedButton;

    // Start is called before the first frame update
    void Start()
    {

        /*if()
        for (int i = 0; i < 4; i++)
        {
            EmojiFavorites.GetChild(i).gameObject.GetComponent<Image>().sprite = userSprite[index];
        }*/

        if (EmojiList.childCount != _avatarFaceList.Count) Debug.Log("Number of Child does not match number of Images");

        Transform childImage;
        for(int i = 0; i < EmojiList.childCount; i++)
        {
            childImage = EmojiList.GetChild(i);
            childImage.GetChild(1).GetComponent<Image>().sprite = _avatarFaceList[i];
            childImage.GetComponentInChildren<Text>().text      = _avatarFaceList[i].name;
        }
    }

    public void SelectEmoji(GameObject clicked)
    {
        AvatarFaceButton clickedButton = clicked.GetComponent<AvatarFaceButton>();

        if(_currentlySelectedButton != clickedButton)
        {
            if (_isSelected) _currentlySelectedButton.DeselectButton();
            clickedButton.SelectButton();
            ChangeCurrentlySelected(clickedButton);
        }
        else
        {
            DeselectCurrentlySelected();
        }
    }

    void ChangeCurrentlySelected(AvatarFaceButton selected)
    {
        _currentlySelectedButton = selected;
        _isSelected = true;
    }

    void DeselectCurrentlySelected()
    {
        _currentlySelectedButton.DeselectButton();
        _currentlySelectedButton = null;
        _isSelected = false;
    }

    // Invoke this Function when a FaceButton is Currently Selected & Favorites Face Button is Clicked
    public void AddToFavorites(AvatarFaceButton avatarFaceButton)
    {
        if(_isSelected)
        {
            avatarFaceButton.SetButtonText(_currentlySelectedButton.ButtonText);
            avatarFaceButton.SetButtonImage(_currentlySelectedButton.ButtonImage);
            DeselectCurrentlySelected();
        }
    }
}
