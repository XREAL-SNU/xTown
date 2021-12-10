using System.Collections;
using System.Collections.Generic;
//unity
using UnityEngine;
using UnityEngine.UI;
//photon chat
using Photon.Chat;
using Photon.Realtime;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
//pun
using Photon.Pun;

/*
 * Contributors:
 * Drafted by Hanjun Kim 2021. 12. 05
 */


namespace PhotonTextChat
{
    public class PhotonChatManager : MonoBehaviour, IChatClientListener {

        //channels
        //list of channels currently subscribed to
        private readonly List<string> myChannels = new List<string>();
        private string selectedChannelName; // mainly used for GUI/input

        //default channels: automatically join on Connect()
        public string[] ChannelsToJoinOnConnect;
        public int HistoryLengthToFetch; // # previously sent messages that can be fetched for context
        public string UserName;

        
        [Header("Panels")]
        //the ui elements - panels -> set in inspector
        public RectTransform ChatPanel;    
        public GameObject UsernamePanel;

        [Header("UI")]
        //the ui elements - buttons and texts -> set in inspector
        public InputField InputFieldChat; 
        public InputField InputFieldSendTo;
        public Button SendButton;
        public Text CurrentChannelText;

        //set in inspector!
        [SerializeField] 
        string userID;

        ChatClient chatClient;
        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            UsernamePanel.SetActive(false);
            if (string.IsNullOrEmpty(this.UserName))
            {
                //made-up username
                this.UserName = "default user";
            }

            //add listeners to input field and send button
            InputFieldChat.onEndEdit.AddListener(delegate { OnEnterSend(); });
            SendButton.onClick.AddListener(delegate { OnClickSend(); });
        
            //connect on start
            Connect();
        }

        void Connect()
        {   
            //set up the chat client
            this.chatClient = new ChatClient(this);
            this.chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
                PhotonNetwork.AppVersion, new AuthenticationValues(userID));

