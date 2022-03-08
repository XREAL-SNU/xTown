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
        VideoToggle, AudioToggle, PinToggle
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
        if (IsLocal)
        {
            GetUIComponent<ImageToggle>((int)Toggles.PinToggle).gameObject.SetActive(false);
        }
        else
        {
            GetUIComponent<ImageToggle>((int)Toggles.PinToggle).AddListener(Pin);
        }
        // custom logic to bind the video surface to this screen.
    }

    private void OnDisable()
    {
        GetUIComponent<ImageToggle>((int)Toggles.VideoToggle).RemoveListener(SetVideoMute);
        GetUIComponent<ImageToggle>((int)Toggles.AudioToggle).RemoveListener(SetAudioMute);
        GetUIComponent<ImageToggle>((int)Toggles.PinToggle).RemoveListener(Pin);
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

        ScreenUser = new User();
    }

    #endregion

    #region User
    User _screenUser;

    public User ScreenUser
    {
        get
        {
            if (_screenUser is null) _screenUser = new User();
            return _screenUser;
        }
        set
        {
            _screenUser = value;

            // sets displayed name as well
            if (value.Name is null) return;
            SetNickName(value.Name);
        }
    }

    // TODO set nickname of player
    public void SetNickName(string name)
    {
        GetUIComponent<Text>((int)InfoText.NickNameText).text = name;
    }

    public string GetNickName()
    {
        return GetUIComponent<Text>((int)InfoText.NickNameText).text;
    }

    #endregion

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

    void Pin(bool state)
    {
        var container = GetComponentInParent<VideoChatScreens>();
        Pinned = state;
        if (state) container.PinScreen(transform.GetSiblingIndex());
        if (!state) container.UnpinScreen(transform.GetSiblingIndex());
    }

    public void ForceUnpin()
    {
        GetUIComponent<ImageToggle>((int)Toggles.PinToggle).SetToggleValue(false);
    }

    // destruction of screen
    public void Hide()
    {
        // TODO detach stream from this screen
        gameObject.SetActive(false);
    }

    public void Destroy()
    {
        // TODO: do cleanup for destruction
        gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    //test
    Button killBtn;
    private void Start()
    {
        if (TryGetComponent(out Button btn)){
            killBtn = btn;
        }
    }

    public void OnClick_KillItem()
    {
        Debug.Log($"killed user {_screenUser.Name}");
        VideoChatUsers.RemoveUser(_screenUser);
    }

}
