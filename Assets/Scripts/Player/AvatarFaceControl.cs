using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AvatarFaceControl : MonoBehaviour
{
    public Material AvatarFace;

    [SerializeField] List<Texture> _emojiTextures;

    private void Start() { AvatarFace.SetTexture("_MainTex", _emojiTextures[11]); }

    public void ChangeFace(AvatarFaceButton faceButton) { AvatarFace.SetTexture("_MainTex", _emojiTextures[faceButton.GetImageIndex()]); }
}