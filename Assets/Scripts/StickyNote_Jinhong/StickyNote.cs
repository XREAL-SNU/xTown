using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class StickyNote : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField]
    private ContentCanvas _contentCanvas;
    public ContentCanvas ContentCanvas { get { return _contentCanvas; } }
    [SerializeField]
    private ControllerCanvas _controllerCanvas;
    public ControllerCanvas ControllerCanvas { get { return _controllerCanvas; } }
    [SerializeField]
    private EditCanvas _editCanvas;
    public EditCanvas EditCanvas { get { return _editCanvas; } }


    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _contentCanvas.Initialize(this);
        _controllerCanvas.Initialize(this);
        _editCanvas.Initialize(this);
    }
}
