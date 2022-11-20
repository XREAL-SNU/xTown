using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace VideoChatUI_TaeHo
{
    public class VideoPanel : MonoBehaviour, IPointerClickHandler
    {
        public int VideoPanelNum;
        private VideoPanelGroup _videoPanel;
        private PlayerListPanel _playerListPanel;
        private Color _origin;

        private void Awake()
        {
            _videoPanel = GameObject.Find("VideoPanel").GetComponent<VideoPanelGroup>();
            _playerListPanel = GameObject.Find("PlayerListPanel").GetComponent<PlayerListPanel>();
            _origin = GetComponent<Image>().color;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PlayerVideoList.IsPin(VideoPanelNum);
            PlayerVideoList.CheckPlayerPin(VideoPanelNum);
            PlayerVideoList.CheckPlayer();
            PlayerVideoList.CheckPlayerInPage();
            _videoPanel.VideoPanelUpdate();
            _videoPanel.PageButton();
            _playerListPanel.PlayerPanelUpdate();
            ColorUpdate();
        }

        public void ColorUpdate()
        {
            for (int i = 0; i < PlayerVideoList.PlayerSetInPage.Count; i++)
            {
                if (i < PlayerVideoList.PlayerSetPin.Count) _videoPanel.transform.GetChild(i).GetComponent<Image>().color = Color.cyan;
                else _videoPanel.transform.GetChild(i).GetComponent<Image>().color = _origin;
            }
        }
    }

}
