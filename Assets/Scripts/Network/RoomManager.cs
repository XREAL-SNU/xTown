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

    // getters utility
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
            playerInfo.PlayerName = player.NickName;
            playerInfo.ActorNr = player.ActorNumber;
            //playerInfo.PlayerName = (string)player.CustomProperties[PlayerInfo.PlayerInfoField.PlayerName.ToString()];
            playerInfos.Add(playerInfo);
        }
        Debug.Log($"GetPlayerInfoList returning {playerInfos.Count} players");
        return playerInfos;
    }

    public List<string> GetPlayerNameList()
    {

        List<string> playerInfos = new List<string>();
        if (!PhotonNetwork.InRoom)
        { // if not in room, just return empty.
            return playerInfos;
        }
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            string playerInfo = player.NickName;
            //playerInfo.PlayerName = (string)player.CustomProperties[PlayerInfo.PlayerInfoField.PlayerName.ToString()];
            playerInfos.Add(playerInfo);
        }
        Debug.Log($"GetPlayerInfoList returning {playerInfos.Count} players");
        return playerInfos;
    }

    // key is photonView Id in room, which may be reused.
    Dictionary<int, GameObject> _playerObjects = new Dictionary<int, GameObject>();
    public T GetComponentInPlayerById<T>(int id) where T : UnityEngine.Object
    {
        GameObject playerGo;
        if (!PhotonNetwork.InRoom) return null;

        if(!_playerObjects.TryGetValue(id, out playerGo))
        {
            Debug.LogError("RoomManager/ could not fetch player gameobject with Actor Id:" + id);
            return null;
        }

        if(typeof(T) == typeof(GameObject))
        {
            // return the player gameObject
            return playerGo as T;
        }
        else
        {
            // return a specific component
            T component = playerGo.GetComponentInChildren<T>();
            if(component is null)
            {
                Debug.LogError("RoomManager/ could not fetch component in player: " + typeof(T));
            }
            return component;

        }
    }

    public void AddPlayerGameObject(int id, GameObject go)
    {
        if (!PhotonNetwork.InRoom)
        {
            return;
        }
        
        if (_playerObjects.ContainsKey(id))
        {
            // overrides existing element
            _playerObjects[id] = go;
        }
        else
        {
            _playerObjects.Add(id, go);
            Debug.Log("RoomManager/ Added PlayerGO to dict: #" + _playerObjects.Count);
        }
    }

    // Room scale event binders
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
                evt.OnPlayerJoinedHandler -= action;
                evt.OnPlayerJoinedHandler += action;
                break;
            case RoomEvent.PlayerLeft:
                evt.OnPlayerLeftHandler -= action;
                evt.OnPlayerLeftHandler += action;
                break;
        }
    }
}
