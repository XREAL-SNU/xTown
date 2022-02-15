using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarInteractionCanvas : MonoBehaviour
{
    private MainCanvases _mainCanvases;

    private void Start() { Hide(); }
    public void FirstInitialize(MainCanvases canvases)
    {
        _mainCanvases = canvases;
    }
    public void OnClick_Exit()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
