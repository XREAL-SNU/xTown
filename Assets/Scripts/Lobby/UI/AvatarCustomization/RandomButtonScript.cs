using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomButtonScript : MonoBehaviour
{
    public void RandomCustomizing()
    {
        GameObject contents = this.transform.parent.GetChild(0).GetChild(0).gameObject;
        int ran_col = Random.Range(0, contents.transform.GetChild(0).childCount - 1);
        int ran_tex = Random.Range(0, contents.transform.GetChild(1).childCount);

        contents.transform.GetChild(0).GetChild(ran_col).GetComponent<CustomizingButtonScript>().IsClickButton();
        contents.transform.GetChild(0).GetChild(ran_col).GetComponent<CustomizingButtonScript>().Select();
        if (contents.transform.GetChild(0).GetChild(7).GetComponent<fcpButtonScript>().IsEnter)
        {
            this.transform.GetChild(0).GetChild(7).GetComponent<fcpButtonScript>().FcpOn();
        }

        contents.transform.GetChild(1).GetChild(ran_tex).GetComponent<CustomizingButtonScript>().IsClickButton();
        contents.transform.GetChild(1).GetChild(ran_tex).GetComponent<CustomizingButtonScript>().Select();

    }
}
