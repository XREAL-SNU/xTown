using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private InputField[] _inputFields;
    //[SerializeField]
    //private TMP_InputField[] _TMPinputFields;

    private bool _islock = false;


    public void Start()
    {
        foreach (InputField input in _inputFields)
        {
            input.onValueChanged.AddListener(delegate { Lock(); });
            input.onEndEdit.AddListener(delegate { UnLock(); });
        }

        /*foreach (TMP_InputField input in _TMPinputFields)
        {
            input.onValueChanged.AddListener(delegate { Lock(); });
            input.onEndEdit.AddListener(delegate { UnLock(); });
        }*/
    }

    public static void Lock()
    {
        PlayerKeyboard.InputLockAll(true);
        PlayerMouse.InputLockAll(true);
    }

    public static void UnLock()
    {
        PlayerKeyboard.InputLockAll(false);
        PlayerMouse.InputLockAll(false);
    }

    public void ButtonLock()
    {
        _islock = !_islock;
        if (!_islock) UnLock();
        else Lock();
    }
}
