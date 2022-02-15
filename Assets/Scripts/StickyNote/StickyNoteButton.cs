using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StickyNoteButton : MonoBehaviour
{
    public GameObject stickyNotePrefab;
    private GameObject _player;
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }
    public void CreateStickyNote()
    {
        Vector3 mainCamRotation = Camera.main.transform.rotation.eulerAngles;
        Vector3 forwardRotation = new Vector3(0, mainCamRotation.y, 0);
        //if(StickyNoteNetworkManager.Instance.networked)
            PhotonNetwork.Instantiate(stickyNotePrefab.name, _player.transform.position, Quaternion.Euler(forwardRotation));
        //else
        //    Instantiate(stickyNotePrefab, Vector3.zero, Quaternion.identity);
    }
}
