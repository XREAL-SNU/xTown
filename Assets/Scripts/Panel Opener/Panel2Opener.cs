using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel2Opener : MonoBehaviour
{
    public GameObject Panel2;

    public void OpenPanel()
    {
        if (Panel2 != null)
        {
              bool isActive=Panel2.activeSelf;
              Panel2.SetActive(!isActive);
        }
    }

}
