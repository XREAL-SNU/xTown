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

    // Start is called before the first frame update
    void Start()
    {

        /*if()
        for (int i = 0; i < 4; i++)
        {
            EmojiFavorites.GetChild(i).gameObject.GetComponent<Image>().sprite = userSprite[index];
        }*/

        Transform childImage;
        for(int i = 0; i < EmojiList.childCount; i++)
        {
            childImage = EmojiList.GetChild(i);
            childImage.GetComponent<Image>().sprite = _avatarFaceList[i];
            childImage.GetChild(0).GetComponent<Text>().text = _avatarFaceList[i].name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChooseFace()
    {

    }

    void ChangeSlot()
    {

    }
}
