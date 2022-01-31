using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StickyNoteCanvas : MonoBehaviour
{
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private TMP_Text _contentText;

    private void Start()
    {
        _closeButton.onClick.AddListener(Close);
    }

    private void Close()
    {
        Destroy(gameObject);
    }

    public void SetText(string text)
    {
        _contentText.text = text;
    }
}
