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

		/* private fields */

		// required so different version users cannot play together
		private string _gameVersion = "0";
		private bool isConnecting;


		/* Monobehaviour callbacks */

		void Awake()
		{
			_joinPanel.SetActive(false);
			PhotonNetwork.AutomaticallySyncScene = true;
		}


		/* public connection management methods */

		public void Connect()
		{

			isConnecting = true;
			_joinPanel.SetActive(false);

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
		}


		/* Yacht scene specific funcs */
		void Update()
        {
			if (Input.GetMouseButtonDown(0) && GameManager.turnCount <= 3)
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Debug.DrawRay(ray.origin, ray.direction, Color.green);
				if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == gameObject)
				{
					if (!_joinPanel.activeInHierarchy) _joinPanel.SetActive(true);
				}
			}
		}
		/* Pun Callbacks */

		// called even when we just exited a scene. that's why we need that isConnecting variable
		public override void OnConnectedToMaster()
		{
			if (isConnecting)
			{
				Debug.Log("Yacht/PhotonLauncher: OnConnectedToMaster");
				PhotonNetwork.JoinRandomRoom();
			}
		}

		
		public override void OnJoinedRoom()
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
	}



}
