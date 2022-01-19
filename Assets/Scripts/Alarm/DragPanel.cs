using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField]
    private RectTransform _dragRectTransform;
    [SerializeField]
    private Canvas _canvas;

    private void Awake()
    {
        if (_dragRectTransform == null)
        {
            _dragRectTransform = transform.parent.GetComponent<RectTransform>();
        }

        if (_canvas == null)
        {
            Transform _testCanvasTransform = transform.parent;
            while (_testCanvasTransform != null)
            {
                _canvas = _testCanvasTransform.GetComponent<Canvas>();
                if (_canvas != null)
                {
                    break;
                }
                _testCanvasTransform = _testCanvasTransform.parent;
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        _dragRectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _dragRectTransform.SetAsLastSibling();
    }
}
