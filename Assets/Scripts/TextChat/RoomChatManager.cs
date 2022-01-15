using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
using ExitGames.Client.Photon;

public class RoomChatManager : MonoBehaviour, IChatClientListener
{
    public List<string> subscriberInfo;
    public List<GameObject> BlockList = new List<GameObject>();
    public List<Button> subscribersListButton = new List<Button>();
    

    [Header("Buttons")]
    public Button RoomSubscriberListButton;

    [Header("Panels")]
    public RectTransform RoomChatPanel;

    [Header("Scrolls")]
    public Scrollbar RoomSubscriberListScrollbar;


    [SerializeField]
    private string userID;
    private int currentSubscribers = 0;
    private int memberChangedChecker = 1;
    //private GameObject userInfo = (GameObject)Instantiate(Resources.Load("Prefabs/Room Subscriber"));


    ChatClient chatClient;

    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        userID = "ChatUser" + UnityEngine.Random.Range(0, 1000).ToString();
        //add listeners to input field and send button
        //InputFieldChat.onEndEdit.AddListener(delegate { OnEnterSend(); });
        RoomSubscriberListButton.onClick.AddListener(delegate { OnClickSend(); });
        RoomChatPanel.gameObject.SetActive(false);
        subscriberInfo.Add("Jane");
        subscriberInfo.Add("Peter");
        subscriberInfo.Add("Ana");
        for (int i = 0; i < subscriberInfo.Count; i++)
        {
            Debug.Log(subscriberInfo[i]);
        }

        //connect on start
        Connect();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (currentSubscribers != memberChangedChecker)
        {
            for (int i = 0; i < currentSubscribers; i++)
            {
                subscribersListButton[i].gameObject.SetActive(true);
                subscribersListButton[i].GetComponent<Text>().text = subscriberInfo[i];
            }
        }
    }




    void FlipTruthValue(GameObject Object)
    {
        if (Object.activeSelf == true)
        {
            Object.SetActive(false);
        }
        else
        {
            Object.SetActive(true);
        }
    }

    void OnClickSend()
    {
        FlipTruthValue(RoomChatPanel.gameObject);
    }

    void OnEnterSend()
    {

    }
    void Connect()
    {
        //set up the chat client
        this.chatClient = new ChatClient(this);
        this.chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
            PhotonNetwork.AppVersion, new AuthenticationValues(userID));

        Debug.Log("ChatManager/Connecting as: " + userID);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }
}
