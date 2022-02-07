using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    static RoomManager _room;

    public enum RoomEvent
    {
        PlayerJoined,
        PlayerLeft,
        PlayerPropertiesUpdate
    }
    public static RoomManager Room
    {
        get => _room;
    }

    private void Awake()
    {
        if (_room is null) _room = this;
    }

    public List<PlayerInfo> GetPlayerInfoList()
    {

        List<PlayerInfo> playerInfos = new List<PlayerInfo>();
        if (!PhotonNetwork.InRoom)
        { // if not in room, just return empty.
            return playerInfos;
        }
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.PlayerName = PhotonNetwork.LocalPlayer.NickName;
            //playerInfo.PlayerName = (string)player.CustomProperties[PlayerInfo.PlayerInfoField.PlayerName.ToString()];
            playerInfos.Add(playerInfo);
        }
        Debug.Log($"GetPlayerInfoList returning {playerInfos.Count} players");
        return playerInfos;
    }

    // event binding
    public static void BindEvent(GameObject go, Action<PlayerInfo> action, RoomEvent evtype)
    {
        RoomEventHandler evt = go.GetComponent<RoomEventHandler>();
        if (evt is null)
        {
            evt = go.AddComponent<RoomEventHandler>();
        }

        switch (evtype)
        {
            case RoomEvent.PlayerJoined:
                evt.OnPlayerJoinedHandler += action;
                break;
            case RoomEvent.PlayerLeft:
                evt.OnPlayerLeftHandler += action;
                break;
        }
    }
}
