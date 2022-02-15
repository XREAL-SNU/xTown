using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace XReal.XTown.VoiceChat{
    public class PlayerVoice : MonoBehaviour
    {
        PhotonVoiceView _voiceView;
        PhotonView _view;
        Recorder _recorder;
        Speaker _speaker;
        AudioSource _audioSource;

        public bool IsVoiceOn;


        void Start()
        {
            InitVoice();
            RoomManager.BindEvent(gameObject, OnPlayerJoined_SetPlayerVoice, RoomManager.RoomEvent.PlayerJoined);
        }

        void InitVoice()
        {
            _voiceView = GetComponent<PhotonVoiceView>();

            if (!_voiceView.isActiveAndEnabled)
            {
                Debug.LogError("PlayerVoice/ VoiceView component not active at Start");
                return;
            }
            _speaker = _voiceView.SpeakerInUse;
            _audioSource = GetComponentInChildren<AudioSource>();
            _recorder = _voiceView.RecorderInUse;

            _view = GetComponent<PhotonView>();

        }

        // controls the audio source, not speaker playback.
        public bool AudioSourceMuted
        {
            get => _audioSource.mute;
            set
            {

                Debug.Log($"audio muted: {_view.Owner.NickName}");
                _audioSource.mute = value;
            }
        }

        // controls the speaker playback directly.
        // this may be less resource consuming in the long term,
        // but incurs a small overhead when value changes.
        public bool PlaybackEnabled
        {
            // PlaybackStarted is true if speaker is initialized and not stopped.
            get => _speaker.PlaybackStarted;
            set  {
                if (value && PlaybackEnabled)
                {
                    _speaker.StopPlayback();
                }
                else if(!value)
                {
                    // maybe check for PlaybackStarted? 
                    _speaker.RestartPlayback();
                }
            }
        }

        // Room events
        public void OnPlayerJoined_SetPlayerVoice(PlayerInfo info)
        {
            Debug.Log($"PlayerVoice/ On player {info.PlayerName} joined");
            // broadcast my state to the new joined actor. specify actor target~!
            SetVoiceState(IsVoiceOn, info.ActorNr);
        }
        // Netcode
        public void SetVoiceState(bool state, int actorNr = -1)
        {
            IsVoiceOn = state;
            // _view may be null depending on timing of call
            if (_view is null) _view = GetComponent<PhotonView>();
            if (_view.IsMine) _view.RPC("SyncVoiceStateRPC", RpcTarget.Others, state, actorNr);
        }

        [PunRPC]
        public void SyncVoiceStateRPC(bool state, int actorNr)
        {
            // if specified, return if not mine.
            if(actorNr > 0 && PhotonNetwork.LocalPlayer.ActorNumber != actorNr)
            {
                return;
            }
            Debug.Log("SyncPlayerVoice RPC " + state);
            IsVoiceOn = state;
        }


    }


}
