using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArchivingController : MonoBehaviour
{
    [SerializeField]
    private GameObject _archivingCamera;
    [SerializeField]
    private GameObject _archivingCanvas;

    private bool _isTriggered;
    public bool isTriggered { get { return _isTriggered; } set { _isTriggered = value; } }

    private float _canvasWidth = 1330;

    private RectTransform _archivingRT;

    private void Start()
    {
        _archivingRT = _archivingCanvas.GetComponent<RectTransform>();
    }
    public void OnTriggerChange()
    {
        if (_isTriggered)
        {
            PlayOpenSequence();
        }
        else
        {
            PlayCloseSequence();
        }
    }

    private void PlayOpenSequence()
    {
        Sequence _openSequence = DOTween.Sequence()
            .PrependInterval(1f)
            .OnStart(() =>
            {
                _archivingCamera.SetActive(true);

            })
            .InsertCallback(1, () =>
            {
                _archivingCanvas.SetActive(true);
                _archivingRT.sizeDelta = new Vector2(0, _archivingRT.sizeDelta.y);
            })
            .Append(DOVirtual.Float(0, _canvasWidth, 1f, RectTransformSize)).SetEase(Ease.OutCubic);
    }

    private void PlayCloseSequence()
    {
        Sequence _closeSequence = DOTween.Sequence()
            .OnStart(() =>
            {
                _archivingCamera.SetActive(false);
                _archivingRT.sizeDelta = new Vector2(0, _archivingRT.sizeDelta.y);
            })
            .Append(DOVirtual.Float(_canvasWidth, 0, 1f, RectTransformSize)).SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                if (!_isTriggered)
                {
                    _archivingCanvas.SetActive(false);
                }
            });
    }

    private void RectTransformSize(float x)
    {
        _archivingRT.sizeDelta = new Vector2(x, _archivingRT.sizeDelta.y);
    }
}
