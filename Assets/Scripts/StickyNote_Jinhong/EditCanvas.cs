using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditCanvas : MonoBehaviour
{
    private StickyNote _stickyNote;

    [SerializeField]
    private TMP_InputField _inputField;
    [SerializeField]
    private Button _confirmButton;

    public void Initialize(StickyNote stickyNote)
    {
        _stickyNote = stickyNote;

        _inputField.onValueChanged.AddListener(_stickyNote.ContentCanvas.OnValueChanged);
        _confirmButton.onClick.AddListener(OnClick_Confirm);
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _inputField.text = _stickyNote.ContentCanvas.ContentText.text;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnClick_Confirm()
    {
        Hide();
    }
}
