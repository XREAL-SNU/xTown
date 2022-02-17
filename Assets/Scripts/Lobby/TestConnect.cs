using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestConnect : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;

    [SerializeField]
    private RoomListing _roomListing;

    private List<RoomListing> _listings = new List<RoomListing>();
    public bool _isStarting = true;
    public static TestConnect Instance = null;
    public bool _isStartingYacht = false;
    public bool _isLeavingYacht = false;
    public bool _isStartingPortal = false;
    public bool _isLeavingPortal = false;
    public bool _isStartingPocket = false;
    public bool _isLeavingPocket = false;
    public bool _notStart = false;
    public string _portalSceneName;

    private void Awake()
    {
        // singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        Debug.Log("Connecting to Photon...", this);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();

        PlayerPrefs.SetString("prevScene", "None");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("TestConnect/Connected to Master.", this);
        if (!PhotonNetwork.InLobby) PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Failed to connect to Photon: " + cause.ToString(), this);
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Player Left Lobby.", this);
        PlayerPrefs.DeleteAll();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("TestConnect/Player Joined Lobby");
        if(_isStarting)
        {
            _isStarting = false;
            Debug.Log("TestConnect/isStarting");
            RoomsCanvases.Instance.LoadingCanvas.Hide();
            RoomsCanvases.Instance.PlayerNameInputCanvas.Show();
        }
        if(_isStartingPortal)
        {
            Debug.Log("TestConnect/isStartingPortal");
            RoomOptions options = new RoomOptions();
            options.BroadcastPropsChangeToAll = true;
            options.MaxPlayers = 10;
            PhotonNetwork.JoinOrCreateRoom(_portalSceneName, options, TypedLobby.Default);
            _isStartingPortal =false;
        }
        if(_isStartingYacht)
        {
            Debug.Log("TestConnect/isStartingYacht");
            RoomOptions options = new RoomOptions();
			options.BroadcastPropsChangeToAll = true;
			options.MaxPlayers = 2;
			PhotonNetwork.JoinOrCreateRoom("Yacht", options, TypedLobby.Default);
            _isStartingYacht = false;
        }
        if(_isStartingPocket)
        {
            Debug.Log("TestConnect/isStartingPocket");
            RoomOptions options = new RoomOptions();
			options.BroadcastPropsChangeToAll = true;
			options.MaxPlayers = 2;
			PhotonNetwork.JoinOrCreateRoom("PocketBall", options, TypedLobby.Default);
            _isStartingPocket = false;
        }
        if(_isLeavingYacht)
        {
            Debug.Log("isLeavingYacht");
            RoomOptions options = new RoomOptions();
            options.BroadcastPropsChangeToAll = true;
            options.MaxPlayers = 20;
            options.EmptyRoomTtl = 20000;
            options.PlayerTtl = 30000;
            PhotonNetwork.JoinOrCreateRoom("MainWorldTest", options, TypedLobby.Default);
            _notStart = true;
            _isLeavingYacht= false;
        }
        if(_isLeavingPortal)
        {
            Debug.Log("isLeavingPortal");
            RoomOptions options = new RoomOptions();
            options.BroadcastPropsChangeToAll = true;
            options.MaxPlayers = 20;
            options.EmptyRoomTtl = 20000;
            options.PlayerTtl = 30000;
            PhotonNetwork.JoinOrCreateRoom("MainWorldTest", options, TypedLobby.Default);
            _notStart = true;
            _isLeavingPortal = false;
        }
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.CurrentRoom.Name.Contains("MainWorldTest"))
        {
            Debug.Log("Player Joined Room, room_name:" + PhotonNetwork.CurrentRoom.Name + ", actor number:" + PhotonNetwork.LocalPlayer.ActorNumber);
        }
        else
        {
            Debug.Log("TestConect/PlayerJoined MainWorld");
            if(_notStart)
            {
                Debug.Log("LoadLevel MainRoom");
                SceneManager.LoadScene("MainRoom", LoadSceneMode.Single);
                _notStart = false;
            }
        }
        Thread.Sleep(2000);
    }

    
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"master client switched to : {newMasterClient.ActorNumber}");
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Player Left Room");
        if(!PhotonNetwork.InLobby) PhotonNetwork.JoinLobby();
        // loading the default scene.
        // PhotonNetwork.LoadLevel("MainRoom");

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("new player entered room! #" + newPlayer.ActorNumber + "name:" + newPlayer.NickName);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("player left room! #" + otherPlayer.ActorNumber + "name:" + otherPlayer.NickName);
    }

    /*public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("TestConnect/Room List Updated!!!!");
        foreach (RoomInfo info in roomList)
        {
            //Removed from rooms list.
            if (info.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index != -1)
                {
                    Debug.Log("TestConnect/RoomListRemoved " + _listings[index].RoomInfo.Name);
                    if (_listings[index] == null) return;
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            //Added to Room list.
            else
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index == -1)
                {
                    RoomListing listing = Instantiate(_roomListing, _content);
                    if (listing != null)
                    {
                        listing.SetRoomInfo(info);
                        _listings.Add(listing);
                        Debug.Log("Room Added " + listing.RoomInfo.Name);
                    }
                }
                else
                {
                    //Modify listing here.
                    //_listings[index].dowhatever.

                }
            }
        }
    }*/
}