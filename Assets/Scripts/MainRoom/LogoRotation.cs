using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoRotation : MonoBehaviour
{
    public GameObject logoRotatePivot;
    void FixedUpdate()
    {
        transform.RotateAround(logoRotatePivot.transform.position,new Vector3(0,1,0),1);
    }
}
