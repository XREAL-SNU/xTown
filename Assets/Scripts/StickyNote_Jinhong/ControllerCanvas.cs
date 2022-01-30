using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


public class ControllerCanvas : MonoBehaviour
{
    private StickyNote _stickyNote;

    [SerializeField]
    private Image _background;
    [SerializeField]
    private Button _editButton;
    [SerializeField]
    private Button _scaleButton;
    [SerializeField]
    private Button _rotateButton;
    [SerializeField]
    private Button _removeButton;

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

        // Edit 버튼에 내용 편집 함수 바인딩
        _editButton.onClick.AddListener(OnClick_Edit);

        // Remove 버튼에 스티키노트 제거 함수 바인딩
        _removeButton.onClick.AddListener(OnClick_Remove);

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

    // 스티키노트 컨트롤러의 Remove 버튼을 눌렀을 때
    public void OnClick_Remove()
    {
        Destroy(_stickyNote.gameObject);
    }

    // 스티키노트 컨트롤러 UI를 나타나게 하는 함수
    public void ShowController()
    {
        _background.transform.DOScale(1, 0.4f);
    }
}
