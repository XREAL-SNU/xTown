using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VideoChatUI_TaeHo
{
    public class PlayerListPanel : MonoBehaviour
    {
        public void PlayerPanelAdd()
        {
            GameObject playerpanel = Instantiate(Resources.Load<GameObject>("VideoChatUI_TaeHo/PlayerList"));
            playerpanel.transform.SetParent(gameObject.transform);
        }

        public void PlayerPanelUpdate()
        {
            for (int i = 0; i < PlayerVideoList.PlayerSet.Count; i++)
            {
                GameObject playerpanel = transform.GetChild(i).gameObject;
                playerpanel.GetComponent<PlayerListButton>()._playerNum = i;
                PlayerPanelName(playerpanel, i);
                if (PlayerVideoList.PlayerSetInPage.Contains(PlayerVideoList.PlayerSet[i])) playerpanel.GetComponent<Image>().color = Color.yellow;
                else playerpanel.GetComponent<Image>().color = Color.white;
            }
        }

        public void PlayerPanelName(GameObject playerpanel, int playernum)
        {
            string name = PlayerVideoList.PlayerSet[playernum].Playername;
            int index = name.IndexOf("r") + 1;
            if (index > 0) playerpanel.name = name.Substring(index, name.Length - index);
            playerpanel.transform.GetChild(0).GetComponent<Text>().text = playerpanel.name;
        }
    }

}
