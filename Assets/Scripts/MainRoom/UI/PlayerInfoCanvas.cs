using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoCanvas : MonoBehaviour
{
    private MainCanvases _mainCanvases;

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
