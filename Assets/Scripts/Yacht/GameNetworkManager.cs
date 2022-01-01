using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameNetworkManager : MonoBehaviourPunCallbacks
{



    /* Pun callbacks */

    public override void OnLeftRoom()
    {
        Debug.Log("Yacht/GameNetworkManager: leaving room");
        SceneManager.LoadScene("Playground");
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.Log("new player joined: " + PhotonNetwork.CurrentRoom.PlayerCount + " in room");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Yacht/GameNetworkManager: master ActorNum = " + PhotonNetwork.LocalPlayer.ActorNumber);
            LoadGame(); // so other players are looking at the same scene
        }
        else
        {
            Debug.Log("Yacht/GameNetworkManager: master ActorNum = " + other.ActorNumber);
        }

    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log("Yacht/GameNetworkManager: Other player left room: leaving room");
        LeaveRoom();
    }

    /* UI callbacks */
    public void OnClick_LeaveRoom()
    {
        Debug.Log("Yacht/GameNetworkManager: Leaving room by player input");
        LeaveRoom();
    }



    /* private methods */
    private void LoadGame()
    {
        if (PhotonNetwork.IsMasterClient)
        Debug.Log("Yacht/GameNetworkManager: Loading Yacht game");
        PhotonNetwork.LoadLevel("Yacht");
    }

    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
