using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
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
            if (!_settings.EnableVoiceOnJoin)
            {
                PauseVoice();
            }
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

    }
}
