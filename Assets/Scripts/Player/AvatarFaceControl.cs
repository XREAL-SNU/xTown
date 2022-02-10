using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AvatarFaceControl : MonoBehaviour
{
    public Material AvatarFace;

    [SerializeField] List<Texture> _emojiTextures;
    [SerializeField] Texture _defaultTexture;

    [SerializeField] List<int> _tempList;
    bool _isDefault = true;

    private void Start() 
    {
        ChangeFace(_defaultTexture);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(ShowFaceForSeconds(AvatarFaceManagement.s_favList[0].GetImageIndex()));
        if (Input.GetKeyDown(KeyCode.Alpha2)) StartCoroutine(ShowFaceForSeconds(AvatarFaceManagement.s_favList[1].GetImageIndex()));
        if (Input.GetKeyDown(KeyCode.Alpha3)) StartCoroutine(ShowFaceForSeconds(AvatarFaceManagement.s_favList[2].GetImageIndex()));
        if (Input.GetKeyDown(KeyCode.Alpha4)) StartCoroutine(ShowFaceForSeconds(AvatarFaceManagement.s_favList[3].GetImageIndex()));
    }

    public void ChangeFace(AvatarFaceButton faceButton) 
    {
        AvatarFace.SetTexture("_MainTex", _emojiTextures[faceButton.GetImageIndex()]);

        if (faceButton.GetButtonTextAsString().Equals("happy"))
            _isDefault = true;
        else
            _isDefault = false;
    }

    public void ChangeFace(Texture faceTexture)
    {
        AvatarFace.SetTexture("_MainTex", faceTexture);

        if (faceTexture.name.Equals("happy"))
            _isDefault = true;
        else
            _isDefault = false;
    }

    IEnumerator ShowFaceForSeconds(int index)
    {
        if (!_isDefault) yield break;

        ChangeFace(_emojiTextures[index]);

        yield return new WaitForSeconds(10f);

        ChangeFace(_defaultTexture);
    }
}