using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Realtime;
using Photon.Pun;
namespace XReal.XTown.Yacht
{
	public class PhotonLauncher : MonoBehaviourPunCallbacks
	{
		/* public fields */
		[SerializeField]
		private GameObject _joinPanel;

		[SerializeField]
		private byte _maxPlayersPerRoom = 2;
		//[SerializeField]
		//private Text _playerNumText;
		[SerializeField]
		private GameObject _mainCanvas;
		private PhotonView _view;
		//private int _currentPlayers = 0;
		private int _currentRoomNum = 0;
		private string _sceneName = "Yacht";

		/* private fields */

		// required so different version users cannot play together
		private string _gameVersion = "0";
		private bool isYachtConnecting = false;


		/* Monobehaviour callbacks */

		void Awake()
		{
			_joinPanel.SetActive(false);
			PhotonNetwork.AutomaticallySyncScene = true;
			_view = GetComponent<PhotonView>();
		}
		void Update()
        {
			if (Input.GetMouseButtonDown(0))
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Debug.DrawRay(ray.origin, ray.direction, Color.green);
				if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == gameObject)
				{
					if (!_joinPanel.activeInHierarchy) _joinPanel.SetActive(true);
				}
				isYachtConnecting = true;
			}
			//_playerNumText.text = _currentPlayers+"/2 players waiting...";
		}

		/* public connection management methods */

		public void Connect()
		{
			_joinPanel.SetActive(false);
			_mainCanvas.SetActive(false);
			PlayerPrefs.SetString("PastScene", "MainRoom");
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.JoinLobby();
			/*
			if (PhotonNetwork.IsConnected)
			{
				Debug.Log("Connected to Photon");
				PhotonNetwork.JoinRandomRoom();
			}
			else
			{
				Debug.Log("Connecting...");
				PhotonNetwork.ConnectUsingSettings();
				PhotonNetwork.GameVersion = _gameVersion;
			}
			*/

		}
		/* Pun Callbacks */

		/* called even when we just exited a scene. that's why we need that isConnecting variable
		public override void OnConnectedToMaster()
		{
			if (isConnecting)
			{
				Debug.Log("Yacht/PhotonLauncher: OnConnectedToMaster");
				PhotonNetwork.JoinRandomRoom();
			}
		}*/
		public override void OnJoinedLobby()
		{
			if(isYachtConnecting){
			//if(_currentPlayers%2 == 1)
			//{
				RoomOptions options = new RoomOptions();
        		options.BroadcastPropsChangeToAll = true;
        		options.MaxPlayers = 2;
        		PhotonNetwork.JoinOrCreateRoom(_sceneName, options, TypedLobby.Default);
			//}
			//else if(_currentPlayers%2 == 0)
			//{
			//	PhotonNetwork.JoinRoom(_sceneName);
			//}
			}
		}
		public override void OnJoinedRoom()
		{
			if(isYachtConnecting)
			{
				Debug.Log("Yacht/PhotonLauncher: Joined Room");
				if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
				{
					Debug.Log("Yacht/PhotonLauncher: waiting for other player");
					PhotonNetwork.LoadLevel("Yacht");
            	}
            	else
            	{
					Debug.Log("Yacht/PhotonLauncher: you are the second player");
            	}
			}
		}
		/*
		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			Debug.Log("Yacht/PhotonLauncher: OnJoinRandomFailed");
			PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = _maxPlayersPerRoom });
		}


		public override void OnDisconnected(DisconnectCause cause)
		{
			Debug.LogError("Yacht/PhotonLauncher: Disconnect with reason " + cause);

			isConnecting = false;
			_joinPanel.SetActive(true);
		}
		[PunRPC]
		void addPlayers()
		{
			_currentPlayers = _currentPlayers/2;
			++_currentPlayers;
		}*/
	}



}
