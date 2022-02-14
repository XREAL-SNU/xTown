using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCanvas : MonoBehaviour
{

    private RoomsCanvases _roomCanvases;
    private string _linkedSceneName;
    public string LinkedSceneName
    {
        get => _linkedSceneName;
        set
        {
            Debug.Log("linked create game scene " + value);
            _linkedSceneName = value;
        }
    }
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
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
