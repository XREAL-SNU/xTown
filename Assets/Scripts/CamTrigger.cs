using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTrigger : MonoBehaviour
{
    [Header("Camera")]
    public GameObject camObject;

    private CamCapture camScript;
    void Start()
    {
        camScript = camObject.GetComponent<CamCapture>();
    }
    
    private void OnTriggerEnter(Collider col)
    {
        camScript.setReady();
    }

    private void OnTriggerExit(Collider col)
    {
        camScript.resetReady();
    }
}
