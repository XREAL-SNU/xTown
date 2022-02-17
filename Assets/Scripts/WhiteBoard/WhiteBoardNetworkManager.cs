using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WhiteBoardNetworkManager : MonoBehaviourPunCallbacks
{
    public static WhiteBoardNetworkManager Instance { get; private set; }
    private WhiteBoardNetworkManager() { }
    public bool networked = false;
    void Awake()
    {
        Debug.Log("WhiteBoardNetworkManager/Awake");
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
            Debug.Log("WhiteBoardManager/Start : Destroy?");
        }
        else
        {
            Debug.LogWarning("NOT CONNECTED TO NETWORK: singleplay mode");
            return;
        }

    }
}
