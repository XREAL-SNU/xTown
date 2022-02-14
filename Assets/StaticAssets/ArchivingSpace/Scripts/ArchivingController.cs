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

    private Sequence _openSequence;
    private Sequence _closeSequence;

    private void Start()
    {
        _archivingRT = _archivingCanvas.GetComponent<RectTransform>();

        _openSequence = DOTween.Sequence().
            OnStart(() =>
            {
                Debug.Log("open");
                _archivingCamera.SetActive(true);
                _archivingCanvas.SetActive(true);
                _archivingRT.sizeDelta = new Vector2(0, _archivingRT.sizeDelta.y);
            })
            .Append(DOVirtual.Float(0, _canvasWidth, 1f, RectTransformSize)).SetEase(Ease.OutCubic)
            .SetAutoKill(false);

        _closeSequence = DOTween.Sequence().
            OnStart(() =>
            {
                Debug.Log("close");
                _archivingCamera.SetActive(false);
                _archivingRT.sizeDelta = new Vector2(0, _archivingRT.sizeDelta.y);
            })
            .Append(DOVirtual.Float(_canvasWidth, 0, 1f, RectTransformSize)).SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                _archivingCanvas.SetActive(false);
            })
            .SetAutoKill(false);

        _openSequence.Pause();
        _closeSequence.Pause();
    }
    public void OnTriggerChange()
    {
        if (_isTriggered)
        {
            _openSequence.Restart();
        }
        else
        {
            _closeSequence.Restart();
        }
    }

    private void RectTransformSize(float x)
    {
        _archivingRT.sizeDelta = new Vector2(x, _archivingRT.sizeDelta.y);
    }

    private void PlaySequence()
    {

    }

}
