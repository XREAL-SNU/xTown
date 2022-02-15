using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XReal.XTown.UI;
using XReal.XTown.VoiceChat;

public class AudioChatUserListing : UIBase
{
    // used for discriminating between users with same nickname.
    // maybe we want PlayerInfo, but we probably won't need ALL of its information.
    // so for minimality.
    public int ActorNr;
    enum Texts
    {
        PlayerNameText
    }

    enum Images
    {
        SpeakerOnImage,
        SpeakerOffImage
    }

    public override void Init()
    {

        Bind<Text>(typeof(Texts));

        Bind<Image>(typeof(Images));
        // to get a bound gameObject, use GetUIComponent and provide it with UIElementType and UIElementId.
        GetUIComponent<Image>((int)Images.SpeakerOffImage).gameObject.BindEvent(OnClick_Unmute);
        GetUIComponent<Image>((int)Images.SpeakerOnImage).gameObject.BindEvent(OnClick_Mute);

        SpeakerOffImage.enabled = false;
    }
    
    Image SpeakerOffImage
    {
        get
        {
            return GetUIComponent<Image>((int)Images.SpeakerOffImage);
        }
    }

    Image SpeakerOnImage
    {
        get
        {
            return GetUIComponent<Image>((int)Images.SpeakerOnImage);
        }
    }


    // save current volume before muting.
    float _previousVolume;

    public void OnClick_Mute(PointerEventData evData)
    {
        Debug.Log("Mute speaker: " + PlayerNameText);
        // mute that person's audiosource.
        SpeakerOnImage.enabled = false;
        SpeakerOffImage.enabled = true;

        if (ActorNr < 1)
        {
            Debug.LogError("AudioChatUserListing/ ActorNr of item not set");
            return;
        }
        PlayerVoice voice = RoomManager.Room.GetComponentInPlayerById<PlayerVoice>(this.ActorNr);
        voice.AudioSourceMuted = true;
    }

    public void OnClick_Unmute(PointerEventData evData)
    {
        // unmute that person's audiosource.
        SpeakerOnImage.enabled = true;
        SpeakerOffImage.enabled = false;

        PlayerVoice voice = RoomManager.Room.GetComponentInPlayerById<PlayerVoice>(this.ActorNr);
        voice.AudioSourceMuted = false;
    }

    public string PlayerNameText
    {
        get
        {
            string text = GetUIComponent<Text>((int)Texts.PlayerNameText).text;
            return text;
        }
        set
        {
            GetUIComponent<Text>((int)Texts.PlayerNameText).text = value;
        }
    }

    
    public void Remove()
    {
        Destroy(gameObject);
    }
}
