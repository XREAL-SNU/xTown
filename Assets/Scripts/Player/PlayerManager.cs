using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    // singleton, dont destroy on load
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
            PlayerInfo = new PlayerInfo();
            
        }
        else if (_playerManager != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // local avatar gameObject and its properties

    GameObject _localPlayerGo;
    AvatarAppearanceNew _appearance;
    public PlayerInfo PlayerInfo;

    public GameObject LocalPlayerGo
    {
        get => _localPlayerGo;
        set
        {
            if(value is null)
            {
                Debug.Log("PlayerManager/trying to set null value to LocalPlayerGo");
            }
            _localPlayerGo = value;
            if(_appearance is null)
            {
                _appearance = new AvatarAppearanceNew(AvatarAppearanceNew.XRealSpaceSuitAppearanceDescriptor, _localPlayerGo);
            }
            else
            {
                _appearance.Apply(_localPlayerGo);
            }
        }
    }

    public AvatarAppearanceNew LocalAvatarAppearance
    {

        get
        {
            if(_appearance is null)
            {
                Debug.LogError("PlayerManager/ LocalAvatarAppearence is null");
            }
            return _appearance;
        }
    }



    // event handler

    public enum PlayerEvent
    {
        JoinedRoom,
        LeftRoom
    }
    public static void BindEvent(GameObject go, Action action, PlayerEvent evtype)
    {
        PlayerEventHandler evt = go.GetComponent<PlayerEventHandler>();
        if (evt is null)
        {
            evt = go.AddComponent<PlayerEventHandler>();
        }

        switch (evtype)
        {
            case PlayerEvent.JoinedRoom:
                evt.OnJoinedRoomHandler += action;
                break;
            case PlayerEvent.LeftRoom:
                evt.OnLeftRoomHandler += action;
                break;
        }
    }

    public override void OnJoinedRoom()
    {
        PlayerInfo.ActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
        PlayerInfo.PlayerName = PhotonNetwork.LocalPlayer.NickName;
    }


}
