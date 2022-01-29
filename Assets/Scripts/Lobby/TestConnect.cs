using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestConnect : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private RoomListing _roomListing;

    private List<RoomListing> _listings = new List<RoomListing>();

    public static TestConnect Instance = null;

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
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("TestConnect/Connected to Master.", this);
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
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Player Joined Room, room_name:" + PhotonNetwork.CurrentRoom.Name +", actor number:" + PhotonNetwork.LocalPlayer.ActorNumber);
        LoadCharacter.Instance.PlayerControl.enabled = false;
        // activate the current room canvases
        RoomsCanvases.Instance.CurrentRoomCanvas.Show();
        RoomsCanvases.Instance.CurrentRoomCanvas.LinkedSceneName = RoomsCanvases.Instance.CreateOrJoinRoomCanvas.LinkedSceneName;
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"master client switched to : {newMasterClient.ActorNumber}");
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Player Left Room");
        // loading the default scene.
        PhotonNetwork.LoadLevel("MainRoom");

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("new player entered room! #" + newPlayer.ActorNumber + "name:" + newPlayer.NickName);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("player left room! #" + otherPlayer.ActorNumber + "name:" + otherPlayer.NickName);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            //Removed from rooms list.
            if (info.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index != -1)
                {
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
                    }
                }
                else
                {
                    //Modify listing here.
                    //_listings[index].dowhatever.

                }
            }
        }
    }
}