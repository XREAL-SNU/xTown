using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkDebugger : MonoBehaviour
{
    [SerializeField]
    private Text debugText;
    // Start is called before the first frame update
    void Start()
    {
        debugText.text = "how can this possibly not work?";
    }

    /* public methods */
    public void NetworkDebug(string s)
    {
        Debug.Log("debug:" + s);
        debugText.text = s.ToString();
    }
}
