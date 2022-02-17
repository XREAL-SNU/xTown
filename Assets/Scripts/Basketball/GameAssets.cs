using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace XReal.XTown.Basketball
{
    public class GameAssets : MonoBehaviour
    {
        private static GameAssets _i;

        public static GameAssets i
        {
            get
            {
                if (_i == null)
                {
                    _i = (Instantiate(Resources.Load("Basketball/GameAssets")) as GameObject).GetComponent<GameAssets>();
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
}

