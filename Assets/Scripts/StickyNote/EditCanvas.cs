using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class EditCanvas : MonoBehaviour
{
    private StickyNote _stickyNote;

    [SerializeField]
    private TMP_InputField _inputField;
    [SerializeField]
    private Button _confirmButton;
    private static PhotonView _view;

    public void Initialize(StickyNote stickyNote)
    {
        _stickyNote = stickyNote;
        _inputField.onValueChanged.AddListener(_stickyNote.ContentCanvas.OnValueChanged);
        _inputField.onSubmit.AddListener(FocusInputfield);
        _confirmButton.onClick.AddListener(OnClick_Confirm);
        _view = GetComponent<PhotonView>();

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _inputField.text = _stickyNote.ContentCanvas.ContentText.text;
        //if(!StickyNoteNetworkManager.Instance.networked)
        //{
        //    SetEdit();
        //}
        //else
        //{
            _view.RPC("SetEdit",RpcTarget.All);
        //}
        FocusInputfield(null);
    }



    public void Hide()
    {
        //if(!StickyNoteNetworkManager.Instance.networked)
        //{
        //    SetEditDone(_inputField.text);
        //}
        //else
        //{
            _view.RPC("SetEditDone",RpcTarget.All,_inputField.text);
        //}
        
        gameObject.SetActive(false);
    }

    public void OnClick_Confirm()
    {
        Hide();
    }

    public void FocusInputfield(string value)
    {
        _inputField.ActivateInputField();
        StartCoroutine(MoveToEnd());
    }

    IEnumerator MoveToEnd()
    {
        yield return new WaitForEndOfFrame();
        _inputField.MoveTextEnd(true);
    }

    [PunRPC]
    public void SetEdit()
    {
        _stickyNote.CurrentState = StickyNoteState.Edit;
    }
    [PunRPC]
    public void SetEditDone(string finalText)
    {
        _stickyNote.CurrentState = StickyNoteState.Idle;
        //_stickyNote.ContentCanvas.ContentText.text = finalText;
    }
}
