using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using Photon.Pun;

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

    // ������ ���� ���� ������
    private float _scalingSpeed = 0.05f;
    private float _minScale = 1f;
    private float _maxScale = 10f;
    private float _newScaleX = 0f;
    private float _newScaleY = 0f;

    // ���� ���� ���� ������
    private float _rotationSensitivity = 1; // �� ���� Ŭ���� ȸ���� �ʿ��� ���콺 �������� �� Ŀ��. 
    private int _rotationAngle = 10; // �� ���� ȸ���ϴ� ����
    // private int _minRotation = -60;
    // private int _maxRotation = 60;

    // ��Ʈ ũ�� ���� ���� ������
    private float _fontSizingSpeed = 0.02f;
    private float _minFontSize = 0.15f;
    private float _maxFontSize = 0.6f;
    private float _newFontSize = 0f;

    // ��ƼŰ��Ʈ ���� ���� ����
    private int _colorIndex = 0;

    // ��ƼŰ��Ʈ ����Ŭ�� �ν� ���� ����
    private float _clickedTime;
    private float _doubleClickTime = 0.25f;
    public PhotonView _view;
    public PhotonTransformView _transformView;

    public void Initialize(StickyNote stickyNote)
    {
        _stickyNote = stickyNote;
    }

    private void Start()
    {
        // ��ƼŰ��Ʈ ���� ��ȭ ��ư�� ���� ��ȭ �Լ� ���ε�
        _colorChangerButton.onClick.AddListener(OnClick_Color);

        // �̺�Ʈ Ʈ���ſ� ���콺 ������ Enter/Exit �Լ� ���ε�
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


        // �̺�Ʈ Ʈ���ſ� ���콺 Ŭ�� �Լ� ���ε� (����Ŭ�� �ν� ���ؼ�)
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(pointerDownEntry);
        
        _colorChangerIcon.color = _backgroundColors[GetNextColorIndex(_colorIndex)];
        _colorChangerIcon.DOFade(0, 0);
        _collider.size = new Vector2(_rectTransform.rect.width, _rectTransform.rect.height);
        _hovering = false;
        _view = GetComponent<PhotonView>();
        _transformView = GetComponent<PhotonTransformView>();

        GetComponent<GraphicRaycaster>().ignoreReversedGraphics = false;
    }

    [PunRPC]
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
    [PunRPC]
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

        // x�� ȸ���� ��¦ ������ �־ ����
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

    // ��ƼŰ��Ʈ�� ������ �ٲٴ� �Լ�
    public void OnClick_Color()
    {
        if (!_stickyNote.isLocked)
        {
            /*if(!StickyNoteNetworkManager.Instance.networked)
            {
                ChangeColor();
            }
            else
            {*/
                _view.RPC("ChangeColor",RpcTarget.All);
            //}
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

    // ��ƼŰ��Ʈ�� ���� ��ȭ �������� �Ⱥ��̰� �ϴ� �Լ�
    public void HideColorIcon()
    {
        _colorChangerIcon.DOFade(0, 0.4f);
    }

    // ��ƼŰ��Ʈ�� ���콺 �����Ͱ� ������ ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        _hovering = true;
        _stickyNote.ControllerCanvas.ShowController();
        _colorChangerIcon.DOFade(1, 0.4f);
    }

    // ��ƼŰ��Ʈ�κ��� ���콺 �����Ͱ� ������ ��
    private void OnPointerExit(PointerEventData eventData)
    {
        _hovering = false;
        StartCoroutine(_stickyNote.ControllerCanvas.HideController());
    }

    // ��ƼŰ��Ʈ�� ���콺 Ŭ������ ��
    private void OnPointerDown(PointerEventData eventData)
    {
        float timeClickedBefore = _clickedTime;
        
        if (Time.time - timeClickedBefore < _doubleClickTime)
        {
            // ����Ŭ������ �ν�
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
        GameObject go = Instantiate(Resources.Load("StickyNoteDetailCanvas") as GameObject, Vector3.zero, Quaternion.identity);
        go.GetComponent<StickyNoteCanvas>().SetText(text);
    }

    // ��ƼŰ��Ʈ �����ϴ� ��ǲ�ʵ� ���� �ٲ� ������ ���� ��ƼŰ��Ʈ�� �ݿ��ϴ� �Լ�
    public void OnValueChanged(string value)
    {
        /*Debug.Log("ValueChange");
        if(!StickyNoteNetworkManager.Instance.networked)
        {
            ChangeText(value);
        }else
        {*/
            _view.RPC("ChangeText",RpcTarget.All,value);
        //}
    }

    [PunRPC]
    public void ChangeColor()
    {
        _colorIndex = GetNextColorIndex(_colorIndex);
        _contentImage.color = _backgroundColors[_colorIndex];            
        _colorChangerIcon.color = _backgroundColors[GetNextColorIndex(_colorIndex)];
    }
    [PunRPC]
    public void ChangeText(string value)
    {
        _contentText.text = value;
    }

}
