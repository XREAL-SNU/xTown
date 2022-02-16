using DG.Tweening;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XReal.XTown.UI;
using XReal.XTown.VoiceChat;

public class VoiceChatChannelsPopup : UIPopup
{

    List<PlayerInfo> _playerInfos;
    enum Buttons
    {
        CloseButton,
        
    }

    enum Toggles
    {
        VoiceChatToggle,
    }



    [SerializeField]
    Transform _content;


    private void Start()
    {
        // tweening
        RectTransform panelTransfrom = transform.Find("Panel").GetComponent<RectTransform>();
        var y = panelTransfrom.anchoredPosition.y;
        panelTransfrom.anchoredPosition = new Vector2(1467, y);
        Sequence easeInFromRight = DOTween.Sequence().Append(panelTransfrom.DOAnchorPosX(540, 0.3f)).SetEase(Ease.InCubic);

        Init();
    }

    public override void Init()
    {
        base.Init();


        Bind<Button>(typeof(Buttons));
        Bind<VoiceChatToggle>(typeof(Toggles));

        // to get a bound gameObject, use GetUIComponent and provide it with UIElementType and UIElementId.
        GetUIComponent<Button>((int)Buttons.CloseButton).gameObject.BindEvent(OnClick_Close);
        VoiceChatToggle _voiceToggle = GetUIComponent<VoiceChatToggle>((int)Toggles.VoiceChatToggle);
        _voiceToggle.gameObject.BindEvent((PointerEventData data)=> { _voiceToggle.OnToggleValueChanged_Talk(); });


        _playerInfos = RoomManager.Room.GetPlayerInfoList();
        foreach(PlayerInfo info in _playerInfos)
        {
            Debug.Log($"player: {info.PlayerName}");
        }

        _playerInfos.ForEach((info) => {
            PlayerVoice voice = RoomManager.Room.GetComponentInPlayerById<PlayerVoice>(info.ActorNr);
            if (voice.IsVoiceOn)
            {
                AddItem(info);
            }
        });

        // bind player voice changed event
        Voice.VoiceChat.AddListener(OnPlayerVoiceChanged_UpdateList);
    }

    // player voice event callback
    public void OnPlayerVoiceChanged_UpdateList(int actorNr, bool state)
    {
        // if not listening return
        if (!gameObject.activeInHierarchy) return;

        Debug.Log($"ListPopup/ OnPlayerVoiceChanged event: {actorNr} state {state}");
        bool existInList = false;
        AudioChatUserListing[] list = GetComponentsInChildren<AudioChatUserListing>();
        foreach(AudioChatUserListing listing in list)
        {
            if (listing.ActorNr != actorNr) continue;
            Debug.Log($"Listing found {actorNr} event state {state}");
            if (!state) listing.Remove();
            existInList = true;
        }
        if (!existInList && state)
        {
            Debug.Log($"Listing NOT found {actorNr}, adding!");

            PlayerInfo info = new PlayerInfo();
            info.ActorNr = actorNr;
            info.PlayerName = PhotonNetwork.CurrentRoom.GetPlayer(actorNr).NickName;
            AddItem(info);
        }
    }

    void AddItem(string name)
    {
        AudioChatUserListing listing = Instantiate(Resources.Load<AudioChatUserListing>("UI/Popup/VoiceChatRoom/PlayerListItem"), _content);
        listing.Init();
        listing.PlayerNameText = name;
    }

    void AddItem(PlayerInfo info)
    {
        AudioChatUserListing listing = Instantiate(Resources.Load<AudioChatUserListing>("UI/Popup/VoiceChatRoom/PlayerListItem"), _content);
        listing.Init();

        // set if me!
        if (info.ActorNr == PhotonNetwork.LocalPlayer.ActorNumber) listing.IsMe = true;
        else listing.IsMe = false;

        listing.PlayerNameText = info.PlayerName;
        listing.ActorNr = info.ActorNr;
        // remember and set voice state
        PlayerVoice voice = RoomManager.Room.GetComponentInPlayerById<PlayerVoice>(info.ActorNr);
        if (!listing.IsMe && voice.AudioSourceMuted)
        {
            listing.SpeakerOffImage.enabled = true;
            listing.SpeakerOnImage.enabled = false;
        }
        else if(!listing.IsMe)
        {
            listing.SpeakerOnImage.enabled = true;
            listing.SpeakerOffImage.enabled = false;
        }
    }

    void ClearList()
    {
        AudioChatUserListing[] listings = GetComponentsInChildren<AudioChatUserListing>(_content);
        foreach(AudioChatUserListing listing in listings)
        {
            listing.Remove();
        }
    }

    public void OnPlayerJoined_AddItem(PlayerInfo info)
    {
        Debug.Log($"Player Joined {info.PlayerName}");
    }

    public void OnPlayerLeft_RemoveItem(PlayerInfo info)
    {
        Debug.Log($"Player Left {info.PlayerName}");
    }

    // UI callbacks
    public override void ClosePopup()
    {
        // DOtween
        RectTransform panelTransfrom = transform.Find("Panel").GetComponent<RectTransform>();
        Sequence easeInFromRight = DOTween.Sequence().Append(panelTransfrom.DOAnchorPosX(1467, 0.3f))
            .SetEase(Ease.OutCubic)
            .OnComplete(base.ClosePopup);

    }

    public void OnClick_Close(PointerEventData data)
    {
        ClearList();
        ClosePopup();
    }

    private void OnDestroy()
    {
        Voice.VoiceChat.RemoveListener(OnPlayerVoiceChanged_UpdateList);

    }
}

