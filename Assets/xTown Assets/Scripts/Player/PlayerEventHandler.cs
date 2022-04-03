using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventHandler : MonoBehaviourPunCallbacks
{

    

    // events
    public Action OnJoinedRoomHandler = null;
    public Action OnLeftRoomHandler = null;

    public override void OnJoinedRoom()
    {
        if (OnJoinedRoomHandler != null)
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
