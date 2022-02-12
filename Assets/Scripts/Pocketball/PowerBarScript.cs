using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JK
{
    public class PowerBarScript : MonoBehaviour
    {
        public static float bar_width;
        RectTransform rectTran;
        void Start()
        {
            rectTran = gameObject.GetComponent<RectTransform>();
        }
        void Update()
        {
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bar_width);
        }
    }
}