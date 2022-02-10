using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XReal.XTown.UI
{
    public class UIEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, IDragHandler
    {
        // define events to attach callbacks to.
        public Action<PointerEventData> OnEnterHandler = null;
        public Action<PointerEventData> OnClickHandler = null;
        public Action<PointerEventData> OnExitHandler = null;
        public Action<PointerEventData> OnDragHandler = null;


        //pointer events invocation.
        public void OnDrag(PointerEventData eventData)
        {
            if (OnDragHandler != null) OnDragHandler.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (OnEnterHandler != null) OnEnterHandler.Invoke(eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (OnClickHandler != null) OnClickHandler.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (OnExitHandler != null) OnExitHandler.Invoke(eventData);
        }
    }
}