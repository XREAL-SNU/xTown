using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FaceEmoji : MonoBehaviour, IPointerClickHandler
{
    public Texture img_Emoji;
    public Material AvatarFace;

    public void OnPointerClick(PointerEventData eventData)
    {
        AvatarFace.SetTexture("_MainTex", img_Emoji);
    }
}
