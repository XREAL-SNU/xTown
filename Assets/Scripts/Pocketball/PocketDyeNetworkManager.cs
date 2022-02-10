using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PocketDyeNetworkManager : MonoBehaviourPunCallbacks
{
    public static PocketDyeNetworkManager Instance { get; private set; }
    private PocketDyeNetworkManager() { }
    public bool networked = false;
    void Awake()
    {
        Debug.Log("PocketDyeNetworkManger/Awake");
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
            Debug.Log("PhotonDyeNetworkManager/Start : Destroy?");
        }
        else
        {
            Debug.LogWarning("NOT CONNECTED TO NETWORK: singleplay mode");
            return;
        }

    }
}
