using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JK
{
public class PocketDyeNetworkManager : MonoBehaviourPunCallbacks
{
    
    public static PocketDyeNetworkManager Instance { get; private set; }
    private PocketDyeNetworkManager() { }
    public bool networked = false;
    public GameManager _pocketGameManager;
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
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("OnJoinedRoom/ alone in the room.");
            }
            else if(PhotonNetwork.CurrentRoom.PlayerCount == 2 )
            {
                _pocketGameManager._view.RPC("GameStart",RpcTarget.All);
                Debug.Log("GameStart");
            }
        }
        else
        {
            Debug.LogWarning("NOT CONNECTED TO NETWORK: singleplay mode");
            return;
        }

    }
    public void OnClick_Exit()
    {
        Debug.Log("PocketDyeLeave / Button Clicked!");
        PlayerPrefs.SetString("prevScene", "PocketDye");
        TestConnect.Instance._isLeavingPortal = true;
        PhotonNetwork.LeaveRoom();
    }
}
}
