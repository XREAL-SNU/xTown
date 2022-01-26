
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContentScaler : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private RectTransform _contentTransform;

    private float _scalingSpeed = 0.05f;
    private float _minScale = 1f;
    private float _maxScale = 10f;

    private float _newScaleX = 0f;
    private float _newScaleY = 0f;
    public void OnDrag(PointerEventData eventData)
    {

        if (eventData.delta.x > 0)
        {
            _newScaleX = _contentTransform.sizeDelta.x + _scalingSpeed;
        }
        else if (eventData.delta.x < 0)
        {
            _newScaleX = _contentTransform.sizeDelta.x - _scalingSpeed;
        }

        if (eventData.delta.y > 0)
        {
            _newScaleY = _contentTransform.sizeDelta.y + _scalingSpeed;
        }
        else if (eventData.delta.y < 0)
        {
            _newScaleY = _contentTransform.sizeDelta.y - _scalingSpeed;
        }

        _newScaleX = Mathf.Clamp(_newScaleX, _minScale, _maxScale);
        _newScaleY = Mathf.Clamp(_newScaleY, _minScale, _maxScale);
        _contentTransform.sizeDelta = new Vector2(_newScaleX, _newScaleY);
    }

}
