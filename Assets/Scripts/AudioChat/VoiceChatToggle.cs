using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Voice.Unity;
using Photon.Voice.PUN;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

namespace XReal.XTown.VoiceChat
{
    public class VoiceChatToggle : MonoBehaviour
    {
        private PhotonVoiceNetwork _voiceNetwork;
        [SerializeField]
        private Toggle _talkToggle;
        [SerializeField]
        private Recorder _recorder;

        // monobehaviour callbacks
        private void OnEnable()
        {
            // prefer to do network stuff before START.
            _voiceNetwork = GetComponent<PhotonVoiceNetwork>();

        }

        private void Start()
        {
            _talkToggle = this.GetComponent<Toggle>();
            _talkToggle.onValueChanged.AddListener(delegate { this.OnToggleValueChanged_Talk(); });
        }

        // UI callbacks
        // on chat toggle
        public void OnToggleValueChanged_Talk()
        {
            if (_recorder)
            {
                Debug.Log("Recorder: " + _talkToggle.isOn);
                _recorder.TransmitEnabled = _talkToggle.isOn;
            }
        }

        // 
    }
}

