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

    public static bool KeyboardAvailable = true;
    public string KeyboardSetName;
    public KeyboardInput[] KeyboardInputs;

    // lock 제한을 받을 input set
    public static KeyboardInput[] LockKeyboardInputSet = new KeyboardInput[]
    {
        KeyboardInput.EmotionToggle, KeyboardInput.Emotion1, KeyboardInput.Emotion2, KeyboardInput.Emotion3, KeyboardInput.Emotion4,
        KeyboardInput.CameraViewChange
    };

    // 어떤 상황에서든 항상 구현되어야 할 input set
    public static KeyboardInput[] UnLockKeyboardInputSet = new KeyboardInput[]
    {

    };

    public static PlayerKeyboard DefaultPlayerKeyboard = new PlayerKeyboard("LockInput");


    public PlayerKeyboard(string name)
    {
        KeyboardSetName = name;
        KeyboardInputs = LockKeyboardInputSet;
    }

    public PlayerKeyboard(string name, KeyboardInput[] keySet)
    {
        KeyboardSetName = name;
        KeyboardInputs = keySet;
    }


    public static bool KeyboardInputSet(KeyboardInput input)
    {
        foreach(KeyboardInput key in DefaultPlayerKeyboard.KeyboardInputs)
        {
            if (input.Equals(key))
            {
                switch (input.inputKeyType)
                {
                    case KeyboardInputType.Key:
                        return Input.GetKey(input.ToKeyCode()) && KeyboardAvailable;
                    case KeyboardInputType.KeyDown:
                        return Input.GetKeyDown(input.ToKeyCode()) && KeyboardAvailable;
                    case KeyboardInputType.KeyUp:
                        return Input.GetKeyUp(input.ToKeyCode()) && KeyboardAvailable;
                }
            }
        }
        Debug.LogError("KeyboardInput Error");
        return false;
    }

    public static void PlayerInputLock()
    {
        KeyboardAvailable = false;
    }

    public static void PlayerInputUnLock()
    {
        KeyboardAvailable = true;
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

    public static bool MouseAvailable = true;
    public string MouseSetName;
    public MouseInput[] MouseInputs;

    public static MouseInput[] LockMouseInputSet = new MouseInput[]
    {
        MouseInput.CameraDrag, MouseInput.CameraDragExit
    };

    public static MouseInput[] UnLockMouseInputSet = new MouseInput[]
    {

    };

    public static PlayerMouse DefaultPlayerMouse = new PlayerMouse("LockInput");


    public PlayerMouse(string name)
    {
        MouseSetName = name;
        MouseInputs = LockMouseInputSet;
    }

    public PlayerMouse(string name, MouseInput[] keySet)
    {
        MouseSetName = name;
        MouseInputs = keySet;
    }


    public static bool MouseInputSet(MouseInput input)
    {
        foreach (MouseInput key in DefaultPlayerMouse.MouseInputs)
        {
            if (input.Equals(key))
            {
                switch (input.inputMouseType)
                {
                    case MouseInputType.Mouse:
                        return Input.GetMouseButton((int)input.inputMouseName) && MouseAvailable;
                    case MouseInputType.MouseDown:
                        return Input.GetMouseButtonDown((int)input.inputMouseName) && MouseAvailable;
                    case MouseInputType.MouseUp:
                        return Input.GetMouseButtonUp((int)input.inputMouseName) && MouseAvailable;
                }
            }
        }
        Debug.LogError("MouseInput Error");
        return false;
    }

    public static void PlayerInputLock()
    {
        MouseAvailable = false;
    }

    public static void PlayerInputUnLock()
    {
        MouseAvailable = true;
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
