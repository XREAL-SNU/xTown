using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEventHandler : MonoBehaviourPunCallbacks
{
    public Action<PlayerInfo> OnPlayerJoinedHandler = null;
    public Action<PlayerInfo> OnPlayerLeftHandler = null;


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerInfo playerInfo = new PlayerInfo();
        playerInfo.PlayerName = newPlayer.NickName;
        playerInfo.ActorNr = newPlayer.ActorNumber;
        Debug.Log("RoomManager/ new player #" + playerInfo.ActorNr + " joined, name:" + playerInfo.PlayerName);

        if (OnPlayerJoinedHandler != null)
        {
            OnPlayerJoinedHandler.Invoke(playerInfo);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerInfo playerInfo = new PlayerInfo();
        playerInfo.PlayerName = otherPlayer.NickName;
        playerInfo.ActorNr = otherPlayer.ActorNumber;
        Debug.Log("RoomManager/ player left, name: " + playerInfo.PlayerName);

        if (OnPlayerLeftHandler != null)
        {
            OnPlayerLeftHandler.Invoke(playerInfo);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // to be implemented
    }
}
