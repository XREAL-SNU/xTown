using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VideoChatUI_TaeHo
{
    public class PlayerIn : MonoBehaviour, IPointerClickHandler
    {
        private int _num = 2;
        private VideoPanelGroup _videoPanel;
        private PlayerListPanel _playerListPanel;

        private void Awake()
        {
            _videoPanel = GameObject.Find("VideoPanel").GetComponent<VideoPanelGroup>();
            _playerListPanel = GameObject.Find("PlayerListPanel").GetComponent<PlayerListPanel>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            string name = "Player" + _num;
            _num++;
            PlayerVideoList.AddPlayer(name);
            PlayerVideoList.CheckPlayer();
            PlayerVideoList.CheckPlayerInPage();
            _videoPanel.VideoPanelUpdate();
            _videoPanel.PageButton();
            _playerListPanel.PlayerPanelAdd();
            _playerListPanel.PlayerPanelUpdate();
        }
    }

}

