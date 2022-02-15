using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AvatarFaceControl : MonoBehaviour
{
    public Material AvatarFace;

    [SerializeField] Texture _defaultTexture;
    
    /*bool _isDefault = true;*/
    bool _isChanged = false;
    int _currentTextureIndex;
    float _counter = 0;

    private void Start() 
    {
        AvatarFace.SetTexture("_MainTex", _defaultTexture);
        for(int i = 0; i < AvatarFaceManagement.s_avatarTextureList.Count; i++)
        {
            if (AvatarFaceManagement.s_avatarTextureList[i].name.Equals("happy"))
            {
                _currentTextureIndex = i;
                break;
            }
        }
    }

    private void Update()
    {
        CountSeconds();

        if (PlayerKeyboard.KeyboardInputSet(KeyboardInput.Emotion1)) ShowFace(AvatarFaceManagement.s_favList[0].GetImageIndex());
        if (PlayerKeyboard.KeyboardInputSet(KeyboardInput.Emotion2)) ShowFace(AvatarFaceManagement.s_favList[1].GetImageIndex());
        if (PlayerKeyboard.KeyboardInputSet(KeyboardInput.Emotion3)) ShowFace(AvatarFaceManagement.s_favList[2].GetImageIndex());
        if (PlayerKeyboard.KeyboardInputSet(KeyboardInput.Emotion4)) ShowFace(AvatarFaceManagement.s_favList[3].GetImageIndex());
    }

    public void ChangeFace(int faceIndex) 
    {
        AvatarFace.SetTexture("_MainTex", AvatarFaceManagement.s_avatarTextureList[faceIndex]);
/*
        if (AvatarFaceManagement.s_avatarTextureList[faceIndex].name.Equals("happy"))
            _isDefault = true;
        else
            _isDefault = false;*/
    }

    void ChangeFace(Texture texture)
    {
        AvatarFace.SetTexture("_MainTex", texture);
/*
        if (texture.name.Equals("happy"))
            _isDefault = true;
        else
            _isDefault = false;*/
    }

    void ShowFace(int index)
    {
        /*if (!_isDefault) yield break;*/

        ChangeFace(index);
        _currentTextureIndex = index;

        _isChanged = true;
        _counter = 0;
    }

    void CountSeconds()
    {
        if(_isChanged)
        {
            _counter += Time.deltaTime;
        }
        if(_counter > 10f)
        {
            _isChanged = false;
            _counter = 0;
            ChangeFace(_defaultTexture);
        }
    }
}