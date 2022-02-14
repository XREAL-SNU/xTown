using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchivingController : MonoBehaviour
{
    [SerializeField]
    private GameObject _archivingCamera;

    private bool _isTriggered;
    public bool isTriggered { get { return _isTriggered; } set { _isTriggered = value; } }

    public void OnTriggerChange()
    {
        _archivingCamera.SetActive(_isTriggered);
    }
}
