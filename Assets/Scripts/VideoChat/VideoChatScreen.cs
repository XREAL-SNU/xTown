using agora_gaming_rtc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XReal.XTown.UI;

public class VideoChatScreen : UIBase
{

    enum Toggles
    {
        VideoToggle, AudioToggle
    }

    enum InfoText
    {
        NickNameText
    }

    public bool IsLocal = false;
    public bool Pinned = false;
    #region MonobehaviourCallbacks
    private void OnEnable()
    {
        GetUIComponent<ImageToggle>((int)Toggles.VideoToggle).AddListener(SetVideoMute);
        GetUIComponent<ImageToggle>((int)Toggles.AudioToggle).AddListener(SetAudioMute);
        Debug.Log($"VideoChatScreen/ enabled: Nickname{GetNickName()}");
        // custom logic to bind the video surface to this screen.
    }

    private void OnDisable()
    {
        GetUIComponent<ImageToggle>((int)Toggles.VideoToggle).RemoveListener(SetVideoMute);
        GetUIComponent<ImageToggle>((int)Toggles.AudioToggle).RemoveListener(SetAudioMute);
        Debug.Log($"VideoChatScreen/ disabled: Nickname{GetNickName()}");
        // custom logic to un-bind the video surface from this screen.
    }

    private void Awake()
    {
        // dont do this in OnEnable... you want binding to happen only once!
        Init();
    }

    public override void Init()
    {
        Bind<ImageToggle>(typeof(Toggles));
        Bind<Text>(typeof(InfoText));

        SetNickName("Default Name");
    }

    #endregion

    // TODO set nickname of player
    public void SetNickName(string name)
    {
        GetUIComponent<Text>((int)InfoText.NickNameText).text = name;
    }

    public string GetNickName()
    {
        return GetUIComponent<Text>((int)InfoText.NickNameText).text;
    }

    void SetVideoMute(bool state)
    {
        if (IsLocal)
        {
            Debug.Log("VideoChatScreen/ local video toggled: state = " + state);
        }
        else
        {
            Debug.Log("VideoChatScreen/ remote video toggled: state = " + state);
        }
        // TODO : mute video from state

    }

    void SetAudioMute(bool state)
    {
        if (IsLocal)
        {
            Debug.Log("VideoChatScreen/ local audio toggled: state = " + state);
        }
        else
        {
            Debug.Log("VideoChatScreen/ remote audio toggled: state = " + state);
        }
        // TODO : mute audio from state
    }

    public void Destroy()
    {
        // do cleanup for destruction
        Destroy(this.gameObject);
    }
}
