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

        Recorder _recorder;
        Speaker _speaker;
        AudioSource _audioSource;

        AudioChatSettings _settings;

        // editor
        [SerializeField]
        private bool _isMine = false;

        void Start()
        {


            InitVoice();

        }

        void InitVoice()
        {
            _voiceView = GetComponentInParent<PhotonVoiceView>();

            if (!_voiceView.isActiveAndEnabled)
            {
                Debug.LogError("PlayerVoice/ VoiceView component not active at Start");
                return;
            }
            _speaker = _voiceView.SpeakerInUse;
            _audioSource = GetComponent<AudioSource>();
            _recorder = _voiceView.RecorderInUse;

            PhotonView view = GetComponentInParent<PhotonView>();
            _isMine = view.IsMine;
        }

        // controls the audio source, not speaker playback.
        public bool AudioSourceMuted
        {
            get => _audioSource.mute;
            set
            {
                PhotonView view = GetComponentInParent<PhotonView>();

                Debug.Log($"audio muted: {view.Owner.NickName}");
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
    }


}
