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
    GameObject _currentlySelectedObject;
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
            ChangeCurrentlySelected(clicked);
        }
        else
        {
            DeselectCurrentlySelected();
        }
    }

    void ChangeCurrentlySelected(GameObject selected)
    {
        _currentlySelectedObject = selected;
        _currentlySelectedButton = _currentlySelectedObject.GetComponent<AvatarFaceButton>();
        _isSelected = true;
    }

    void DeselectCurrentlySelected()
    {
        _currentlySelectedButton.DeselectButton();
        _currentlySelectedObject = null;
        _currentlySelectedButton = null;
        _isSelected = false;
    }


    // Not Intuitive Approach (personal opinion)
    //
    // Setting AvatarFaceButton Script for each Favorites Button and
    // Passing AvatarFaceButton Component as parameter in Button Invoke Function
    // and merging ChangeFavoritesText() and ChangeFavoritesImage() seems better.
    public void ChangeFavoritesText(Text favText)
    {
        if(_isSelected) favText.text = _currentlySelectedButton.ButtonText.text;
    }
    public void ChangeFavoritesImage(Image favImage)
    {
        if (_isSelected)
        {
            favImage.sprite = _currentlySelectedButton.ButtonImage.sprite;
            DeselectCurrentlySelected();
        }
    }
}
