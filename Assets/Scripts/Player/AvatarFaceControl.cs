using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AvatarFaceControl : MonoBehaviour
{
    public Material AvatarFace;

    [SerializeField] Texture _defaultTexture;

    /*bool _isDefault = true;
    bool _isChanged = false;
    float _counter = 0;*/

    int _currentTextureIndex;
    bool _crRunning;
    IEnumerator _coroutine;

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
        if (PlayerKeyboard.KeyboardInput("Emotion",KeyboardInput.Emotion1)) ShowFace(AvatarFaceManagement.s_favList[0].GetImageIndex());
        if (PlayerKeyboard.KeyboardInput("Emotion", KeyboardInput.Emotion2)) ShowFace(AvatarFaceManagement.s_favList[1].GetImageIndex());
        if (PlayerKeyboard.KeyboardInput("Emotion", KeyboardInput.Emotion3)) ShowFace(AvatarFaceManagement.s_favList[2].GetImageIndex());
        if (PlayerKeyboard.KeyboardInput("Emotion", KeyboardInput.Emotion4)) ShowFace(AvatarFaceManagement.s_favList[3].GetImageIndex());
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

    /*void ChangeFace(Texture texture)
    {
        AvatarFace.SetTexture("_MainTex", texture);

        if (texture.name.Equals("happy"))
            _isDefault = true;
        else
            _isDefault = false;
    }*/

    void ShowFace(int index)
    {
        /*if (!_isDefault) yield break;*/

        if (_crRunning)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = ShowFaceCoroutine(index);
        StartCoroutine(_coroutine);
        _currentTextureIndex = index;
    }

    IEnumerator ShowFaceCoroutine(int index)
    {
        _crRunning = true;
        Debug.Log("_crRunning = true");

        ChangeFace(index);
        
        yield return new WaitForSeconds(10f);
        
        ChangeFace(11);

        _crRunning = false;
        Debug.Log("_crRunning = false");
    }
}