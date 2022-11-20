using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VideoChatUI_TaeHo
{
    public class VideoPanelGroup : MonoBehaviour
    {
        private List<Transform> _videoPanels = new List<Transform>();
        private GameObject _nextPageButton;
        private GameObject _backPageButton;
        private PlayerListPanel _playerListPanel;

        private void Awake()
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                _videoPanels.Add(this.transform.GetChild(i).transform);
                this.transform.GetChild(i).GetComponent<VideoPanel>().VideoPanelNum = i;
                if (i == 0) this.transform.GetChild(0).GetComponent<Image>().color = Color.cyan;
            }
            _nextPageButton = GameObject.Find("NextPageButton");
            _backPageButton = GameObject.Find("BackPageButton");
            _playerListPanel = GameObject.Find("PlayerListPanel").GetComponent<PlayerListPanel>();
        }

        private void Start()
        {
            PlayerVideoList.CheckPlayer();
            PlayerVideoList.CheckPlayerInPage();
            VideoPanelUpdate();
            PageButton();
            _playerListPanel.PlayerPanelAdd();
            _playerListPanel.PlayerPanelUpdate();
        }

        public void VideoPanelUpdate()
        {
            for (int i = 0; i < PlayerVideoList.playerNumInPage; i++)
            {
                if (PlayerVideoList.PlayerSetInPage.Count > i)
                {
                    _videoPanels[i].gameObject.SetActive(true);
                    _videoPanels[i].GetComponentInChildren<Text>().text = PlayerVideoList.PlayerSetInPage[i].Playername;
                }
                else
                {
                    _videoPanels[i].gameObject.SetActive(false);
                }
            }
        }
        public void PageButton()
        {
            int index = PlayerVideoList.currentPage * (PlayerVideoList.playerNumInPage - PlayerVideoList.PlayerSetPin.Count);
            if (PlayerVideoList.PlayerSet.Count <= PlayerVideoList.playerNumInPage)
            {
                _backPageButton.SetActive(false);
                _nextPageButton.SetActive(false);
            }
            else
            {
                if (index == 0)
                {
                    _backPageButton.SetActive(false);
                    _nextPageButton.SetActive(true);
                }
                else if (index + PlayerVideoList.playerNumInPage >= PlayerVideoList.PlayerSet.Count)
                {
                    _backPageButton.SetActive(true);
                    _nextPageButton.SetActive(false);
                }
                else
                {
                    _backPageButton.SetActive(true);
                    _nextPageButton.SetActive(true);
                }
            }
        }
    }

}
