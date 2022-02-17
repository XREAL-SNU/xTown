using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class WhiteboardEdit : MonoBehaviour
{
    public WhiteboardMarker red;
    public WhiteboardMarker blue;
    public WhiteboardMarker green;
    public WhiteboardMarker yellow;
    public WhiteboardMarker black;
    public WhiteboardMarker_erase eraser;

    public void OnClick_Edit()
    {
        red.TakeOwnerShip();
        blue.TakeOwnerShip();
        green.TakeOwnerShip();
        yellow.TakeOwnerShip();
        black.TakeOwnerShip();
        eraser.TakeOwnerShip();
    }
}
