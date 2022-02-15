using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPManager : MonoBehaviour
{
    private static FPManager _instance;

    public static FPManager Instance
    {
        get { return _instance; }
    }

    private FirstPersonControl _firstPersonControl;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        _firstPersonControl = new FirstPersonControl();
    }

    private void OnEnable()
    {
        _firstPersonControl.Enable();
    }

    private void OnDisable()
    {
        _firstPersonControl.Disable();
    }

    public Vector2 GetMouseDelta()
    {
        return _firstPersonControl.FPS.Look.ReadValue<Vector2>();
    }
}
