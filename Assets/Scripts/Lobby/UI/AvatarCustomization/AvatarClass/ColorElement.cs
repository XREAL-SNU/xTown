using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorElement
{
    //private(property value)
    private Dictionary<string, Color> _myAvatarColor;
    private Dictionary<string, Color> _myButtonColor;

    //property
    public Dictionary<string, Color> AvatarColorP { get { return _myAvatarColor; } }
    public Dictionary<string, Color> ButtonColorP { get { return _myButtonColor; } }

    public ColorElement()
    {
        if (_myAvatarColor is null)
        {
            _myAvatarColor = new Dictionary<string, Color>();
            _myAvatarColor.Add("White", Color.white);
            _myAvatarColor.Add("Red", Color.red);
            _myAvatarColor.Add("Yellow", Color.yellow);
            _myAvatarColor.Add("Green", Color.green);
            _myAvatarColor.Add("Cyan", Color.cyan);
            _myAvatarColor.Add("Magenta", Color.magenta);
            _myAvatarColor.Add("Gray", Color.gray);
        }
        if (_myButtonColor is null)
        {
            _myButtonColor = new Dictionary<string, Color>();
            _myButtonColor.Add("Base", Color.white);
            _myButtonColor.Add("Enter", Color.cyan);
            _myButtonColor.Add("Select", Color.green);
            _myButtonColor.Add("Select2", new Color(255 / 255, 255 / 255, 255 / 255, 100 / 255f));
        }
    }
}