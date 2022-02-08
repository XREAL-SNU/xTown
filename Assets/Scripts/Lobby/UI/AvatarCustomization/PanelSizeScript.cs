using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSizeScript : MonoBehaviour
{
    private void Awake()
    {
        int panelHeight = (int)(this.transform.childCount/4) * 175 + 200;
        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(684, panelHeight);
    }
}
