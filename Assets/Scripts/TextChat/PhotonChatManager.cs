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
using TMPro;

namespace XReal.Xtown.PhotonChat
{
    public class PhotonChatManager : MonoBehaviour, IChatClientListener {

        //channels
        //list of channels currently subscribed to
        private readonly List<string> myChannels = new List<string>();
        private string selectedChannelName; // mainly used for GUI/input
        

        // in development
        private readonly List<string> activeUsers = new List<string>();

        //default channels: automatically join on Connect()
        public string[] ChannelsToJoinOnConnect;
        public int HistoryLengthToFetch; 


        
        [Header("Panels")]
        //the ui elements - panels -> set in inspector
        public RectTransform ChatPanel;    
        public Button ShowChatHandle;    

        [Header("UI")]
        //the ui elements - buttons and texts -> set in inspector
        public InputField InputFieldChat; 
        public InputField InputFieldSendTo;
        //public Button SendButton;
        public Button HideButton;
        //채팅방 관리 버튼을 만들어뒀는데 세부 채팅 씬 개발을 뒤로 미뤄서 일단 그냥 HideButton이랑 연결시켜둠
        public TextMeshProUGUI CurrentChannelText;
        

        //This is the Dropdown
        public Dropdown ChannelDropdown;

        
        private string userID;

        ChatClient chatClient;
        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);

            userID = PhotonNetwork.LocalPlayer.NickName;
            //이걸 PhotonNetWork.Nickname으로 바꾸니까 채팅이 두번씩 쳐짐... 뭐지?
            InputFieldChat.onEndEdit.AddListener(delegate { OnEnterSend(); });
            //SendButton.onClick.AddListener(delegate { OnClickSend(); });
            ChannelDropdown.ClearOptions();
            ChannelDropdown.onValueChanged.AddListener(delegate { OnSwitchChannel(); });
            ChatPanel.gameObject.SetActive(false);
            //connect on start
            Connect();
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

        /* UI callbacks */

