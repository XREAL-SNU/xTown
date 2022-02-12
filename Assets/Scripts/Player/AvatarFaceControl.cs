using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AvatarFaceControl : MonoBehaviour
{
    public Material AvatarFace;

    [SerializeField] Texture _defaultTexture;
    
    bool _isDefault = true;

    private void Start() { AvatarFace.SetTexture("_MainTex", _defaultTexture); }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(ShowFaceForSeconds(AvatarFaceManagement.s_favList[0].GetImageIndex()));
        if (Input.GetKeyDown(KeyCode.Alpha2)) StartCoroutine(ShowFaceForSeconds(AvatarFaceManagement.s_favList[1].GetImageIndex()));
        if (Input.GetKeyDown(KeyCode.Alpha3)) StartCoroutine(ShowFaceForSeconds(AvatarFaceManagement.s_favList[2].GetImageIndex()));
        if (Input.GetKeyDown(KeyCode.Alpha4)) StartCoroutine(ShowFaceForSeconds(AvatarFaceManagement.s_favList[3].GetImageIndex()));
    }

    public void ChangeFace(int faceIndex) 
    {
        AvatarFace.SetTexture("_MainTex", AvatarFaceManagement.s_avatarTextureList[faceIndex]);

        if (AvatarFaceManagement.s_avatarTextureList[faceIndex].name.Equals("happy"))
            _isDefault = true;
        else
            _isDefault = false;
    }

    IEnumerator ShowFaceForSeconds(int index)
    {
        if (!_isDefault) yield break;

        ChangeFace(index);

        yield return new WaitForSeconds(10f);

        ChangeFace(AvatarFaceManagement.DefaultIndex);
    }
}