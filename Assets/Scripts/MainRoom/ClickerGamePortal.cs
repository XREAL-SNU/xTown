using Photon.Pun;
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
            PhotonNetwork.LeaveRoom();
            PlayerPrefs.SetString("PastScene", "MainRoom");
            RoomsCanvases.Instance.gameObject.SetActive(true);
            RoomsCanvases.Instance.CreateOrJoinRoomCanvas.Show();
            RoomsCanvases.Instance.CreateOrJoinRoomCanvas.LinkedSceneName = "GameScene";
        }
    }
}
