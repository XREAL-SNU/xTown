using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    public string[] FriendsList;
    public string[] ChannelsList;
    public string UserName { get; set; }
    private string currentChannelName;

    public ChatClient chatClient;
    public ChatChannel chatChannel;
    protected ChatAppSettings chatAppSettings;
    public InputField inputField;
    public Text currentChannelText;

    private readonly Dictionary<object, object> properties = new Dictionary<object, object>();

    public bool showState = true;
    public GameObject Title;
    public Text StateText;
    public Text UserIdText;

    // pasted from DemoChat >> ChatGui.cs
    private static string HelpText = "\n    -- HELP --\n" +
            "To subscribe to channel(s) (channelnames are case sensitive) :  \n" +
                "\t<color=#E07B00>\\subscribe</color> <color=green><list of channelnames></color>\n" +
                "\tor\n" +
                "\t<color=#E07B00>\\s</color> <color=green><list of channelnames></color>\n" +
                "\n" +
                "To leave channel(s):\n" +
                "\t<color=#E07B00>\\unsubscribe</color> <color=green><list of channelnames></color>\n" +
                "\tor\n" +
                "\t<color=#E07B00>\\u</color> <color=green><list of channelnames></color>\n" +
                "\n" +
                "To switch the active channel\n" +
                "\t<color=#E07B00>\\join</color> <color=green><channelname></color>\n" +
                "\tor\n" +
                "\t<color=#E07B00>\\j</color> <color=green><channelname></color>\n" +
                "\n" +
                "To send a private message: (username are case sensitive)\n" +
                "\t\\<color=#E07B00>msg</color> <color=green><username></color> <color=green><message></color>\n" +
                "\n" +
                "To change status:\n" +
                "\t\\<color=#E07B00>state</color> <color=green><stateIndex></color> <color=green><message></color>\n" +
                "<color=green>0</color> = Offline " +
                "<color=green>1</color> = Invisible " +
                "<color=green>2</color> = Online " +
                "<color=green>3</color> = Away \n" +
                "<color=green>4</color> = Do not disturb " +
                "<color=green>5</color> = Looking For Group " +
                "<color=green>6</color> = Playing" +
                "\n\n" +
                "To clear the current chat tab (private chats get closed):\n" +
                "\t<color=#E07B00>\\clear</color>";


    //
    //
    //
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        this.UserIdText.text = "";
        this.UserIdText.gameObject.SetActive(true);
        this.StateText.text = "";
        this.StateText.gameObject.SetActive(true);
        this.Title.SetActive(true);

        chatClient.MessageLimit = 10;
        //chatChannel.Add();
        
        
    }




    // Update is called once per frame
    void Update()
    {

    }



    public void DebugReturn(DebugLevel level, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnDisconnected()
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

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
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

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

}
