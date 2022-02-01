using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EmoticonMenuScript : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData){
        gameObject.GetComponent<Image>().color = Color.gray;
    }
    public void OnPointerDown(PointerEventData eventData){
        gameObject.GetComponent<Image>().color = Color.black;
    }
    public void OnPointerUp(PointerEventData eventData){
        gameObject.GetComponent<Image>().color = Color.white;
        Emotion._currentMenu = int.Parse(gameObject.name.Substring(17,1));
        // Emotion.EmoticonSelect();
    }
    public void OnPointerExit(PointerEventData eventData){
        gameObject.GetComponent<Image>().color = Color.white;
    }
}
