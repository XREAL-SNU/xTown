using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetCustomization : MonoBehaviour
{
    public void OnClick_Red()
    {
        PlayerAvatar.LocalPlayerAvatar.ChangeMaterialColor();
    }
}
