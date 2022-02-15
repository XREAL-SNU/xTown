using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EmoticonMenuScript : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private GameObject EmotionCanvas;
    private void Start() {
        EmotionCanvas = GameObject.Find("EmoticonMenuCanvas");
    }
    public void OnPointerEnter(PointerEventData eventData){
        gameObject.GetComponent<Image>().color = Color.gray;
    }
    public void OnPointerDown(PointerEventData eventData){
        gameObject.GetComponent<Image>().color = Color.black;
    }
    public void OnPointerUp(PointerEventData eventData){
        gameObject.GetComponent<Image>().color = Color.white;
        EmotionCanvas.GetComponent<Emotion>().EmoticonSelect(int.Parse(gameObject.name.Substring(17,1))-1);
    }
    public void OnPointerExit(PointerEventData eventData){
        gameObject.GetComponent<Image>().color = Color.white;
    }
}
