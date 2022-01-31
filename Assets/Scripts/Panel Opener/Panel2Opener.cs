using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Panel2Opener : MonoBehaviour
{

    // (1) Staic varialbe case
    //public static float timeValue = 10;
    // (2) GetComponent ccase
    public float timeValue = 10;

    public GameObject Panel2;

    public void OpenPanel()
    {
        if (Panel2 != null)
        {
              bool isActive=Panel2.activeSelf;
              Panel2.SetActive(!isActive);
        }
    }

    void Update()
    {
      if (timeValue>0)
      {
          timeValue-=Time.deltaTime;
      }

      else
      {
          timeValue = 0;
      }
    }
}
