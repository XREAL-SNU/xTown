using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTrigger : MonoBehaviour
{
    [Header("Camera")]
    public GameObject camObject;
    
    //reference to the camera's 'CamCapture' script
    private CamCapture camScript;

    void Start()
    {
        camScript = camObject.GetComponent<CamCapture>();
    }
    
    //trigger callbacks notifies cam that someone's in the region
    //!!could be implemented by observer pattern!!
    private void OnTriggerEnter(Collider col)
    {
        camScript.setReady();
    }

    private void OnTriggerExit(Collider col)
    {
        camScript.resetReady();
    }
}
