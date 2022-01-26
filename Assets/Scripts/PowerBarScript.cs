using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBarScript : MonoBehaviour
{
    public static float bar_width;
    RectTransform rectTran;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform rectTran = gameObject.GetComponent<RectTransform>();
        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bar_width);
    }
}
