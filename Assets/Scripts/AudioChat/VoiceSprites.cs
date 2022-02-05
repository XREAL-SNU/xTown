using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceSprites : MonoBehaviour
{
    private PhotonVoiceView _voiceView;

    [SerializeField]
    private Image recorderSprite;

    [SerializeField]
    private Image speakerSprite;

    // Monobehaviour callbacks
    private void Awake()
    {
        _voiceView = this.GetComponentInParent<PhotonVoiceView>();
    }

    
    private void Update()
    {
        recorderSprite.enabled = _voiceView.IsRecording;
        speakerSprite.enabled = _voiceView.IsSpeaking;
    }
}
