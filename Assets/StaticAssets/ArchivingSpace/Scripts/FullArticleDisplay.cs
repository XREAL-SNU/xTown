using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FullArticleDisplay : MonoBehaviour
{
    [SerializeField]
    private Image _fullArticleImage;
    [SerializeField]
    private Button _closeButton;

    private LayoutElement _layoutElement;
    private RectTransform _rectTransform;


    // Start is called before the first frame update
    void Start()
    {
        _closeButton.onClick.AddListener(Hide);

        _layoutElement = _fullArticleImage.GetComponent<LayoutElement>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Show(Sprite article)
    {
        SwitchArticle(article);

        PlayShowSequence();
    }

    public void Hide()
    {
        PlayHideSequence();
    }

    private void SwitchArticle(Sprite article)
    {
        _fullArticleImage.sprite = article;

        float articleWidth = 640;
        float articleHeight = articleWidth * (article.rect.y / article.rect.x);
        _layoutElement.minWidth = articleWidth;
        _layoutElement.minHeight = articleHeight;
    }

    private void PlayShowSequence()
    {
        Sequence _openSequence = DOTween.Sequence()
            .OnStart(() =>
            {
                gameObject.SetActive(true);
                _rectTransform.DOAnchorPosY(-800, 0);
            })
            .Append(_rectTransform.DOAnchorPosY(0, 1).SetEase(Ease.OutBack));
    }

    private void PlayHideSequence()
    {
        Sequence _openSequence = DOTween.Sequence()
            .Append(_rectTransform.DOAnchorPosY(800, 1).SetEase(Ease.InBack))
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
}
