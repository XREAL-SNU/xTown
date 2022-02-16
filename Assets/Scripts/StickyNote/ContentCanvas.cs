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
    private Text _contentText;
    public Text ContentText { get { return _contentText; } }
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private BoxCollider _collider;
    [SerializeField]
    private Color[] _backgroundColors;
    [SerializeField]
    private Button _colorChangerButton;
    [SerializeField]
    private Image _colorChangerIcon;
    [SerializeField]
    private Button _copyToClipboardButton;
    [SerializeField]
    private Image _copyToClipboardIcon;
    [SerializeField]
    private Button _exportToTxtButton;
    [SerializeField]
    private Image _exportToTxtIcon;

    private StickyNote _stickyNote;

    // ������ ���� ���� ������
    private float _scalingSpeed = 15f;
    private float _minScale = 200f;
    private float _maxScale = 2000f;
    private float _newScaleX = 0f;
    private float _newScaleY = 0f;

    // ���� ���� ���� ������
    private float _rotationSensitivity = 1; // �� ���� Ŭ���� ȸ���� �ʿ��� ���콺 �������� �� Ŀ��. 
    private int _rotationAngle = 10; // �� ���� ȸ���ϴ� ����
    // private int _minRotation = -60;
    // private int _maxRotation = 60;

    // ��Ʈ ũ�� ���� ���� ������
    private int _fontSizingSpeed = 4;
    private int _minFontSize = 30;
    private int _maxFontSize = 120;
    private int _newFontSize = 0;

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
        _copyToClipboardButton.onClick.AddListener(OnClick_Copy);
        _exportToTxtButton.onClick.AddListener(OnClick_Export);

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
        _copyToClipboardIcon.color = _backgroundColors[GetNextColorIndex(_colorIndex)];
        _copyToClipboardIcon.DOFade(0, 0);
        _exportToTxtIcon.color = _backgroundColors[GetNextColorIndex(_colorIndex)];
        _exportToTxtIcon.DOFade(0, 0);

        _collider.size = new Vector2(_rectTransform.rect.width * 0.005f, _rectTransform.rect.height * 0.005f);
        _hovering = false;
        _view = GetComponent<PhotonView>();
        _transformView = GetComponent<PhotonTransformView>();
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
        _collider.size = new Vector3(_rectTransform.rect.width * 0.005f, _rectTransform.rect.height * 0.005f, 0.01f);
    }
    [PunRPC]
    public void Rotate(Vector2 delta)
    {
        if (delta.x > _rotationSensitivity)
        {
            _stickyNote.transform.eulerAngles = new Vector3(0, _stickyNote.transform.eulerAngles.y - _rotationAngle, 0);
        }
        else if (delta.x < -1 * _rotationSensitivity)
        {
            _stickyNote.transform.eulerAngles = new Vector3(0, _stickyNote.transform.eulerAngles.y + _rotationAngle, 0);
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

    public void OnClick_Copy()
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = _contentText.text;
        textEditor.SelectAll();
        textEditor.Copy();
    }

    public void OnClick_Export()
    {
        Debug.Log("clicked export button");
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
    public void HideIcons()
    {
        _colorChangerIcon.DOFade(0, 0.4f);
        _copyToClipboardIcon.DOFade(0, 0.4f);
        _exportToTxtIcon.DOFade(0, 0.4f);
    }

    public void ShowIcons()
    {
        _colorChangerIcon.DOFade(1, 0.4f);
        _copyToClipboardIcon.DOFade(1, 0.4f);
        _exportToTxtIcon.DOFade(1, 0.4f);
    }

    // ��ƼŰ��Ʈ�� ���콺 �����Ͱ� ������ ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        _hovering = true;
        _stickyNote.ControllerCanvas.ShowController();
        ShowIcons();
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
        _copyToClipboardIcon.color = _backgroundColors[GetNextColorIndex(_colorIndex)];
        _exportToTxtIcon.color = _backgroundColors[GetNextColorIndex(_colorIndex)];
    }

    [PunRPC]
    public void ChangeText(string value)
    {
        _contentText.text = value;
    }

}
