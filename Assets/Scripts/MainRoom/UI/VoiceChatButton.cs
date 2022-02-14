using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XReal.XTown.UI;

public class VoiceChatButton : MonoBehaviour
{
    public void OnClick_ShowVoiceChatPopup()
    {
        UIManager.UI.ShowPopupUI<VoiceChatChannelsPopup>("VoiceChatChannelsPopup");

    }
}
