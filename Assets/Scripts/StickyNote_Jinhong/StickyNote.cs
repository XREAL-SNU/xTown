using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public enum StickyNoteState
{
    Edit,
    Move,
    Idle
}

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

    private bool _isLocked;
    public bool isLocked { get { return _isLocked; } }

    public event Action onLock;
    public event Action onUnlock;

    private StickyNoteState _currentState;
    public StickyNoteState CurrentState { get; set; }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _contentCanvas.Initialize(this);
        _controllerCanvas.Initialize(this);
        _editCanvas.Initialize(this);

        _currentState = StickyNoteState.Idle;
    }

    public void Lock()
    {
        if (onLock != null)
        {
            onLock();
        }
        _isLocked = true;
    }

    public void Unlock()
    {
        if (onUnlock != null)
        {
            onUnlock();
        }
        _isLocked = false;
    }
}
