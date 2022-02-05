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
        if (!newPlayer.CustomProperties.ContainsKey(PlayerInfo.PlayerInfoField.PlayerName.ToString()))
        {
            Debug.LogError("RoomManager/ Player joined but cannot get its name");
            return;
        }
        PlayerInfo playerInfo = new PlayerInfo();
        playerInfo.PlayerName = (string)newPlayer.CustomProperties[PlayerInfo.PlayerInfoField.PlayerName.ToString()];
        Debug.Log("RoomManager/ new player joined, name:" + playerInfo.PlayerName);

        if (OnPlayerJoinedHandler != null)
        {
            OnPlayerJoinedHandler.Invoke(playerInfo);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerInfo playerInfo = new PlayerInfo();
        playerInfo.PlayerName = (string)otherPlayer.CustomProperties[PlayerInfo.PlayerInfoField.PlayerName.ToString()];
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
