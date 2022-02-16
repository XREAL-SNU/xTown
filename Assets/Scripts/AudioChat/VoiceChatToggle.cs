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
        

        Image _check;
        bool isOn = false;
        // monobehaviour callbacks
        private void OnEnable()
        {
            // prefer to do network stuff before START.
            _voiceNetwork = GetComponent<PhotonVoiceNetwork>();
        }

        private void Start()
        {
            //_talkToggle = this.GetComponent<Toggle>();
            _check = transform.Find("Background/Checkmark").GetComponent<Image>();
            // this toggle is attached to UI element, so START is called after VoiceChat is initialized.
            isOn = Voice.VoiceChat.IsVoiceOn;
            if (!isOn) _check.enabled = false;
        }

        // UI callbacks
        // on chat toggle
        public void OnToggleValueChanged_Talk()
        {
            isOn = !isOn;
            if (isOn)
            {
                Debug.Log("Toggle/isOn");
                Voice.VoiceChat.StartVoice();
                _check.enabled = true;
            }
            else
            {
                Debug.Log("Toggle/isOff");
                Voice.VoiceChat.PauseVoice();
                _check.enabled = false;
            }

            /*
            if (_recorder)
            {
                Debug.Log("Recorder: " + _talkToggle.isOn);
                _recorder.TransmitEnabled = _talkToggle.isOn;
            }
            */
        }

        
    }
}

