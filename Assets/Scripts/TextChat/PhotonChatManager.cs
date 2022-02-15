using System.Collections;
using System.Collections.Generic;
//unity
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//photon chat
using Photon.Chat;
using Photon.Realtime;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
//pun
using Photon.Pun;
using TMPro;

namespace XReal.Xtown.PhotonChat
{
    public class PhotonChatManager : MonoBehaviourPunCallbacks, IChatClientListener{

        //channels
        //list of channels currently subscribed to
        private readonly List<string> myChannels = new List<string>();
        private readonly List<string> publicChannels = new List<string>();
        private readonly List<string> privateChannels = new List<string>();
        private string selectedChannelName = "Default"; // mainly used for GUI/input
        
        
        // in development
        private readonly List<string> activeUsers = new List<string>();

        //default channels: automatically join on Connect()
        public string[] ChannelsToJoinOnConnect;
        public int HistoryLengthToFetch; 


        
        [Header("Panels")]
        //the ui elements - panels -> set in inspector
        public RectTransform ChatPanel;
        public RectTransform ChannelPanel;   
        public Button ChannelToggle;
        public Text SelectedChannelText;
        public Text ChannelToggleText;
        public Text CurrentRoom;
        public Button AnnounceButton;
        public Text AnnounceText;    

        [Header("UI")]
        //the ui elements - buttons and texts -> set in inspector
        public InputField InputFieldChat; 
        //public InputField InputFieldSendTo;
        //public Button SendButton;
        
        //채팅방 관리 버튼을 만들어뒀는데 세부 채팅 씬 개발을 뒤로 미뤄서 일단 그냥 HideButton이랑 연결시켜둠
        public TextMeshProUGUI CurrentChannelText;
        
        public RoomManager roomManager;
        //This is the Dropdown
        public Dropdown ChannelDropdown;

        private bool isAnnounce = false;
        private string userID;

        ChatClient chatClient;
        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);

