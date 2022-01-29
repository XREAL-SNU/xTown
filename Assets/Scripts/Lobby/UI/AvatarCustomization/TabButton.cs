using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup TabGroup;
    public Image background;

    public void OnPointerClick(PointerEventData eventData)
    {
        TabGroup.OnTabSelect(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TabGroup.OnTabExit(this);
    }

    private void Start()
    {
        background = GetComponent<Image>();
        TabGroup.Subscribe(this);
    }
}
