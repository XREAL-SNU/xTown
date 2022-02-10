using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerInfo
{
    public enum PlayerInfoField
    {
        PlayerName, ActorNr
    }

    public string PlayerName;
    public int ActorNr;
}