            userID = PhotonNetwork.LocalPlayer.NickName;
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
            if(PhotonNetwork.CurrentRoom.PlayerCount>1)
            {
                GetUsersFromDefalut();
            }
            //update가 채널 가입보다 빠르게 시작되어 Default Channel에 가입한 이후부터 동작하도록 만듦
        }
        



        /* UI callbacks */

        // on enter pressed
        void OnEnterSend()
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                SendChatMessage(InputFieldChat.text, selectedChannelName);
                InputFieldChat.text = "";
                //InputFieldSendTo.text = "";
                //InputFieldSendTo는 없애버렸지만 추후에 RoomChat이나 뭐 할 때 살려야 할 수도 있다고 생각해서 구조만 남겨둠
            }
        }


        // 현재 이 함수도 필요가 없고 '채팅방 관리'버튼을 누르면 OnClickHide가 실행됨
        //없어도 되는 함수임
        /*public void OnClickHide()
        {
            if (ChatPanel != null)
            {
                ChatPanel.gameObject.SetActive(false);
                ShowChatHandle.gameObject.SetActive(true);
                Debug.Log(ChatPanel.gameObject.activeSelf);
            }
        }

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

        채팅방을 더블 클릭으로 접었다 폈다 하는건 외부에 추가로 구현해서 OnClickShow는 필요없어짐.
        */

        /* Photon Connect */

        void Connect()
        {
            //set up the chat client
            this.chatClient = new ChatClient(this);
            this.chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
                PhotonNetwork.AppVersion, new AuthenticationValues(userID));

            Debug.Log("ChatManager/Connecting as: " + userID);
        }

        public override void OnJoinedRoom()
        {
            JoinRoomChannel();
            //특정 Room을 떠나고 다른 Room으로 들어갈 때 알아서 구독 해제 됨. 
        }
        

        /* main parsing function */
        // tooo large for a function. split!!!
        // parse the message(including command)


        void SendChatMessage(string inputLine, string channelName)
        {
            //nothing to send
            if (string.IsNullOrEmpty(inputLine))
            {
                return;
            }

            if(isAnnounce)
            {
                isAnnounce = false;
                return;
            }

            //RoomChat구현을 위해선 이 로직을 바꿔야 함.
            bool doingPrivateChat = false;
            if(channelName.Contains(userID))
            {
                doingPrivateChat = true;
            }
            string privateChatTarget = string.Empty;
            if (doingPrivateChat)
            {
                string[] words = channelName.Split(':');
                privateChatTarget = words[1];
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


        public new void OnConnected()
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
            string[] defaultChannel = {"Default"};
            this.chatClient.Unsubscribe(defaultChannel);
            Debug.Log("disconnected");
        }

        //switch to channel selected on dropdown
        public void OnSwitchChannel()
        {
            string channelName = "";
            if(SelectedChannelText.text=="Group Chat   ")
            {
                channelName = ChannelDropdown.options[ChannelDropdown.value].text;
            }
            else
            {
                channelName = userID+":"+ChannelDropdown.options[ChannelDropdown.value].text;
            }
            ShowChannel(channelName);
            selectedChannelName = channelName;
            Debug.Log("채널을 변경했습니다"+channelName);
            //selectedChannelName을 여기에서 바꿔주도록 하고
            CurrentChannelText.text = "";
        }

        //this updates and shows messages buffered on the channel




        public void ShowChannel(string channelName)
        {

            //get channel by name.
            ChatChannel channel = null;
            bool found = this.chatClient.TryGetChannel(channelName, out channel);
            CurrentRoom.text = channelName;
            //AnnounceText.text = channel.Announce;
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
                //InputFieldSendTo.text = subtokens[1];
            }
            
            else
            {
                //InputFieldSendTo.text = "";
            }
            //display message

            
            if(channel.IsPrivate)
            {
                CurrentChannelText.text=" ";
                CurrentChannelText.text = channel.ToStringMessages(userID);
                //OnPrivateMessage만 해도 해당 채널의 모든 메시지를 불러오는 형태임
            }
            else
            {
                Debug.Log("This is Public Chat");
                CurrentChannelText.text = channel.ToStringMessages(userID);
                //글씨 색 버그가 있다.
                //TostringMessage를 아예 ChatChannel에서 바꾸면 수정 가능한데 지금으로썬 할필요 없어짐
            }
            
            Debug.Log("ShowChannel: " + selectedChannelName);
        }


        //ShowChannel도 현재로썬 사용하지 않는 함수임. 구조만 남겨둠




        //get message callbacks
        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            string newMessage = "";
            for (int i = 0; i < messages.Length; i++)
            {
                if(senders[i]==userID)
                {
                    if(channelName=="Default")
                    {
                        newMessage+= "<#CB3A3A>"+"("+senders[i]+")"+": "+messages[i]+"</color>"+"\r\n";
                    }
                    else
                    {
                        newMessage+= "<#CB3A3A>"+"("+senders[i]+")"+": "+messages[i]+"</color>"+"\r\n";
                    }
                }
                else
                {
                    if(channelName=="Default")
                    {
                        newMessage+= "("+senders[i]+")"+": "+messages[i]+"\r\n";
                    }
                    else
                    {
                        newMessage+= "("+senders[i]+"): "+messages[i]+"\r\n"  ;
                    }
                }
            }
            CurrentChannelText.text += newMessage;
        }   //Sender따라 글자 색 구분
            //2월 3일 텍스트 앞에 ChannelName이 나오도록 수정, 이렇게 하고 모든 텍스트는 미니 채팅창에서 보여줌
            //2월 11일 원상복구

        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            //add to channels.
            
            if (!myChannels.Contains(channelName))
            {
                myChannels.Add(channelName);
                privateChannels.Add(channelName);
                Dropdown.OptionData option = new Dropdown.OptionData();
                option.text = channelName;
                ChannelDropdown.options.Add(option);


                if (!activeUsers.Exists(id => id == sender))
                {
                    Debug.Log("user " + sender + " added to local list");
                    return;
                }
            }

            
            if (channelName.Equals(this.selectedChannelName))
            {

                ShowChannel(channelName);
            }
            ChatChannel ch = this.chatClient.PrivateChannels[channelName];
            string newMessage = "";

            foreach (object msg in ch.Messages)
            {
                if(sender==userID)
                {
                    string[] words= channelName.Split(':');
                    newMessage+= "<#CB3A3A>"+"("+sender+"):"+msg+"</color>"+"\r\n";
                }
                else
                {
                    newMessage += "("+sender+"):"+message+"\r\n";
                }
            }

            CurrentChannelText.text = newMessage;
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
                    if(channel.Contains(userID))
                    {
                        privateChannels.Add(channel);
                        if(SelectedChannelText.text=="Personal Chat")
                        {
                            option = new Dropdown.OptionData();
                            option.text = channel;
                            ChannelDropdown.options.Add(option);
                        }
                    }
                    else
                    {
                        publicChannels.Add(channel);
                        if(SelectedChannelText.text=="Group Chat   " || selectedChannelName=="Default")
                        {
                            option = new Dropdown.OptionData();
                            option.text = channel;
                            ChannelDropdown.options.Add(option);
                        }
                    }
                }
                
                //this.chatClient.TryGetChannel(channelName: channel, out ChatChannel newChannel);
                //newChannel.AddSubscriber(userID);


            }
            Debug.Log("OnSubscribed: " + string.Join(", ", channels));
            
            // Switch to the first newly joined channel
            // ShowChannel(channels[0]);    
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
                        //ShowChannel(firstEntry.Current);
                    }

                    //remove both from list and dropdown
                    myChannels.Remove(channelName);
                    ChannelDropdown.options.RemoveAll(item => item.text.Equals(channelName));
                }
                else//trying to unsuscribe to a channel not subscribed to.
                {
                    Debug.Log("Can't unsubscribe from channel '" + channelName + "' because you are currently not subscribed to it.");
                }

                if(channelName.Contains(userID))
                {
                    privateChannels.Remove(channelName);
                }
                else
                {
                    publicChannels.Remove(channelName);
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

        public void GetUsersFromDefalut()
        {
            List<string> playerNameList = roomManager.GetPlayerNameList();
            foreach(string user in playerNameList)
            {
                if(user != userID)
                {
                    string channelName = userID+":"+user;
                    if(!myChannels.Contains(channelName))
                    {
                        myChannels.Add(channelName);
                        privateChannels.Add(channelName);
                        if(SelectedChannelText.text =="Personal Chat")
                        {
                            Dropdown.OptionData option = new Dropdown.OptionData();
                            option.text = user;
                            ChannelDropdown.options.Add(option);
                        }
                    }
                    else
                    {
                        Debug.Log(myChannels);
                    }
                    
                }
                else
                {
                    string channelName = userID+":"+user;
                }
            }
        }

        /*public void RemoveLeavedUsers()
        {
            List<string> playerNameList = roomManager.GetPlayerNameList();
            foreach(string channelName in privateChannels)
            {
                string user = channelName.Split(':')[1];
                if(!playerNameList.Contains(user))
                {
                    myChannels.Remove(channelName);
                    privateChannels.Remove(channelName);
                    if(SelectedChannelText.text =="Personal Chat")
                    {
                        ChannelDropdown.options.RemoveAll(item => item.text.Equals(user));
                    }
                    break;
                }
                else
                {
                    foreach(string user1 in playerNameList)
                    {
                        Debug.Log(user1);
                    }
                }
            }
            OnPlayerLeftRoom으로 일단 구독해제를 만들어놓긴 했는데 예외 케이스 때문에
            이 코드도 남겨두도록 하겠음.
        }*/



        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            //언젠가 써먹을 것 같아서 남겨둠
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            string user = otherPlayer.NickName;
            string channelName = userID+":"+user;
            if(privateChannels.Contains(channelName))
            {
                myChannels.Remove(channelName);
                privateChannels.Remove(channelName);
                if(SelectedChannelText.text =="Personal Chat")
                {
                    ChannelDropdown.options.RemoveAll(item => item.text.Equals(user));
                }
            }
        }

        /*GetUsersFromDefault함수는 Public 채널에서 Subscribers를 가져와서 채널 리스트에 추가해주는 함수임
        이걸 Update함수에 넣어 새 멤버가 들어오면 채널에 자동으로 추가가 됨
        Public 채널에 Subscribers가 자동으로 추가되지는 않는데 특정 채널에 Subscribers를 외부에서 추가할 경우
        버그가 발생함. 그래서 ChatChannel내부에서 메시지 들어올 때 senders를 확인해서 추가하도록 구현함.
        (ChatChannel내 AddSubscriber, AddSubscribers 확인)
        (현재 Public Chat의 경우 구독할 때 기본으로 메시지를 보내기 때문에 구독하자마자 Subscribers에 추가됨)

        현재 privateChat, publicChat의 채널명을 어디에서 설정하는지 몰라서 아직 바꾸지 않았는데
        그래서 모두에게 보내는 채널은 Default, private channel은 내 이름: 상대이름 으로 뜨게 되어있고
        코드는 그걸 기반으로 동작함. 추후에 수정하겠음
        

        아직 상대방이 나갈 때 채널목록에서 자동으로 삭제되지는 않음.
        이건 개개인이 채널하고 연결이 끊길 때 모든 채널의 구독을 해제하도록 만들어야 할 듯함
        ->위의 OnPlayerLeftRoom Callback으로 임시로 해결함.
        */



        public void JoinRoomChannel()
        {
            Dropdown.OptionData option;
            Scene scene = SceneManager.GetActiveScene();
            if(scene.name!="MainRoom" && scene.name!="Lobby")
            {
                if(PhotonNetwork.InRoom)
                {
                    string[] channelName = {PhotonNetwork.CurrentRoom.Name};
                    if(!myChannels.Contains(channelName[0]))
                    {
                        this.chatClient.Subscribe(channelName,0);
                        myChannels.Add(channelName[0]);
                        publicChannels.Add(channelName[0]);
                        if(SelectedChannelText.text == "Group Chat   ")
                        {
                            option = new Dropdown.OptionData();
                            option.text = channelName[0];
                            ChannelDropdown.options.Add(option);
                        }
                        //뒤에 숫자 0으로 그냥 써놔도 무방한지 확실히하기
                    }
                }
                else
                {
                    string[] channelName = {scene.name};
                    if(!myChannels.Contains(channelName[0]))
                    {
                        this.chatClient.Subscribe(channelName,0);
                        myChannels.Add(channelName[0]);
                        publicChannels.Add(channelName[0]);
                        if(SelectedChannelText.text == "Group Chat   ")
                        {
                            option = new Dropdown.OptionData();
                            option.text = channelName[0];
                            ChannelDropdown.options.Add(option);
                        }
                        //뒤에 숫자 0으로 그냥 써놔도 무방한지 확실히하기
                    }
                }
            }
        }


        public void ChangeChannel()
        {
            Dropdown.OptionData option;
            if(ChannelToggleText.text=="Personal Chat")
            {
                SelectedChannelText.text = "Personal Chat";
                ChannelToggleText.text = "Group Chat   ";
                foreach(string channelName in publicChannels)
                {
                    ChannelDropdown.options.RemoveAll(item => item.text.Equals(channelName));
                }
                foreach(string channelName in privateChannels)
                {
                    option = new Dropdown.OptionData();
                    string[] channelNameParse = channelName.Split(':');
                    option.text = channelNameParse[1];
                    ChannelDropdown.options.Add(option);
                }
                if(privateChannels.Count>0)
                {
                    this.chatClient.Subscribe(privateChannels[0]);
                    ShowChannel(privateChannels[0]);
                    selectedChannelName = privateChannels[0];
                    Debug.Log("채널을 변경했습니다"+privateChannels[0]);
                }
                
            }
            else
            {
                SelectedChannelText.text = "Group Chat   ";
                ChannelToggleText.text = "Personal Chat";
                foreach(string channelName in privateChannels)
                {
                    string[] channelNameParse = channelName.Split(':');
                    ChannelDropdown.options.RemoveAll(item => item.text.Equals(channelNameParse[1]));
                }
                foreach(string channelName in publicChannels)
                {
                    option = new Dropdown.OptionData();
                    option.text = channelName;
                    ChannelDropdown.options.Add(option);
                }
                ShowChannel("Default");
                selectedChannelName = "Default";
            }
        }

        public void ChangeAnnounce()
        {
            if(isAnnounce == true)
            {
                isAnnounce = false;
                ColorBlock cb = AnnounceButton.colors;
                Color newColor = new Color32(245, 245, 245, 225);
                cb.normalColor = newColor;
                cb.highlightedColor = newColor;
                cb.pressedColor = newColor;
                cb.disabledColor = newColor;
                cb.selectedColor = newColor;
                AnnounceButton.colors = cb;


            }
            else
            {
                isAnnounce = true;
                ColorBlock cb = AnnounceButton.colors;
                Color newColor = new Color32(69, 199, 247, 225);
                cb.normalColor = newColor;
                cb.highlightedColor = newColor;
                cb.pressedColor = newColor;
                cb.disabledColor = newColor;
                cb.selectedColor = newColor;
                AnnounceButton.colors = cb;
            }
        }
        


        //UnSubscribe하는 부분을 구현하지 않았는데 그냥 방 나오면 자동으로 채널 사라짐. 씬이 바뀌면서 채널 연결을 다시 해서 그렇게 되는 것인듯?
        //당장은 괜찮지만 버그가 없는지 찾아봐야 할 것 같다.

        












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
