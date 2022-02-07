using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class PlayerNameInputMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _playerName;
    [SerializeField]
    private Text _playerPassword;
    string url = "http://localhost:3000/enter";

    private RoomsCanvases _roomCanvases;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
    }
    const string playerNamePrefKey = "PlayerName";
  

    private void Start()
    {
        if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
            PhotonNetwork.NickName = PlayerPrefs.GetString(playerNamePrefKey);
        }       
    }

    public void OnClick_SetPlayerName()
    {
        if (string.IsNullOrEmpty(_playerName.text) || string.IsNullOrEmpty(_playerPassword.text))
        {
            Debug.LogError("Player Name or Password is null or empty");
            return;
        }

        StartCoroutine(SendRequest());

        // PhotonNetwork.NickName = _playerName.text;
        // Debug.Log("Player Name is "+ _playerName.text, this);

        // PlayerPrefs.SetString(playerNamePrefKey, _playerName.text);

        // _roomCanvases.AvatarSelectionCanvas.Show();
        // _roomCanvases.PlayerNameInputCanvas.Hide();
    }

    IEnumerator SendRequest() {
        WWWForm form = new WWWForm();
        form.AddField("name", _playerName.text);
        form.AddField("pw", _playerPassword.text);

        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);

            PhotonNetwork.NickName = _playerName.text;
            Debug.Log("Player Name is "+ _playerName.text, this);

            PlayerPrefs.SetString(playerNamePrefKey, _playerName.text);

            _roomCanvases.AvatarSelectionCanvas.Show();
            _roomCanvases.PlayerNameInputCanvas.Hide();
        }



        // List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        // // formData.Add(new MultipartFormDataSection("name"+_playerName.text+"&pw="+_playerPassword.text));
        // formData.Add(new MultipartFormDataSection("name", _playerName.text));
        // formData.Add(new MultipartFormDataSection("pw", _playerPassword.text));

        // UnityWebRequest www = UnityWebRequest.Post(url, formData);
        
        // yield return www.SendWebRequest();

        // if (www.result != UnityWebRequest.Result.Success)
        // {
        //     Debug.LogError(www.error);
        // } else {
        //     Debug.Log("Complete!");
        //     // JObject json = JObject.Parse(www.result);
        //     Debug.Log("Result:" + "\n" + www.result);
        //     connection = true;
        // }
    }

/*
    public void OnClick_JoinLobby()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        Debug.Log("Joining to Lobby...", this);
        Debug.Log(PhotonNetwork.LocalPlayer.NickName, this);
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        _roomCanvases.CreateOrJoinRoomCanvas.Show();
    }
*/
}
