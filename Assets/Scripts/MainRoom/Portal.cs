using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Portal : MonoBehaviourPunCallbacks
{
    public string SceneName;
    public CamManager _camManager;
    private bool _portalConnect = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            TestConnect.Instance._portalSceneName = SceneName;
            TestConnect.Instance._isStartingPortal = true;
            _camManager.enabled = false;
            PlayerPrefs.SetString("PastScene", "MainRoom");
            _portalConnect = true;
            PhotonNetwork.LeaveRoom();
        }
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("Portal/Joined Room!!!");
        //SceneManager.LoadScene("MainRoom", LoadSceneMode.Single);
    }
    public override void OnCreatedRoom()
    {
        if(_portalConnect)
        {
            Debug.Log("Portal/Created Whiteboard Room!!!");
            PhotonNetwork.LoadLevel(SceneName);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Failed.. because " + message);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed.. because " + message);
    }
}
