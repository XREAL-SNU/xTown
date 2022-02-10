using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButtonScript : MonoBehaviour
{
    public void ResetCustomizing()
    {
        CustomizingButtonScript cbtn = this.transform.parent.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<CustomizingButtonScript>();
        CustomizingButtonScript tbtn = this.transform.parent.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<CustomizingButtonScript>();

        cbtn.IsClickButton();
        cbtn.Select();
        tbtn.IsClickButton();
        tbtn.Select();
        if (cbtn.transform.parent.GetChild(7).GetComponent<fcpButtonScript>().IsEnter)
        {
            cbtn.transform.parent.GetChild(7).GetComponent<fcpButtonScript>().FcpOn();
        }
    }
}
