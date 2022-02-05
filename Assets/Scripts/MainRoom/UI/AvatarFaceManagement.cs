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
    AvatarFaceButton _currentlySelected;

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

    public void Select(AvatarFaceButton clickedButton)
    {
        if(_currentlySelected != clickedButton)
        {
            if (_isSelected) _currentlySelected.DeselectButton();
            clickedButton.SelectButton();
            _currentlySelected = clickedButton;
            _isSelected = true;
        }
        else
        {
            _currentlySelected.DeselectButton();
            _currentlySelected = null;
            _isSelected = false;
        }
    }

    void ChangeFavorites()
    {

    }
}
