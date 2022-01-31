using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class ContentCanvas : MonoBehaviour
{
    private bool _hovering;
    public bool hovering { get { return _hovering; } }
    [SerializeField]
    private RectTransform _controllerTarget;
    public RectTransform ControllerTarget { get { return _controllerTarget; } }
    [SerializeField]
    private Image _contentImage;
    [SerializeField]
    private TMP_Text _contentText;
    public TMP_Text ContentText { get { return _contentText; } }
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private BoxCollider2D _collider;
    [SerializeField]
    private Color[] _backgroundColors;
    [SerializeField]
    private Button _colorChangerButton;
    [SerializeField]
    private Image _colorChangerIcon;

    private StickyNote _stickyNote;

    // 스케일 조절 관련 변수들
    private float _scalingSpeed = 0.05f;
    private float _minScale = 1f;
    private float _maxScale = 10f;
    private float _newScaleX = 0f;
    private float _newScaleY = 0f;

    // 각도 조절 관련 변수들
    private float _rotationSensitivity = 1; // 이 값이 클수록 회전에 필요한 마우스 움직임이 더 커짐. 
    private int _rotationAngle = 10; // 한 번에 회전하는 각도
    // private int _minRotation = -60;
    // private int _maxRotation = 60;

    // 폰트 크기 조절 관련 변수들
    private float _fontSizingSpeed = 0.02f;
    private float _minFontSize = 0.15f;
    private float _maxFontSize = 0.6f;
    private float _newFontSize = 0f;

    // 스티키노트 색상 관련 변수
    private int _colorIndex = 0;

    // 스티키노트 더블클릭 인식 관련 변수
    private float _clickedTime;
    private float _doubleClickTime = 0.25f;

    public void Initialize(StickyNote stickyNote)
    {
        _stickyNote = stickyNote;
    }

    private void Start()
    {
        // 스티키노트 색깔 변화 버튼에 색깔 변화 함수 바인딩
        _colorChangerButton.onClick.AddListener(OnClick_Color);

        // 이벤트 트리거에 마우스 포인터 Enter/Exit 함수 바인딩
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        EventTrigger.Entry scrollEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        scrollEntry.eventID = EventTriggerType.Scroll;
        pointerEnterEntry.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
        pointerExitEntry.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
        scrollEntry.callback.AddListener((data) => { OnScrollContent((PointerEventData)data); });
        trigger.triggers.Add(pointerEnterEntry);
        trigger.triggers.Add(pointerExitEntry);
        trigger.triggers.Add(scrollEntry);


        // 이벤트 트리거에 마우스 클릭 함수 바인딩 (더블클릭 인식 위해서)
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(pointerDownEntry);
        
        _colorChangerIcon.color = _backgroundColors[GetNextColorIndex(_colorIndex)];
        _colorChangerIcon.DOFade(0, 0);
        _collider.size = new Vector2(_rectTransform.rect.width, _rectTransform.rect.height);
        _hovering = false;
    }

    public void Scale(Vector2 delta)
    {
        if (delta.x > 0)
        {
            _newScaleX = _rectTransform.sizeDelta.x + _scalingSpeed;
        }
        else if (delta.x < 0)
        {
            _newScaleX = _rectTransform.sizeDelta.x - _scalingSpeed;
        }

        if (delta.y > 0)
        {
            _newScaleY = _rectTransform.sizeDelta.y + _scalingSpeed;
        }
        else if (delta.y < 0)
        {
            _newScaleY = _rectTransform.sizeDelta.y - _scalingSpeed;
        }

        _newScaleX = Mathf.Clamp(_newScaleX, _minScale, _maxScale);
        _newScaleY = Mathf.Clamp(_newScaleY, _minScale, _maxScale);
        _rectTransform.sizeDelta = new Vector2(_newScaleX, _newScaleY);
        _collider.size = new Vector2(_rectTransform.rect.width, _rectTransform.rect.height);
    }

    public void Rotate(Vector2 delta)
    {
        if (delta.x > _rotationSensitivity)
        {
            _rectTransform.eulerAngles = new Vector3(_rectTransform.eulerAngles.x, _rectTransform.eulerAngles.y - _rotationAngle, _rectTransform.eulerAngles.z);
        }
        else if (delta.x < -1 * _rotationSensitivity)
        {
            _rectTransform.eulerAngles = new Vector3(_rectTransform.eulerAngles.x, _rectTransform.eulerAngles.y + _rotationAngle, _rectTransform.eulerAngles.z);
        }

        // x축 회전은 살짝 문제가 있어서 보류
        /*
        if (eventData.delta.y > _rotationSensitivity)
        {
            _contentTransform.eulerAngles = new Vector3(Mathf.Clamp(_contentTransform.eulerAngles.x + _rotationAngle, _minRotation, _maxRotation), _contentTransform.eulerAngles.y, _contentTransform.eulerAngles.z);
        }
        else if (eventData.delta.y < -1 * _rotationSensitivity)
        {
            _contentTransform.eulerAngles = new Vector3(Mathf.Clamp(_contentTransform.eulerAngles.x - _rotationAngle, _minRotation, _maxRotation), _contentTransform.eulerAngles.y, _contentTransform.eulerAngles.z);
        }
        */
    }

    // 스티키노트의 색깔을 바꾸는 함수
    public void OnClick_Color()
    {
        if (!_stickyNote.isLocked)
        {
            _colorIndex = GetNextColorIndex(_colorIndex);
            _contentImage.color = _backgroundColors[_colorIndex];
            _colorChangerIcon.color = _backgroundColors[GetNextColorIndex(_colorIndex)];
        }
    }

    private int GetNextColorIndex(int i)
    {
        if (i + 1 > _backgroundColors.Length - 1)
        {
            return 0;
        }
        else
        {
            return i + 1;
        }
    }

    // 스티키노트의 색깔 변화 아이콘을 안보이게 하는 함수
    public void HideColorIcon()
    {
        _colorChangerIcon.DOFade(0, 0.4f);
    }

    // 스티키노트에 마우스 포인터가 들어왔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        _hovering = true;
        _stickyNote.ControllerCanvas.ShowController();
        _colorChangerIcon.DOFade(1, 0.4f);
    }

    // 스티키노트로부터 마우스 포인터가 나갔을 때
    private void OnPointerExit(PointerEventData eventData)
    {
        _hovering = false;
        StartCoroutine(_stickyNote.ControllerCanvas.HideController());
    }

    // 스티키노트에 마우스 클릭했을 때
    private void OnPointerDown(PointerEventData eventData)
    {
        float timeClickedBefore = _clickedTime;
        
        if (Time.time - timeClickedBefore < _doubleClickTime)
        {
            // 더블클릭으로 인식
            OnDoubleClick();
        }
        _clickedTime = Time.time;
    }

    private void OnDoubleClick()
    {
        ShowStickyNoteDetail(_contentText.text);
    }

    private void OnScrollContent(PointerEventData eventData)
    {
        switch (_stickyNote.CurrentState)
        {
            case (StickyNoteState.Edit):
                {
                    if (eventData.scrollDelta.y > 0)
                    {
                        _newFontSize = _contentText.fontSize + _fontSizingSpeed;
                    }
                    else
                    {
                        _newFontSize = _contentText.fontSize - _fontSizingSpeed;
                    }
                    _newFontSize = Mathf.Clamp(_newFontSize, _minFontSize, _maxFontSize);

                    _contentText.fontSize = _newFontSize;
                    break;
                }
        }
    }

    private void ShowStickyNoteDetail(string text)
    {
        GameObject go = Instantiate(Resources.Load("StickyNote/StickyNoteDetailCanvas") as GameObject, Vector3.zero, Quaternion.identity);
        go.GetComponent<StickyNoteCanvas>().SetText(text);
    }

    // 스티키노트 편집하는 인풋필드 값이 바뀔 때마다 실제 스티키노트에 반영하는 함수
    public void OnValueChanged(string value)
    {
        _contentText.text = value;
    }
}
