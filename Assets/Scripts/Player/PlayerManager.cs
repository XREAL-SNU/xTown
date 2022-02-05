using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    static PlayerManager _playerManager;

    public static PlayerManager Players
    {
        get => _playerManager;
    }
    void Awake()
    {
        if (_playerManager == null)
        {
            _playerManager = this;
        }
        else if (_playerManager != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        CreateLocalPlayerInfo();
    }


    // properties
    public PlayerInfo _playerInfo;
    

    void CreateLocalPlayerInfo()
    {
        _playerInfo = new PlayerInfo();

    }

    // events
    public Action OnJoinedRoomHandler = null;
    public Action OnLeftRoomHandler = null;

    public override void OnJoinedRoom()
    {
        if(OnJoinedRoomHandler!= null)
        {
            OnJoinedRoomHandler.Invoke();
        }
    }

    public override void OnLeftRoom()
    {
        if (OnLeftRoomHandler != null)
        {
            OnLeftRoomHandler.Invoke();
        }
    }
}
