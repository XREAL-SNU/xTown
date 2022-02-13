using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JK
{
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
            rectTran = gameObject.GetComponent<RectTransform>();
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bar_width);
        }
    }
}