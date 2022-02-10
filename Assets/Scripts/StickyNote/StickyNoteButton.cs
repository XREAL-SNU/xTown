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
        
        //if(StickyNoteNetworkManager.Instance.networked)
            PhotonNetwork.Instantiate(stickyNotePrefab.name, _player.transform.position, Quaternion.identity);
        //else
        //    Instantiate(stickyNotePrefab, Vector3.zero, Quaternion.identity);
    }
}
