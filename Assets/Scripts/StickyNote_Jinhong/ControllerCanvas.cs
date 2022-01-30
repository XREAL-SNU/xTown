using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;


public class ControllerCanvas : MonoBehaviour
{
    private StickyNote _stickyNote;

    [SerializeField]
    private Image _background;
    [SerializeField]
    private Button _lockButton;
    [SerializeField]
    private Button _editButton;
    [SerializeField]
    private Button _scaleButton;
    [SerializeField]
    private Button _rotateButton;
    [SerializeField]
    private Button _removeButton;
    [SerializeField]
    private RadialProgress _removeProgressBar;

    private bool _hovering;


    void Start()
    {
        // 이벤트 트리거에 마우스 포인터 Enter/Exit 함수 바인딩
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerEnterEntry.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
        pointerExitEntry.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
        trigger.triggers.Add(pointerEnterEntry);
        trigger.triggers.Add(pointerExitEntry);

        // Lock 버튼에 잠금 함수 바인딩
        _lockButton.onClick.AddListener(OnClick_Lock);

        // Edit 버튼에 내용 편집 함수 바인딩
        _editButton.onClick.AddListener(OnClick_Edit);

        // Remove 버튼에 스티키노트 제거 함수 바인딩
        // _removeButton.onClick.AddListener(OnClick_Remove);

        EventTrigger removeTrigger = _removeButton.GetComponent<EventTrigger>();
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerDownEntry.callback.AddListener((data) => { OnPointerDown_Remove((PointerEventData)data); });
        pointerUpEntry.callback.AddListener((data) => { OnPointerUp_Remove((PointerEventData)data); });
        removeTrigger.triggers.Add(pointerDownEntry);
        removeTrigger.triggers.Add(pointerUpEntry);

        // Scale 버튼 이벤트 트리거에 스케일 함수 바인딩
        EventTrigger scaleTrigger = _scaleButton.GetComponent<EventTrigger>();
        EventTrigger.Entry scaleEntry = new EventTrigger.Entry();
        scaleEntry.eventID = EventTriggerType.Drag;
        scaleEntry.callback.AddListener((data) => { OnDrag_Scale((PointerEventData)data); });
        scaleTrigger.triggers.Add(scaleEntry);

        // Rotate 버튼 이벤트 트리거에 회전 함수 바인딩
        EventTrigger rotateTrigger = _rotateButton.GetComponent<EventTrigger>();
        EventTrigger.Entry rotateEntry = new EventTrigger.Entry();
        rotateEntry.eventID = EventTriggerType.Drag;
        rotateEntry.callback.AddListener((data) => { OnDrag_Rotate((PointerEventData)data); });
        rotateTrigger.triggers.Add(rotateEntry);

        _stickyNote.onLock += OnLock;
        _stickyNote.onUnlock += OnUnlock;

        _background.transform.DOScale(0, 0);
        _hovering = false;
    }

    void Update()
    {
        _background.transform.position = Camera.main.WorldToScreenPoint(_stickyNote.ContentCanvas.ControllerTarget.position);
    }

    public void Initialize(StickyNote stickyNote)
    {
        _stickyNote = stickyNote;
    }

    // 스티키노트 컨트롤러 UI를 숨기는 코루틴
    public IEnumerator HideController()
    {
        yield return new WaitForSeconds(0.5f);

        // 마우스 포인터가 스티키노트 컨트롤러나 스티키노트 자체에서 벗어났을 경우, 스티키노트 컨트롤러를 숨김
        if (_hovering || _stickyNote.ContentCanvas.hovering)
        {
            yield return null;
        }
        else
        {
            _background.transform.DOScale(0, 0.4f);
            _stickyNote.ContentCanvas.HideColorIcon();
        }
    }

    // 스티키노트 컨트롤러에 마우스 포인터가 들어왔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        _hovering = true;
    }

    // 스티키노트 컨트롤러로부터 마우스 포인터가 나갔을 때
    private void OnPointerExit(PointerEventData eventData)
    {
        _hovering = false;
        StartCoroutine(HideController());
    }

    // 스티키노트 컨트롤러의 Scale 버튼을 드래그했을 때
    public void OnDrag_Scale(PointerEventData eventData)
    {
        _stickyNote.ContentCanvas.Scale(eventData.delta);
    }

    // 스티키노트 컨트롤러의 Rotate 버튼을 드래그했을 때
    public void OnDrag_Rotate(PointerEventData eventData)
    {
        _stickyNote.ContentCanvas.Rotate(eventData.delta);
    }

    // 스티키노트 컨트롤러의 Edit 버튼을 눌렀을 때
    public void OnClick_Edit()
    {
        _stickyNote.EditCanvas.Show();
    }

    public void OnPointerDown_Remove(PointerEventData eventData)
    {
        _removeProgressBar.Activate();
    }

    public void OnPointerUp_Remove(PointerEventData eventData)
    {
        _removeProgressBar.Deactivate();

        if (_removeProgressBar.done)
        {
            Destroy(_stickyNote.gameObject);
        }
    }

    private void OnClick_Lock()
    {
        if (_stickyNote.isLocked)
        {
            _stickyNote.Unlock();
        }
        else
        {
            _stickyNote.Lock();
        }
    }

    // 스티키노트 컨트롤러 UI를 나타나게 하는 함수
    public void ShowController()
    {
        _background.transform.DOScale(1, 0.4f);
    }

    private void OnLock()
    {
        _lockButton.GetComponentInChildren<TMP_Text>().text = "Unlock";
        _editButton.gameObject.SetActive(false);
        _scaleButton.gameObject.SetActive(false);
        _rotateButton.gameObject.SetActive(false);
        _removeButton.gameObject.SetActive(false);
    }

    private void OnUnlock()
    {
        _lockButton.GetComponentInChildren<TMP_Text>().text = "Lock";
        _editButton.gameObject.SetActive(true);
        _scaleButton.gameObject.SetActive(true);
        _rotateButton.gameObject.SetActive(true);
        _removeButton.gameObject.SetActive(true);
    }
}
