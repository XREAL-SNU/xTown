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
            _playerInfo = new PlayerInfo();

        }
        else if (_playerManager != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // local avatar gameObject and its appreanance bind on set.

    GameObject _localPlayerGo;
    AvatarAppearanceNew _appearance;
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

    // properties
    public PlayerInfo _playerInfo;
    


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
