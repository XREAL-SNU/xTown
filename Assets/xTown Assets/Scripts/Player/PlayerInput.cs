using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerKeyboard
{
    public enum KeyboardInputName
    {
        W, A, S, D, T, G, Alpha1, Alpha2, Alpha3, Alpha4
    };

    public enum KeyboardInputType
    {
        Key, KeyDown, KeyUp
    }

    public bool KeyboardAvailable;
    public string KeyboardSetName;
    public KeyboardInput[] KeyboardInputs;

    // lock 제한을 받을 input set
    public static KeyboardInput[] EmotionKeyboardSet = new KeyboardInput[]
    {
        global::KeyboardInput.EmotionToggle, global::KeyboardInput.Emotion1, global::KeyboardInput.Emotion2, global::KeyboardInput.Emotion3, global::KeyboardInput.Emotion4
    };

    public static KeyboardInput[] CameraKeyboardSet = new KeyboardInput[]
    {
        global::KeyboardInput.CameraViewChange
    };

    public static PlayerKeyboard MovementPlayerKeyboard = new PlayerKeyboard("Movement");
    public static PlayerKeyboard EmotionPlayerKeyboard = new PlayerKeyboard("Emotion", EmotionKeyboardSet);
    public static PlayerKeyboard CameraPlayerKeyboard = new PlayerKeyboard("Camera", CameraKeyboardSet);

    static List<PlayerKeyboard> PlayerKeyboards = new List<PlayerKeyboard> 
    { 
        MovementPlayerKeyboard, EmotionPlayerKeyboard, CameraPlayerKeyboard 
    };

    public PlayerKeyboard(string name)
    {
        this.KeyboardSetName = name;
        this.KeyboardAvailable = true;
    }

    public PlayerKeyboard(string name, KeyboardInput[] keySet)
    {
        this.KeyboardSetName = name;
        this.KeyboardInputs = keySet;
        this.KeyboardAvailable = true;
    }

    public static PlayerKeyboard GetPlayerKeyboard(string name)
    {
        foreach(PlayerKeyboard set in PlayerKeyboards)
        {
            if (set.KeyboardSetName.Equals(name)) return set;
        }
        Debug.LogError($"{name} PlayerKeyboard is null");
        return null;
    }

    public static bool KeyboardInput(string name, KeyboardInput input)
    {
        foreach(KeyboardInput key in GetPlayerKeyboard(name).KeyboardInputs)
        {
            if (input.Equals(key))
            {
                switch (input.inputKeyType)
                {
                    case KeyboardInputType.Key:
                        return Input.GetKey(input.ToKeyCode()) && GetPlayerKeyboard(name).KeyboardAvailable;
                    case KeyboardInputType.KeyDown:
                        return Input.GetKeyDown(input.ToKeyCode()) && GetPlayerKeyboard(name).KeyboardAvailable;
                    case KeyboardInputType.KeyUp:
                        return Input.GetKeyUp(input.ToKeyCode()) && GetPlayerKeyboard(name).KeyboardAvailable;
                }
            }
        }
        Debug.LogError("KeyboardInput Error");
        return false;
    }

    public void InputLock()
    {
        this.KeyboardAvailable = false;
    }

    public void InputUnLock()
    {
        this.KeyboardAvailable = true;
    }

    public void InputUnLockOnly()
    {
        foreach(PlayerKeyboard set in PlayerKeyboards)
        {
            set.InputLock();
        }
        this.InputUnLock();
    }

    public static void InputLockAll(bool isLock)
    {
        foreach(PlayerKeyboard set in PlayerKeyboards)
        {
            if (isLock) set.InputLock();
            else set.InputUnLock();
        }
    }

}

[Serializable]
public class PlayerMouse
{
    public enum MouseInputName
    {
        Left, Right, Wheel
    }

    public enum MouseInputType
    {
        Mouse, MouseDown, MouseUp
    }

    public bool MouseAvailable;
    public string MouseSetName;
    public MouseInput[] MouseInputs;

    public static MouseInput[] CameraMouseSet = new MouseInput[]
    {
        global::MouseInput.CameraDrag, global::MouseInput.CameraDragExit
    };

    public static PlayerMouse WheelPlayerMouse = new PlayerMouse("Zoom");
    public static PlayerMouse CameraPlayerMouse = new PlayerMouse("Camera", CameraMouseSet);

    static List<PlayerMouse> PlayerMice = new List<PlayerMouse> { WheelPlayerMouse, CameraPlayerMouse };

    public PlayerMouse(string name)
    {
        this.MouseSetName = name;
        this.MouseAvailable = true;
    }

