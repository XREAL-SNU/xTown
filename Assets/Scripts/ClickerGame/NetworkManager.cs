using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("DisconnectPanel")]
    public GameObject DisconnectPanel;
    public InputField NicknameInput;

    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public Text ValueText, PlayersText, ClickUpgradeText, AutoUpgradeText,
        ValuePerClickText, ValuePerSecondText;
    public Button ClickUpgradeBtn, AutoUpgradeBtn;

    float nextTime;
    int clickUpgradeCostPer = 10;
    int clickUpgradeAddPer = 2;
    int autoUpgradeostPer = 500;
    int autoUpgradeAddPer = 2;

    void Start()
    {
        Screen.SetResolution(540, 960, false);

    }

    public void Connect()
    {
        if (PhotonNetwork.LocalPlayer != null)
        {
            PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null);

    }
    public override void OnJoinedRoom()
    {
        ShowPanel(RoomPanel);
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    void ShowPanel(GameObject CurPanel)
    {
        DisconnectPanel.SetActive(false);
        RoomPanel.SetActive(false);
        CurPanel.SetActive(true);
    }

    PlayerScript FindPlayer()
    {
        foreach(GameObject Player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (Player.GetPhotonView().IsMine) return Player.GetComponent<PlayerScript>();
        }
        return null;
    }
    public void Click()
    {
        PlayerScript Player = FindPlayer();
        Player.value += Player.valuePerClick;
    }

    public void ClickUpgrade()
    {
        PlayerScript Player = FindPlayer();

        if(Player.value >= Player.clickUpgradeCost)
        {
            Player.value -= Player.clickUpgradeCost;
            Player.valuePerClick += Player.clickUpgradeAdd;
            Player.clickUpgradeCost += Player.clickUpgradeAdd * clickUpgradeCostPer;
            Player.clickUpgradeAdd += clickUpgradeAddPer;

            ClickUpgradeText.text = Player.clickUpgradeAdd + " / click" + "\n" + "Cost : "
                + Player.clickUpgradeCost;

            ValuePerClickText.text = Player.valuePerClick.ToString();
        }
    }

    public void AutoUpgrade()
    {
        PlayerScript Player = FindPlayer();

        if (Player.value >= Player.autoUpgradeCost)
        {
            Player.value -= Player.autoUpgradeCost;
            Player.valuePerSecond += Player.autoUpgradeAdd;
            Player.autoUpgradeCost += autoUpgradeostPer;
            Player.autoUpgradeAdd += autoUpgradeAddPer;

            AutoUpgradeText.text = Player.autoUpgradeAdd + " / sec" + "\n" + "Cost : "
                + Player.autoUpgradeCost;

            ValuePerSecondText.text = Player.valuePerSecond.ToString();
        }
    }

    void ShowPlayers()
    {
        string playersText = "";

        foreach (GameObject Player in GameObject.FindGameObjectsWithTag("Player"))
        {
            playersText += Player.GetPhotonView().Owner.NickName + " / " +
                Player.GetComponent<PlayerScript>().value.ToString() + "\n";
        }
        PlayersText.text = playersText;
    }

    void EnableUpgrade()
    {
        PlayerScript Player = FindPlayer();

        ClickUpgradeBtn.interactable = Player.value >= Player.clickUpgradeCost;
        AutoUpgradeBtn.interactable = Player.value >= Player.autoUpgradeCost;
    }

    void UpdateValue()
    {
        PlayerScript Player = FindPlayer();
        Player.value += Player.valuePerSecond;
    }
    void Update()
    {
        if (!PhotonNetwork.InRoom) return;

        ShowPlayers();
        EnableUpgrade();

        if(Time.time > nextTime)
        {
            nextTime = Time.time + 1;
            UpdateValue();
        }
    }
}
