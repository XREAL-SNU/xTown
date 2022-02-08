using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PlayerListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private PlayerListing _playerListing;
    [SerializeField]
    private Text _readyUpText;

    private List<PlayerListing> _listings = new List<PlayerListing>();
    private RoomsCanvases _roomsCanvases;
    private bool _ready = false;


    public override void OnEnable()
    {
        Debug.Log("PlayerListingMenu/fetching current room's players!");
        base.OnEnable();
        SetReadyUp(false);
        GetCurrentRoomPlayers();
    }

    public override void OnDisable()
    {

        base.OnDisable();
        for (int i = 0; i < _listings.Count; i++)
            Destroy(_listings[i].gameObject);

        _listings.Clear();
    }

    public void FirstInitialize(RoomsCanvases canvases)
    {

        _roomsCanvases = canvases;
    }

    private void SetReadyUp(bool state)
    {
        _ready = state;
        if (_ready)
            _readyUpText.text = "Not";
        else
            _readyUpText.text = "Ready";
    }


    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;

        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
        
    }

    private void AddPlayerListing(Player player)
    {
        int index = _listings.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            _listings[index].SetPlayerInfo(player);
        }
        else
        {
            PlayerListing listing = Instantiate(_playerListing, _content);

            if (listing != null)
            {
                listing.SetPlayerInfo(player);
                _listings.Add(listing);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("new player entered room! #" + newPlayer.ActorNumber + "name:" + newPlayer.NickName);
        AddPlayerListing(newPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // why?
        _roomsCanvases.CurrentRoomCanvas.LeaveRoomMenu.OnClick_LeaveRoom();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _listings.FindIndex(x => x.Player == otherPlayer);
        Debug.Log("player left room! #" + otherPlayer.ActorNumber + "name:" + otherPlayer.NickName);

        if (index != -1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }

    // UI callbacks
    public void OnClick_StartGame()
    {
        string linkedSceneName = RoomsCanvases.Instance.CurrentRoomCanvas.LinkedSceneName;
        if (PhotonNetwork.IsMasterClient)
        {
            for(int i = 0; i < _listings.Count; i++)
            {
                if(_listings[i].Player != PhotonNetwork.LocalPlayer)
                {
                    if (!_listings[i].Ready) 
                    {
                        Debug.Log("Not Ready is who " + _listings[i].Player.NickName);
                        return;
                    }
                }
            }
            
            if (SceneManager.GetActiveScene().name.Equals(linkedSceneName))
            { // already in that level
                RoomsCanvases.Instance.CurrentRoomCanvas.Hide();
                return;
            }
            else
            { // not in level
                PhotonNetwork.LoadLevel(RoomsCanvases.Instance.CurrentRoomCanvas.LinkedSceneName);
                RoomsCanvases.Instance.CurrentRoomCanvas.Hide();
            }

            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
    }

    public void OnClick_ReadyUp()
    {
        if (PhotonNetwork.LocalPlayer == null) return; 
        if (!PhotonNetwork.IsMasterClient)
        {
            SetReadyUp(!_ready);
            photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, _ready);
        }
    }

    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool ready)
    {
        int index = _listings.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            _listings[index].Ready = ready;
        }
    }

    
}
