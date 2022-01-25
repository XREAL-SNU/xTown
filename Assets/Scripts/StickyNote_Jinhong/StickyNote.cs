using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class StickyNote : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField]
    private RectTransform _controllerTarget;
    [SerializeField]
    private Image _controllerImage;

    [Header("Content")]
    [SerializeField]
    private RectTransform _contentTransform;
    [SerializeField]
    private BoxCollider2D _contentCollider;
    [SerializeField]
    private Image _contentImage;
    [SerializeField]
    private TMP_Text _contentText;

    [Header("Edit")]
    [SerializeField]
    private GameObject _editCanvas;
    [SerializeField]
    private TMP_InputField _editInputField;

    [Header("Colors")]
    [SerializeField]
    private Color[] _backgroundColors;
    [SerializeField]
    private Image _colorChangerIcon;

    private bool _hoveringOnController;
    private bool _hoveringOnContent;
    private int _colorIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _contentCollider.size = new Vector2(_contentTransform.rect.width, _contentTransform.rect.height);
        _controllerImage.transform.DOScale(0, 0);
        _hoveringOnController = false;
        _hoveringOnContent = false;

        _colorChangerIcon.color = _backgroundColors[GetNextColorIndex(_colorIndex)];
        _colorChangerIcon.DOFade(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        _contentCollider.size = new Vector2(_contentTransform.rect.width, _contentTransform.rect.height);
        _controllerImage.transform.position = Camera.main.WorldToScreenPoint(_controllerTarget.position);
    }

    public void OnValueChanged(string value)
    {
        _contentText.text = value;
    }


    #region PointerEvents
    public void OnPointerEnterController()
    {
        _hoveringOnController = true;
    }

    public void OnPointerExitController()
    {
        _hoveringOnController = false;
        StartCoroutine(HideController());
    }

    public void OnPointerEnterContent()
    {
        _hoveringOnContent = true;
        _controllerImage.transform.DOScale(1, 0.4f);
        _colorChangerIcon.DOFade(1, 0.4f);
    }

    public void OnPointerExitContent()
    {
        _hoveringOnContent = false;
        StartCoroutine(HideController());
    }

    IEnumerator HideController()
    {
        yield return new WaitForSeconds(0.5f);
        if (_hoveringOnController || _hoveringOnContent)
        {
            yield return null;
        }
        else
        {
            _controllerImage.transform.DOScale(0, 0.4f);
            _colorChangerIcon.DOFade(0, 0.4f);
        }
    }
    #endregion

    #region ControllerCallbacks
    public void OnClick_Edit()
    {
        _editCanvas.SetActive(true);
        _editInputField.text = _contentText.text;
    }

    public void OnClick_Remove()
    {
        Destroy(gameObject);
    }
    #endregion

    #region ButtonCallbacks
    public void OnClick_Confirm()
    {
        _editCanvas.SetActive(false);
    }

    public void OnClick_Color()
    {
        _colorIndex = GetNextColorIndex(_colorIndex);

        _contentImage.color = _backgroundColors[_colorIndex];
        _colorChangerIcon.color = _backgroundColors[GetNextColorIndex(_colorIndex)];
    }

    private int GetNextColorIndex(int i)
    {
        if (i+1 > _backgroundColors.Length - 1)
        {
            return 0;
        }
        else
        {
            return i + 1;
        }
    }
    #endregion
}