        // on enter pressed
        void OnEnterSend()
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                SendChatMessage(InputFieldChat.text, InputFieldSendTo.text);
                InputFieldChat.text = "";
                //InputFieldSendTo.text = ""; 채널을 선택했을 때 메시지를 보내도 input field는 초기화되지 않게 함
            }
        }

        // onClicksend는 지워버렸지만 혹시 언젠가 쓸모가 있을까 싶어 주석으로 남겨둠...
        //void OnClickSend()
        //{
        //    if (InputFieldChat != null)
        //    {
        //        SendChatMessage(InputFieldChat.text, InputFieldSendTo.text);
        //        InputFieldChat.text = "";
                //InputFieldSendTo.text = "";
        //    }
        //}



        // 현재 이 함수도 필요가 없고 '채팅방 관리'버튼을 누르면 OnClickHide가 실행됨
        //없어도 되는 함수임
        public void OnClickHide()
        {
            if (ChatPanel != null)
            {
                ChatPanel.gameObject.SetActive(false);
                ShowChatHandle.gameObject.SetActive(true);
                Debug.Log(ChatPanel.gameObject.activeSelf);
            }
        }

        // onClickShow에서 버튼 하나로 채팅방을 접었다 폈다 할 수 있게 만듦
        public void OnClickShow()
        {
            if (ChatPanel != null)
            {
                if(ChatPanel.gameObject.activeSelf==true)
                {
                    ChatPanel.gameObject.SetActive(false);
                    Debug.Log(ChatPanel.gameObject.activeSelf);
                }
                else if(ChatPanel.gameObject.activeSelf==false)
                {
                    ChatPanel.gameObject.SetActive(true);
                    Debug.Log(ChatPanel.gameObject.activeSelf);
                }
            }
        }
        

        /* Photon Connect */

        void Connect()
        {
            //set up the chat client
            this.chatClient = new ChatClient(this);
            this.chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
                PhotonNetwork.AppVersion, new AuthenticationValues(userID));

            Debug.Log("ChatManager/Connecting as: " + userID);
        }


        /* main parsing function */
        // tooo large for a function. split!!!
        // parse the message(including command)
        void SendChatMessage(string inputLine, string toUser)
        {
            //nothing to send
            if (string.IsNullOrEmpty(inputLine))
            {
                return;
            }

            //is this message private?
            bool doingPrivateChat = !string.IsNullOrEmpty(toUser);
            string privateChatTarget = string.Empty;
            if (doingPrivateChat)
            {
                privateChatTarget = toUser;
            }

            if (inputLine[0].Equals('\\')) //commands begin with character '\'
            {
                string[] tokens = inputLine.Split(new char[] { ' ' }, 2);
                
                //새로운 채널을 구독하는 방식
                if ((tokens[0].Equals("\\subscribe") || tokens[0].Equals("\\s")) && !string.IsNullOrEmpty(tokens[1]))
                {
                    this.chatClient.Subscribe(tokens[1].Split(new char[] { ' ', ',' }));
                }
                //to unsubscribe a channel: type "\unsubscribe <channel name>
                else if ((tokens[0].Equals("\\unsubscribe") || tokens[0].Equals("\\u")) && !string.IsNullOrEmpty(tokens[1]))
                {
                    this.chatClient.Unsubscribe(tokens[1].Split(new char[] { ' ', ',' }));
                }
                //clearing channel - renove channel / for testing only
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
                    if (myChannels.Contains(subtokens[0]))
                    {
                        ShowChannel(subtokens[0]);
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
            else //send messages
            {
                if (doingPrivateChat)
                {
                    Debug.Log("sending private message to " + privateChatTarget);
                    this.chatClient.SendPrivateMessage(privateChatTarget, inputLine);
                }
                else
                {
                    Debug.Log("sending public message" + inputLine);
                    Debug.Log(selectedChannelName);
                    this.chatClient.PublishMessage(this.selectedChannelName, inputLine);
                }
            }
        }

        public void SendHello(string toID)
        {
            this.chatClient.SendPrivateMessage(toID, "user " + userID + " says hi");
        }

        /* Photon Callbacks */
        public void OnConnected()
        {
            //automatic subscription to default channels.
            if (this.ChannelsToJoinOnConnect != null && this.ChannelsToJoinOnConnect.Length > 0)
            {
                this.chatClient.Subscribe(this.ChannelsToJoinOnConnect, this.HistoryLengthToFetch);
            }
            
            Debug.Log( "Connected as " + this.userID );
            //this.ChatPanel.gameObject.SetActive(true);
            //이거 주석 풀면 들어가자마자 채팅방이 활성화 되는데 접혀있는게 낫겠다 생각해서 없애버림
        }

        public void OnDisconnected()
        {
            Debug.Log("disconnected");
        }

        //switch to channel selected on dropdown
        public void OnSwitchChannel()
        {
            ShowChannel(ChannelDropdown.options[ChannelDropdown.value].text);
            //
        }

        //this updates and shows messages buffered on the channel
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
            selectedChannelName = channelName;

            //update selection (dropdown)
            for (int i = 0; i < ChannelDropdown.options.Count; ++i)
            {
                if (ChannelDropdown.options[i].Equals(channelName)) ChannelDropdown.value = i;
            }

            // display private hints
            string[] subtokens = channelName.Split(':');
            if(subtokens[0] == userID)
            {
                InputFieldSendTo.text = subtokens[1];
            }
            
            else
            {
                InputFieldSendTo.text = "";
            }
            //display message

            
            if(channel.IsPrivate)
            {
                foreach(object msg in channel.Messages)
                {
                    Debug.Log(msg);
                }
                OnPrivateMessage(channel.Senders[0], channel.Messages, channel.Name);
                //OnPrivateMessage만 해도 해당 채널의 모든 메시지를 불러오는 형태임
            }
            else
            {
                Debug.Log("This is Public Chat");
                CurrentChannelText.text = channel.ToStringMessages();
                //다른 채널을 들어갔다가 public channel로 돌아올 때
                //대화 내용을 유지하면서 내 글씨 색과 상대방의 글씨 색을 구분해주는건 아직 구현하지 못함
                //private message와 마찬가지로 channel안의 msg를 그대로 받아와 for문을 돌리면 될 것 같은데
                //둘이 메시지 저장하는 방식이 약간 달라서 일단 이대로 함.
                // 추후에 agora baseline 구현과 동시에 오류 수정하고 다시 업데이트함. 
            }
            
            Debug.Log("ShowChannel: " + selectedChannelName);

        }


        //get message callbacks
        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            string newMessage = "";
            for (int i = 0; i < messages.Length; i++)
            {
                if(senders[i]==userID)
                {
                    newMessage+= "<#CB3A3A>"+senders[i]+":"+messages[i]+"</color>"+"\r\n";
                }
                else
                {
                    newMessage+= senders[i]+":"+messages[i]+"\r\n";
                }
                CurrentChannelText.text += newMessage;
            }
        }   //Sender로 구분해 senders와 userID가 동일하면 글자를 빨간 색으로 표시하도록 함
            //이 방식을 통해 내 화면에서만 글자가 빨간색으로 나옴

        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            //add to channels.
            if (!myChannels.Contains(channelName))
            {
                myChannels.Add(channelName);

                Dropdown.OptionData option = new Dropdown.OptionData();
                option.text = channelName;
                ChannelDropdown.options.Add(option);


                if (!activeUsers.Exists(id => id == sender))
                {
                    Debug.Log("user " + sender + " added to local list");
                    return;
                }
            }

            
            //if (channelName.Equals(this.selectedChannelName))
            //{
            //    ShowChannel(channelName);
            //}
            ChatChannel ch = this.chatClient.PrivateChannels[channelName];
            string newMessage = "";
            foreach (object msg in ch.Messages)
            {
                if(sender==userID)
                {
                    newMessage+= "<#CB3A3A>"+sender+":"+msg+"</color>"+"\r\n";
                }
                else
                {
                    newMessage+= sender+":"+msg+"\r\n";
                }
                //Sender로 구분해 내가 보낸 메시지일 경우 색을 바꿔주도록 함
                //OnPrivateMessage가 실행될때마다 채널에서 메시지 전체를 다시 가져오는 형식으로 구현되어있는데
                //사용자 색 구분하면서 어떤 오류가 발생해서 이렇게 짜 두었음.
                //이 경우도 메시지가 많을수록 효율이 떨어지니 이유가 기억나면 다시 개선해보겠습니다.
                //일단은 제대로 돌아가는지 위주로 확인해주세요...ㅎ
            }
            CurrentChannelText.text = newMessage;
            foreach(string user in ch.Subscribers)
            {
                Debug.Log(user);
            }
        }   



        //subscription callbacks
        public void OnSubscribed(string[] channels, bool[] results)
        {
            Dropdown.OptionData option;
            //notify entry on each channel
            foreach (string channel in channels)
            {
                string msg = userID + " subscribed to channel";
                this.chatClient.PublishMessage(channel, msg);

                //add to my channels
                if (!myChannels.Contains(channel))
                {
                    myChannels.Add(channel);
                    option = new Dropdown.OptionData();
                    option.text = channel;
                    ChannelDropdown.options.Add(option);
                }
            }
            Debug.Log("OnSubscribed: " + string.Join(", ", channels));
            
            // Switch to the first newly joined channel
            ShowChannel(channels[0]);
            
        }

        public void OnUnsubscribed(string[] channels)
        {
            foreach (string channelName in channels)
            {
                if (myChannels.Contains(channelName))//if i'm subscribed to the channel
                {
                    Debug.Log("Unsubscribed from channel '" + channelName + "'.");

                    // Showing another channel if the active channel is the one we unsubscribed from
                    if (channelName == selectedChannelName && myChannels.Count > 0)
                    {
                        //get enumerator(like cpp iterator) to the first in the channels.
                        IEnumerator<string> firstEntry = myChannels.GetEnumerator();
                        firstEntry.MoveNext();
                        //IEnumerator.Current gets the element pointed to by the enumerator in the collection
                        ShowChannel(firstEntry.Current);
                    }

                    //remove both from list and dropdown
                    myChannels.Remove(channelName);
                    ChannelDropdown.options.RemoveAll(item => item.text.Equals(channelName));
                }
                else//trying to unsuscribe to a channel not subscribed to.
                {
                    Debug.Log("Can't unsubscribe from channel '" + channelName + "' because you are currently not subscribed to it.");
                }
            }

        }

        public void OnUserSubscribed(string channel, string user)
        {
            Debug.LogFormat("OnUserSubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
            activeUsers.Add(user);
            SendHello(user);
        }

        public void OnUserUnsubscribed(string channel, string user)
        {
            Debug.LogFormat("OnUserUnsubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
            activeUsers.Remove(user);

        }


        //more code(requires implementation by IChatClientListener interface)

        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {
            throw new System.NotImplementedException();
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

        /*void CreateNewChannel(string channelName, int maxNumber, string[] users)
        {
            if(!myChannels.Contains(channelName))
            {
                myChannels.Add(channelName);
                Dropdown.OptionData option = new Dropdown.OptionData();
                option.text = channelName;
                ChannelDropdown.options.Add(option);
            }

            ChatChannel channel = null;
            bool found = this.chatClient.TryGetChannel(channelName, out channel);
            channel.MaxSubscribers = maxNumber;
            if(found && users.Length<=maxNumber)
            {
                foreach(string user in users)
                {
                    channel.Subscribers.Add(user);
                }
            }
            else
            {
                Debug.LogError("Error occurs");
                //일단 이렇게 써놨는데 아마 Max유저보다 덜
            }
            
            //photon ChatChannel에서 MaxSubscirbers를 수정함
            this.chatClient.PublishMessage(channelName, "New chat room is created");

        }

        void JoinNewChannel(string channelName)
        {
            ChatChannel channel = null;
            bool found = this.chatClient.TryGetChannel(channelName, out channel);
            if(found)
            {
                
            }
        }

        이 코드는 룸챗내의 로직에 대해 작성하고 있던 부분인데 해당 개발계획이 뒤로 미뤄져 주석처리해둠.
        별개의 채팅방을 생성하고
        
        인원수에 맞춰서 초대할 수 있는 단체 채팅방을 만들고 있었는데 이게 미니 챗에 필요한 기능이라는
        생각이 들면 이 기능 역시 추가하도록 하겠음.

        (룸 안에 모인 사람들끼리 대화하는 채팅방을 따로 파려면 역시 이 기능이 필요할 것으로 보임)
        이 코드를 동작시키기 위해선 ChatChannel에서 MaxSubscriber를 public set으로 바꿔줘야 하는데
        이번에 pull request할 때는 해당 부분을 수정하지 않은 상태로 진행해
        당장은 주석처리 풀어서 실행시키면 오류가 날 것

        */
    }   

}
