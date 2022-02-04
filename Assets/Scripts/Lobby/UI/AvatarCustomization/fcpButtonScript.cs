using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fcpButtonScript : MonoBehaviour
{
    public bool IsEnter = false;
    public Color FcpColor;
    private GameObject fcp;

    private void Start()
    {
        fcp = this.transform.parent.parent.parent.GetChild(1).gameObject;
    }

    public void FcpOn()
    {
        IsEnter = !IsEnter;
        fcp.SetActive(IsEnter);
    }
}
