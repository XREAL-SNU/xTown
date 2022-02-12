using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using Photon.Pun;


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
        // �̺�Ʈ Ʈ���ſ� ���콺 ������ Enter/Exit �Լ� ���ε�
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerEnterEntry.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
        pointerExitEntry.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
        trigger.triggers.Add(pointerEnterEntry);
        trigger.triggers.Add(pointerExitEntry);

        // Lock ��ư�� ��� �Լ� ���ε�
        _lockButton.onClick.AddListener(OnClick_Lock);

        // Edit ��ư�� ���� ���� �Լ� ���ε�
        _editButton.onClick.AddListener(OnClick_Edit);

        // Remove ��ư�� ��ƼŰ��Ʈ ���� �Լ� ���ε�
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

        // Scale ��ư �̺�Ʈ Ʈ���ſ� ������ �Լ� ���ε�
        EventTrigger scaleTrigger = _scaleButton.GetComponent<EventTrigger>();
        EventTrigger.Entry scaleEntry = new EventTrigger.Entry();
        scaleEntry.eventID = EventTriggerType.Drag;
        scaleEntry.callback.AddListener((data) => { OnDrag_Scale((PointerEventData)data); });
        scaleTrigger.triggers.Add(scaleEntry);

        // Rotate ��ư �̺�Ʈ Ʈ���ſ� ȸ�� �Լ� ���ε�
        EventTrigger rotateTrigger = _rotateButton.GetComponent<EventTrigger>();
        EventTrigger.Entry rotateEntry = new EventTrigger.Entry();
        rotateEntry.eventID = EventTriggerType.Drag;
        rotateEntry.callback.AddListener((data) => { OnDrag_Rotate((PointerEventData)data); });
        rotateTrigger.triggers.Add(rotateEntry);

        _stickyNote.onLock += OnLock;
        _stickyNote.onUnlock += OnUnlock;

        _background.transform.DOScale(0, 0);
        _hovering = false;
        _stickyNote.Lock();
    }

    void Update()
    {
        _background.transform.position = Camera.main.WorldToScreenPoint(_stickyNote.ContentCanvas.ControllerTarget.position);
    }

    public void Initialize(StickyNote stickyNote)
    {
        _stickyNote = stickyNote;
    }

    // ��ƼŰ��Ʈ ��Ʈ�ѷ� UI�� ����� �ڷ�ƾ
    public IEnumerator HideController()
    {
        yield return new WaitForSeconds(0.5f);

        // ���콺 �����Ͱ� ��ƼŰ��Ʈ ��Ʈ�ѷ��� ��ƼŰ��Ʈ ��ü���� ����� ���, ��ƼŰ��Ʈ ��Ʈ�ѷ��� ����
        if (_hovering || _stickyNote.ContentCanvas.hovering)
        {
            yield return null;
        }
        else
        {
            _background.transform.DOScale(0, 0.4f);
            _stickyNote.ContentCanvas.HideIcons();
        }
    }

    // ��ƼŰ��Ʈ ��Ʈ�ѷ��� ���콺 �����Ͱ� ������ ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        _hovering = true;
    }

    // ��ƼŰ��Ʈ ��Ʈ�ѷ��κ��� ���콺 �����Ͱ� ������ ��
    private void OnPointerExit(PointerEventData eventData)
    {
        _hovering = false;
        StartCoroutine(HideController());
    }

    // ��ƼŰ��Ʈ ��Ʈ�ѷ��� Scale ��ư�� �巡������ ��
    public void OnDrag_Scale(PointerEventData eventData)
    {
        /*if(!StickyNoteNetworkManager.Instance.networked)
            _stickyNote.ContentCanvas.Scale(eventData.delta);
        else*/
            _stickyNote.ContentCanvas._view.RPC("Scale",RpcTarget.All,eventData.delta);
    }

    // ��ƼŰ��Ʈ ��Ʈ�ѷ��� Rotate ��ư�� �巡������ ��
    public void OnDrag_Rotate(PointerEventData eventData)
    {
        /*if(!StickyNoteNetworkManager.Instance.networked)
            _stickyNote.ContentCanvas.Rotate(eventData.delta);
        else*/
            _stickyNote.ContentCanvas._view.RPC("Rotate",RpcTarget.All,eventData.delta);
    }

    // ��ƼŰ��Ʈ ��Ʈ�ѷ��� Edit ��ư�� ������ ��
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
            /*if(!StickyNoteNetworkManager.Instance.networked)
            {
                Destroy(_stickyNote.gameObject);
            }
            else
            {*/
                PhotonNetwork.Destroy(_stickyNote.gameObject);
            //}
        }
    }

    private void OnClick_Lock()
    {
        /*if(!StickyNoteNetworkManager.Instance.networked)
        {
            if (_stickyNote.isLocked)
            {
                UnLock();
            }
            else
            {
                Lock();
            }
        }
        else
        {*/
            if (_stickyNote.isLocked)
            {
                UnLock();
                _stickyNote._view.RPC("Lock",RpcTarget.Others);
            }
            else
            {
                Lock();
            }
        //}
    }
    [PunRPC]
    private void Lock()
    {
        _stickyNote.Lock();
    }
    [PunRPC]
    private void UnLock()
    {
        _stickyNote.Unlock();
    }

    // ��ƼŰ��Ʈ ��Ʈ�ѷ� UI�� ��Ÿ���� �ϴ� �Լ�
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
        _stickyNote._view.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
        _stickyNote.ContentCanvas._view.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
        _editButton.gameObject.SetActive(true);
        _scaleButton.gameObject.SetActive(true);
        _rotateButton.gameObject.SetActive(true);
        _removeButton.gameObject.SetActive(true);
    }
}
