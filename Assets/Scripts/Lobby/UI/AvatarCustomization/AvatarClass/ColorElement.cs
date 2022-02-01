using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorElement
{
    //private(property value)
    private Dictionary<string,Color> _myAvatarColor;
    private Dictionary<string, Color> _myButtonColor;

    //property
    public Dictionary<string, Color> AvatarColorP { get { return _myAvatarColor; } }
    public Dictionary<string, Color> ButtonColorP { get { return _myButtonColor; } }

    public ColorElement()
    {
        if (_myAvatarColor == null)
        {
            _myAvatarColor.Add("white", Color.white);
            _myAvatarColor.Add("red", Color.red);
            _myAvatarColor.Add("yellow", Color.yellow);
            _myAvatarColor.Add("green", Color.green);
            _myAvatarColor.Add("cyan", Color.cyan);
            _myAvatarColor.Add("magenta", Color.magenta);
            _myAvatarColor.Add("gray", Color.gray);
        }
        if (_myButtonColor == null)
        {
            _myButtonColor.Add("base", Color.white);
            _myButtonColor.Add("Enter", Color.cyan);
            _myButtonColor.Add("Slect", Color.green);
            _myButtonColor.Add("Slect2", new Color(255 / 255, 255 / 255, 255 / 255, 100 / 255f));
        }
    }
}