
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContentScaler : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private RectTransform _contentTransform;

    private float _scalingSpeed = 0.1f;
    private float _minScale = 1f;
    private float _maxScale = 6f;

    private float _newScale = 0f;
    public void OnDrag(PointerEventData eventData)
    {

        if (eventData.delta.x > 0)
        {
            _newScale = _contentTransform.sizeDelta.x + _scalingSpeed;
        }
        else if (eventData.delta.x < 0)
        {
            _newScale = _contentTransform.sizeDelta.x - _scalingSpeed;
        }

        _newScale = Mathf.Clamp(_newScale, _minScale, _maxScale);
        _contentTransform.sizeDelta = new Vector2(_newScale, _newScale);
    }

}
