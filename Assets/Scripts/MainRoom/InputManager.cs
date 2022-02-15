using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private InputField[] _inputFields;
    [SerializeField]
    private TMP_InputField[] _TMPinputFields;
    [SerializeField]
    private Button[] _lockUI;
    [SerializeField]
    private Button[] _unlockUI;

    private bool _islock = false;


    public void Start()
    {
        foreach (InputField input in _inputFields)
        {
            input.onValueChanged.AddListener(delegate { Lock(); });
            input.onEndEdit.AddListener(delegate { UnLock(); });
        }

        foreach (TMP_InputField input in _TMPinputFields)
        {
            input.onValueChanged.AddListener(delegate { Lock(); });
            input.onEndEdit.AddListener(delegate { UnLock(); });
        }
    }

    public static void  Lock()
    {
        PlayerKeyboard.PlayerInputLock();
        PlayerMouse.PlayerInputLock();
    }

    public static void UnLock()
    {
        PlayerKeyboard.PlayerInputUnLock();
        PlayerMouse.PlayerInputUnLock();
    }

    public void ButtonLock()
    {
        _islock = !_islock;
        if (!_islock)
        {
            UnLock();
            Debug.Log("unlock");
        }
        else
        {
            Lock();
            Debug.Log("lock");
        }
    }
}
