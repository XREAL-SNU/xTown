using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BoatIconCanvas : MonoBehaviour
{
    [SerializeField]
    private Image _iconImage;

    private BoatSeat _seat;
    public BoatSeat seat { set { _seat = value; } }

    void FixedUpdate()
    {
        _iconImage.rectTransform.position = Camera.main.WorldToScreenPoint(transform.parent.position + new Vector3(0, 1, 0));
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _iconImage.transform.DOScale(0, 0);
        _iconImage.transform.DOScale(1, 0.5f);
    }

    public void Hide()
    {
        Sequence hideSequence = DOTween.Sequence()
            .Append(_iconImage.transform.DOScale(0, 0.5f))
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
}
