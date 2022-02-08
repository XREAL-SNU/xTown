using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballAssets : MonoBehaviour
{
    private static BasketballAssets _i;
    
    public static BasketballAssets i
    {
        get
        {
            if (_i == null)
            {
                _i = (Instantiate(Resources.Load("Basketball/BasketballAssets")) as GameObject).GetComponent<BasketballAssets>();
            }
            return _i;
        }
    }

    public SoundAudioClip[] soundAudioClipArray;


    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
}
