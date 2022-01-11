using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickerGamePortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player play ClickerGame");
            PlayerPrefs.SetString("PastScene", "MainRoom");
            SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }
    }
}
