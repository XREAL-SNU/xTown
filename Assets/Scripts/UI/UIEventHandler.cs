using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XReal.XTown.UI
{
    public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
    {
        // define events to attach callbacks to.
        public Action<PointerEventData> OnClickHandler = null;
        public Action<PointerEventData> OnDragHandler = null;


        //pointer events invocation.
        public void OnDrag(PointerEventData eventData)
        {
            if (OnDragHandler != null) OnDragHandler.Invoke(eventData);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (OnClickHandler != null) OnClickHandler.Invoke(eventData);
        }


    }
}