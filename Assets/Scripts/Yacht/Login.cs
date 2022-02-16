using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [Header("UI")]
    public InputField NameInput;
    public Button JoinBtn;

    private Dictionary<string, RoomInfo> _cachedRoomsDict = new Dictionary<string, RoomInfo>();
    private Dictionary<string, RoomListItem> _roomListItemsDict = new Dictionary<string, RoomListItem>();

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        // set up a default username
        NameInput.text = "XTown" + Random.Range(0, 1000);

    }

    public void OnClick_Login()
    {
        string name = NameInput.text;

        if (name == "")
        {
            Debug.LogError("Invalid username");
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = name;
        PhotonConnector.Instance.ConnectToPhoton();

        // should check for success?
        PanelsManager.Instance.SetActivePanel("MenuPanel");
    }
}
