using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FullArticleDisplay : MonoBehaviour
{
    [SerializeField]
    private ScrollRect _scrollRect;
    [SerializeField]
    private Image _fullArticleImage;
    [SerializeField]
    private Button _closeButton;

    private LayoutElement _layoutElement;
    private RectTransform _rectTransform;


    public void Initialize()
    {
        _scrollRect = GetComponentInChildren<ScrollRect>();
        _closeButton.onClick.AddListener(Hide);

        _layoutElement = _fullArticleImage.GetComponent<LayoutElement>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Show(Sprite article)
    {
        PlayShowSequence(article);
    }

    public void Hide()
    {
        PlayHideSequence();
    }

    private void SwitchArticle(Sprite article)
    {
        _fullArticleImage.sprite = article;

        float articleWidth = 770;
        float articleHeight = articleWidth * (article.rect.height / article.rect.width);
        _layoutElement.minWidth = articleWidth;
        _layoutElement.minHeight = articleHeight;

        _scrollRect.verticalNormalizedPosition = 1f;

    }

    private void PlayShowSequence(Sprite article)
    {
        Sequence showSequence = DOTween.Sequence();

        showSequence
            .OnStart(() =>
            {
                gameObject.SetActive(true);
            })
            .Append(_rectTransform.DOAnchorPosY(900, 0.5f).SetEase(Ease.InBack))
            .AppendCallback(() =>
            {
                SwitchArticle(article);
            })
            .Append(_rectTransform.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutBack));
    }

    private void PlayHideSequence()
    {
        Sequence hideSequence = DOTween.Sequence()
            .Append(_rectTransform.DOAnchorPosY(900, 0.5f).SetEase(Ease.InBack))
            //.Join(_fullArticleImage.DOFade(0, 0.5f))
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
}
