using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XReal.XTown.UI;

public class VideoChatUITest : MonoBehaviour
{
    void OnGUI()
    {

        if (GUI.Button(new Rect(10, 10, 50, 50), "OpenCanvas"))
            UIManager.UI.ShowPopupUI<PagingUI>("VideoChat/VideoChatCanvas");


    }
}
