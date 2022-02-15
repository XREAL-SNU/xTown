using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class StickyNoteNetworkManager : MonoBehaviourPunCallbacks
{
    public static StickyNoteNetworkManager Instance { get; private set; }
    private StickyNoteNetworkManager() { }
    public bool networked = false;
    void Awake()
    {
        Debug.Log("StickyNetworkManger/Awake");
        Instance = this;
    }
    private void Start()
    {
        networked = PhotonNetwork.IsConnected;
        Debug.Log(networked ? "----networked----" : "----not networked----");
        if (networked)
        {
            //TurnListener = GetComponent<StickyNoteManager>();
            //Destroy(GameObject.Find("MugCup"));
            Debug.Log("StickyNoteManager/Start : Destroy?");
        }
        else
        {
            Debug.LogWarning("NOT CONNECTED TO NETWORK: singleplay mode");
            return;
        }

    }
}
