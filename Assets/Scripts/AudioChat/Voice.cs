using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XReal.XTown.VoiceChat
{
    public class Voice : MonoBehaviour
    {
        static Voice _voiceChat;
        public static Voice VoiceChat
        {
            get => _voiceChat;
        }


        void Awake()
        {
            if (_voiceChat == null)
            {
                _voiceChat = this;
            }
        }

        public bool IsVoiceOn = true;

        Recorder _recorder;
        PhotonVoiceNetwork _network;
        AudioChatSettings _settings;
        public PlayerVoice MyVoice
        {
            get
            {
                if (!PhotonNetwork.InRoom) return null;
                //PlayerVoice voice = RoomManager.Room.GetComponentInPlayerById<PlayerVoice>(PhotonNetwork.LocalPlayer.ActorNumber);
                PlayerVoice voice = PlayerManager.Players.LocalPlayerGo.GetComponent<PlayerVoice>();
                return voice;
            }
        }

        private void Start()
        {
            _network = GetComponent<PhotonVoiceNetwork>();
            _settings = GetComponent<AudioChatSettings>();
            _recorder = _network.PrimaryRecorder;

            Init();
        }

        private void Init()
        {
            Debug.Log("Voice/ INIT!");
            if (!_settings.EnableVoiceOnJoin)
            {
                StartCoroutine("PauseVoiceOnJoinCoroutine");
            }
        }

        // stop voice on join if setting says so.
        IEnumerator PauseVoiceOnJoinCoroutine()
        {
            while (!_recorder.IsRecording)
            {
                Debug.Log("Voice/ Inside coroutine, waiting for init");
                yield return null;
            }
            PauseVoice();
            Debug.Log("Voice/ paused.Coroutine returns.");
        }

        // button callbacks
        public void StartVoice()
        {
            Debug.Log("Voice/ starting voice");

            IsVoiceOn = true;
            _recorder.StartRecording();
            MyVoice.SetVoiceState(true);
        }

        public void PauseVoice()
        {
            Debug.Log("Voice/ pausing voice");

            IsVoiceOn = false;
            _recorder.StopRecording();
            MyVoice.SetVoiceState(false);
        }


        // events
        public Action<int, bool> OnPlayerVoiceChangedHandler = null;

        public void OnPlayerVoiceChanged(int actorNr, bool state)
        {
            if (OnPlayerVoiceChangedHandler != null)
            {
                OnPlayerVoiceChangedHandler.Invoke(actorNr, state);
            }
        }

        public void RemoveListener(Action<int, bool> action)
        {
            if(OnPlayerVoiceChangedHandler != null)
            {
                OnPlayerVoiceChangedHandler -= action;
            }
        }

        public void AddListener(Action<int, bool> action)
        {
            OnPlayerVoiceChangedHandler -= action;
            OnPlayerVoiceChangedHandler += action;
        }
    }
}
