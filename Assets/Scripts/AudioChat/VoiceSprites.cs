using Photon.Pun;
using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceSprites : MonoBehaviour
{
    private PhotonVoiceView _voiceView;
    private PhotonView _view;

    [SerializeField]
    private Image speakerSprite;

    
    // Monobehaviour callbacks
    private void Awake()
    {
        _voiceView = this.GetComponentInParent<PhotonVoiceView>();
    }

    private void Start()
    {
        _view = GetComponentInParent<PhotonView>();
        if (_view.IsMine) speakerSprite.enabled = false;
    }

    private void Update()
    {
        if (_view.IsMine) return;
        speakerSprite.enabled = _voiceView.IsSpeaking;
    }
}
