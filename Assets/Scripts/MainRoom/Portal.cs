using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string SceneName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerPrefs.SetString("PastScene", "MainRoom");
            RoomsCanvases.Instance.CreateOrJoinRoomCanvas.Show();
            RoomsCanvases.Instance.CreateOrJoinRoomCanvas.LinkedSceneName = SceneName;
        }
    }
}
