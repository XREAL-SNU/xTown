using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
public class PocketBallJoin : MonoBehaviourPunCallbacks
{
	/* public fields */
	[SerializeField]
	private GameObject _joinPanel;

	[SerializeField]
    private GameObject _mainCanvas;
	private byte _maxPlayersPerRoom = 2;
		//[SerializeField]
		//private Text _playerNumText;
		//private int _currentPlayers = 0;
	private int _currentRoomNum = 0;
    private string _sceneName = "PocketDye";

		/* private fields */

		// required so different version users cannot play together
	private string _gameVersion = "0";
	private bool isPocketBallConnecting = false;


		/* Monobehaviour callbacks */

	void Awake()
	{
		_joinPanel.SetActive(false);
		PhotonNetwork.AutomaticallySyncScene = true;
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
				isPocketBallConnecting = true;
			}
		}
			//_playerNumText.text = _currentPlayers+"/2 players waiting...";
	}

	/* public connection management methods */

	public void Connect()
	{
		_joinPanel.SetActive(false);
		_mainCanvas.SetActive(false);
		PlayerPrefs.SetString("PastScene", "MainRoom");
		TestConnect.Instance._isStartingPocket = true;
        PhotonNetwork.LeaveRoom();
	}
		/* Pun Callbacks */
        /*
		public override void OnConnectedToMaster()
		{
			if (isConnecting)
			{
				Debug.Log("Yacht/PhotonLauncher: OnConnectedToMaster");
				PhotonNetwork.JoinRandomRoom();
			}
		}*/
		/*public override void OnJoinedLobby()
		{
			if (isYachtConnecting)
			{
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
		}*/
	public override void OnJoinedRoom()
	{
		if(isPocketBallConnecting)
		{
			Debug.Log("PocketBallJoin: Joined Room");
			if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
			{
				Debug.Log("PocketBallJoin: waiting for other player");
				PhotonNetwork.LoadLevel("PocketDye");
            }
            else
            {
				Debug.Log("PocketBallJoin: you are the second player");
            }	
		}
	}
}