            Debug.Log("Connecting as: " + this.UserName);
        }


        // Update is called once per frame
        void Update()
        {
            if (this.chatClient != null)
            {
                //check for new messages: call every frame
                this.chatClient.Service();
            }
        }

        //send callbacks
        //on enter pressed
        void OnEnterSend()
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                Debug.Log("Enter clicked");
                this.SendChatMessage(this.InputFieldChat.text, this.InputFieldSendTo.text);
                this.InputFieldChat.text = "";
            }
        }

        //on send button clicked
        void OnClickSend()
        {
            if (this.InputFieldChat != null)
            {
                this.SendChatMessage(this.InputFieldChat.text, this.InputFieldSendTo.text);
                this.InputFieldChat.text = "";
            }
        }

        //parse the message(including command)
        void SendChatMessage(string inputLine, string toUser)
        {
            //nothing to send
            if (string.IsNullOrEmpty(inputLine))
            {
                return;
            }

            //is this message private?
            bool doingPrivateChat = this.chatClient.PrivateChannels.ContainsKey(this.selectedChannelName);
            string privateChatTarget = string.Empty;
            if (doingPrivateChat)
            {
                // the channel name for a private is in form:  user1 : user2
                //we need the user2 part.
                string[] splitNames = this.selectedChannelName.Split(new char[] { ':' });
                privateChatTarget = splitNames[1];
            }

            if (inputLine[0].Equals('\\')) //commands begin with character '\'
            {
                string[] tokens = inputLine.Split(new char[] { ' ' }, 2);
                
                //to subscribe to a channel: type "\subscribe <channel name>
                if ((tokens[0].Equals("\\subscribe") || tokens[0].Equals("\\s")) && !string.IsNullOrEmpty(tokens[1]))
                {
                    this.chatClient.Subscribe(tokens[1].Split(new char[] { ' ', ',' }));
                }
                //to unsubscribe a channel: type "\unsubscribe <channel name>
                else if ((tokens[0].Equals("\\unsubscribe") || tokens[0].Equals("\\u")) && !string.IsNullOrEmpty(tokens[1]))
                {
                    this.chatClient.Unsubscribe(tokens[1].Split(new char[] { ' ', ',' }));
                }
                //clearing channel - delete all messages (testing purpose)
                else if (tokens[0].Equals("\\clear"))
                {
                    if (doingPrivateChat)
                    {
                        this.chatClient.PrivateChannels.Remove(this.selectedChannelName);
                    }
                    else //public chat
                    {
                        ChatChannel channel;
                        if (this.chatClient.TryGetChannel(this.selectedChannelName, doingPrivateChat, out channel))
                        {
                            channel.ClearMessages();
                        }
                    }
                }
                //join channel
                else if ((tokens[0].Equals("\\join") || tokens[0].Equals("\\j")) && !string.IsNullOrEmpty(tokens[1]))
                {
                    string[] subtokens = tokens[1].Split(new char[] { ' ', ',' }, 2);

                    //already subscribed to the channel :switch to it
                    if (this.myChannels.Contains(subtokens[0]))
                    {
                        this.ShowChannel(subtokens[0]);
                    }
                    else //not yet joined - create a new channel and enter
                    {
                        this.chatClient.Subscribe(new string[] { subtokens[0] });
                    }
                }
                else //invalid input
                {
                    Debug.Log("The command '" + tokens[0] + "' is invalid.");
                }
            }
            else //messages
            {
                if (doingPrivateChat)
                {
                    Debug.Log("sending private message" + inputLine);
                    this.chatClient.SendPrivateMessage(privateChatTarget, inputLine);
                }
                else
                {
                    Debug.Log("sending public message" + inputLine);
                    this.chatClient.PublishMessage(this.selectedChannelName, inputLine);
                }
            }
        }


        public void OnConnected()
        {
            //automatic subscription to default channels.
            if (this.ChannelsToJoinOnConnect != null && this.ChannelsToJoinOnConnect.Length > 0)
            {
                this.chatClient.Subscribe(this.ChannelsToJoinOnConnect, this.HistoryLengthToFetch);
            }

            Debug.Log( "Connected as " + this.UserName );
            this.ChatPanel.gameObject.SetActive(true);

        }

        public void OnDisconnected()
        {
            Debug.Log("disconnected");
        }

        //this shows messages buffered on the channel
        public void ShowChannel(string channelName)
        {

            //get channel by name.
            ChatChannel channel = null;
            bool found = this.chatClient.TryGetChannel(channelName, out channel);
            if (!found)
            {
                Debug.Log("ShowChannel failed to find channel: " + channelName);
                return;
            }

            //switch to this channel
            this.selectedChannelName = channelName;

            //display message
            this.CurrentChannelText.text = channel.ToStringMessages();
            Debug.Log("ShowChannel: " + this.selectedChannelName);

        }

        //get message callbacks
        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            if (channelName.Equals(this.selectedChannelName))
            {
                this.ShowChannel(this.selectedChannelName);
            }
        }

        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            if (channelName.Equals(this.selectedChannelName))
            {
                this.ShowChannel(channelName);
            }
        }



        //subscription callbacks
        public void OnSubscribed(string[] channels, bool[] results)
        {
            //notify entry on each channel
            foreach (string channel in channels)
            {
                this.chatClient.PublishMessage(channel, "says 'hi'.");
                //add to my channels
                if(!myChannels.Contains(channel)) this.myChannels.Add(channel);
            }
            Debug.Log("OnSubscribed: " + string.Join(", ", channels));
            
            // Switch to the first newly joined channel
            this.ShowChannel(channels[0]);
        }

        public void OnUnsubscribed(string[] channels)
        {
            foreach (string channelName in channels)
            {
                if (this.myChannels.Contains(channelName))//if i'm subscribed to the channel
                {
                    Debug.Log("Unsubscribed from channel '" + channelName + "'.");

                    // Showing another channel if the active channel is the one we unsubscribed from
                    if (channelName == this.selectedChannelName && this.myChannels.Count > 0)
                    {
                        //get enumerator(like cpp iterator) to the first in the channels.
                        IEnumerator<string> firstEntry = this.myChannels.GetEnumerator();
                        firstEntry.MoveNext();
                        //IEnumerator.Current gets the element pointed to by the enumerator in the collection
                        this.ShowChannel(firstEntry.Current);
                    }
                }
                else//trying to unsuscribe to a channel not subscribed to.
                {
                    Debug.Log("Can't unsubscribe from channel '" + channelName + "' because you are currently not subscribed to it.");
                }
            }
        }



        //more code(requires implementation by IChatClientListener interface)

        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {
            throw new System.NotImplementedException();
        }
        public void OnUserSubscribed(string channel, string user)
        {
            Debug.LogFormat("OnUserSubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
        }

        public void OnUserUnsubscribed(string channel, string user)
        {
            Debug.LogFormat("OnUserUnsubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
        }

        public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
        {
            Debug.Log(message);
        }

        public void OnChatStateChange(ChatState state)
        {
            Debug.Log(state);
        }

        public void OnDestroy()
        {
            if (this.chatClient != null)
            {
                this.chatClient.Disconnect();
            }
        }

        public void OnApplicationQuit()
        {
            if (this.chatClient != null)
            {
                this.chatClient.Disconnect();
            }
        }

    }

}
