using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VideoChatUI_TaeHo
{
    public class PlayerVideoList
    {
        public static int playerNumInPage = 6;
        public static int currentPage = 0;
        public static List<PlayerVideo> PlayerSet = new List<PlayerVideo> { new PlayerVideo("Player1", true) };
        public static List<PlayerVideo> PlayerSetInPage = new List<PlayerVideo> { new PlayerVideo("Player1", true) };
        public static List<PlayerVideo> PlayerSetPin = new List<PlayerVideo> { new PlayerVideo("Player1", true) };

        public static void AddPlayer(string name)
        {
            PlayerSet.Add(new PlayerVideo(name));
        }

        public static void IsPin(int num)
        {
            if (num >= 0 && num < PlayerSetInPage.Count)
            {
                PlayerSetInPage[num].isPin = !PlayerSetInPage[num].isPin;
            }
        }

        public static void CheckPlayerPin(int num)
        {
            if (PlayerSetInPage[num].isPin)
            {
                if (!PlayerSetPin.Contains(PlayerSetInPage[num]))
                {
                    PlayerSetPin.Add(PlayerSetInPage[num]);
                }
            }
            else
            {
                if (PlayerSetPin.Contains(PlayerSetInPage[num]))
                {
                    PlayerSetPin.Remove(PlayerSetInPage[num]);
                }
            }
        }

        public static void CheckPlayer()
        {
            int index = currentPage * (playerNumInPage - PlayerSetPin.Count);
            List<PlayerVideo> playersettmp = new List<PlayerVideo>();
            for (int i = 0; i < PlayerSet.Count; i++)
            {
                if (!PlayerSet[i].isPin)
                {
                    playersettmp.Add(PlayerSet[i]);
                }
            }
            playersettmp.InsertRange(index, PlayerSetPin);
            PlayerSet = playersettmp.ToList();
        }

        public static void CheckPlayerInPage()
        {
            int index = currentPage * (playerNumInPage - PlayerSetPin.Count);
            PlayerSetInPage.Clear();
            if (index <= PlayerSet.Count)
            {
                if (index + playerNumInPage <= PlayerSet.Count)
                {
                    foreach (PlayerVideo player in PlayerSet.GetRange(index, playerNumInPage))
                    {
                        PlayerSetInPage.Add(player);
                    }
                }
                else
                {
                    foreach (PlayerVideo player in PlayerSet.GetRange(index, PlayerSet.Count - index))
                    {
                        PlayerSetInPage.Add(player);
                    }
                }
            }
        }

        public static void RemovePlayer(int num)
        {
            int index = currentPage * (playerNumInPage - PlayerSetPin.Count);
            if (PlayerSetInPage.Contains(PlayerSet[num]))
            {
                if (PlayerSetPin.Contains(PlayerSet[num]))
                {
                    PlayerSetPin.Remove(PlayerSet[num]);
                }
                PlayerSet.Remove(PlayerSet[num]);
                CheckPlayer();
                CheckPlayerInPage();
            }
            else
            {
                PlayerSet.Remove(PlayerSet[num]);
            }
        }
    }

    public class PlayerVideo
    {
        public string Playername;
        public bool isPin;

        public PlayerVideo(string name)
        {
            this.Playername = name;
            this.isPin = false;
        }
        public PlayerVideo(string name, bool tmp)
        {
            this.Playername = name;
            this.isPin = tmp;
        }
    }

}
