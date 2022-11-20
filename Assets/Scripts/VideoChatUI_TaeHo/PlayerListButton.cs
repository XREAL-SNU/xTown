using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VideoChatUI_TaeHo
{
    public class PlayerListButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public int _playerNum;
        private VideoPanelGroup _videoPanel;
        private PlayerListPanel _playerListPanel;

        private void Awake()
        {
            _videoPanel = GameObject.Find("VideoPanel").GetComponent<VideoPanelGroup>();
            _playerListPanel = GameObject.Find("PlayerListPanel").GetComponent<PlayerListPanel>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.GetComponent<Image>().color = Color.red;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            this.transform.parent.GetComponent<PlayerListPanel>().PlayerPanelUpdate();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PlayerVideoList.RemovePlayer(_playerNum);
            _videoPanel.VideoPanelUpdate();
            _videoPanel.PageButton();
            _playerListPanel.PlayerPanelUpdate();
            _videoPanel.transform.GetChild(0).GetComponent<VideoPanel>().ColorUpdate();
            Destroy(this.gameObject);
        }
    }

}
