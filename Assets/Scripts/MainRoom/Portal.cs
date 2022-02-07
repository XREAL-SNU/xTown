using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Portal : MonoBehaviour
{
    public string SceneName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerPrefs.SetString("PastScene", "MainRoom");
            PhotonNetwork.LeaveRoom();
            RoomsCanvases.Instance.CreateOrJoinRoomCanvas.Show();
            RoomsCanvases.Instance.CreateOrJoinRoomCanvas.LinkedSceneName = SceneName;
        }
    }
}
