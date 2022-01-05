using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WhiteboardPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player use Whiteboard");
            SceneManager.LoadScene("Whiteboard", LoadSceneMode.Single);
        }
    }
}
