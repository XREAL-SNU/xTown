using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArticleDisplay : MonoBehaviour
{
    [SerializeField]
    private Article _article;

    [SerializeField]
    private Image _thumbnailImage;
    [SerializeField]
    private Text _titleText;
    [SerializeField]
    private Text _publisherText;

    private Button _articleButton;

    private void Start()
    {
        _thumbnailImage.sprite = _article.thumbnail;
        _titleText.text = _article.title;
        _publisherText.text = _article.publisher;

        _articleButton = GetComponent<Button>();
        _articleButton.onClick.AddListener(OnClick_Article);
    }

    private void OnClick_Article()
    {

    }
}