    public PlayerMouse(string name, MouseInput[] keySet)
    {
        this.MouseSetName = name;
        this.MouseInputs = keySet;
        this.MouseAvailable = true;
    }

    public static PlayerMouse GetPlayerMouse(string name)
    {
        foreach(PlayerMouse set in PlayerMice)
        {
            if (set.MouseSetName.Equals(name)) return set;
        }
        Debug.LogError($"{name} PlayerMouse is null");
        return null;
    }

    public static bool MouseInput(string name, MouseInput input)
    {
        foreach(MouseInput key in GetPlayerMouse(name).MouseInputs)
        {
            if (input.Equals(key))
            {
                switch (input.inputMouseType)
                {
                    case MouseInputType.Mouse:
                        return Input.GetMouseButton((int)input.inputMouseName) && GetPlayerMouse(name).MouseAvailable;
                    case MouseInputType.MouseDown:
                        return Input.GetMouseButtonDown((int)input.inputMouseName) && GetPlayerMouse(name).MouseAvailable;
                    case MouseInputType.MouseUp:
                        return Input.GetMouseButtonUp((int)input.inputMouseName) && GetPlayerMouse(name).MouseAvailable;
                }
            }
        }
        Debug.LogError("MouseInput Error");
        return false;
    }

    public void InputLock()
    {
        this.MouseAvailable = false;
    }

    public void InputUnLock()
    {
        this.MouseAvailable = true;
    }

    public void InputUnLockOnly()
    {
        foreach(PlayerMouse set in PlayerMice)
        {
            set.InputLock();
        }
        this.InputUnLock();
    }

    public static void InputLockAll(bool isLock)
    {
        foreach(PlayerMouse set in PlayerMice)
        {
            if (isLock) set.InputLock();
            else set.InputUnLock();
        }
    }
}

[Serializable]
public class KeyboardInput
{
    public static KeyboardInput EmotionToggle = new KeyboardInput("EmotionToggle", PlayerKeyboard.KeyboardInputName.T, PlayerKeyboard.KeyboardInputType.KeyDown);
    public static KeyboardInput Emotion1 = new KeyboardInput("Emotion1", PlayerKeyboard.KeyboardInputName.Alpha1, PlayerKeyboard.KeyboardInputType.KeyDown);
    public static KeyboardInput Emotion2 = new KeyboardInput("Emotion2", PlayerKeyboard.KeyboardInputName.Alpha2, PlayerKeyboard.KeyboardInputType.KeyDown);
    public static KeyboardInput Emotion3 = new KeyboardInput("Emotion3", PlayerKeyboard.KeyboardInputName.Alpha3, PlayerKeyboard.KeyboardInputType.KeyDown);
    public static KeyboardInput Emotion4 = new KeyboardInput("Emotion4", PlayerKeyboard.KeyboardInputName.Alpha4, PlayerKeyboard.KeyboardInputType.KeyDown);

    public static KeyboardInput CameraViewChange = new KeyboardInput("CameraViewChange", PlayerKeyboard.KeyboardInputName.G, PlayerKeyboard.KeyboardInputType.KeyDown);
    

    public KeyboardInput(string name, PlayerKeyboard.KeyboardInputName keyName, PlayerKeyboard.KeyboardInputType keyType)
    {
        this.inputName = name;
        if (Enum.IsDefined(typeof(KeyCode), inputKeyName.ToString()))
        {
            this.inputKeyName = keyName;
            this.inputKeyType = keyType;
        }
    }

    public KeyCode ToKeyCode()
    {
        return (KeyCode)Enum.Parse(typeof(KeyCode), inputKeyName.ToString());
    }

    public string inputName;
    public PlayerKeyboard.KeyboardInputName inputKeyName;
    public PlayerKeyboard.KeyboardInputType inputKeyType;
}

[Serializable]
public class MouseInput
{
    public static MouseInput CameraDrag = new MouseInput("CameraDrag", PlayerMouse.MouseInputName.Right, PlayerMouse.MouseInputType.MouseDown);
    public static MouseInput CameraDragExit = new MouseInput(CameraDrag, PlayerMouse.MouseInputType.MouseUp);

    public MouseInput(string name, PlayerMouse.MouseInputName mouseName, PlayerMouse.MouseInputType mouseType)
    {
        this.inputName = name;
        this.inputMouseName = mouseName;
        this.inputMouseType = mouseType;
    }

    public MouseInput(MouseInput origin, PlayerMouse.MouseInputType type)
    {
        this.inputName = origin.inputName;
        this.inputMouseName = origin.inputMouseName;
        this.inputMouseType = type;
    }

    public string inputName;
    public PlayerMouse.MouseInputName inputMouseName;
    public PlayerMouse.MouseInputType inputMouseType;
}
