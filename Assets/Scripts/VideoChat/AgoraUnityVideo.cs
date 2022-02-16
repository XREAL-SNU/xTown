using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using agora_gaming_rtc;
using agora_utilities;
using Photon.Pun;
#if(UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif

// this is an example of using Agora Unity SDK
// It demonstrates:
// How to enable video
// How to join/leave channel
// 
public class AgoraUnityVideo: MonoBehaviour
{

    // instance of agora engine
    private IRtcEngine mRtcEngine;
    private UserInfo userInfo;
    private UserInfo remoteUserInfo;
    private Text NickName;
    private Text remoteUserNickNameText;
    private Text MessageText;
    private string staticChannelName = "Xreal";
    private string AppID;
    // a token is a channel key that works with a AppID that requires it. 
    // Generate one by your token server or get a temporary token from the developer console
    public string token = "00667cb00c63d4341ec8aba2d7de7283bbdIAByIMR2DG5wc4BUWbvF5nKkQ/2T4Y6LqpOXolN8kv6mc15liXkAAAAAEADzxwcSvskMYgEAAQC/yQxi";
    public List<uint> uidList = new List<uint>();
    //token이 만료가 되는걸 어떻게 방지하는지? 혹은 아예 token없이 통신해야 하는지
    // load agora engine
    float xPos = -220;
    float yPos = 110;
    public int channelJoined = 1;
    int panelNum = 1;
    int currentPage = 1;
    void Awake()
        {
    #if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
            permissionList.Add(Permission.Microphone);         
            permissionList.Add(Permission.Camera);               
    #endif
        }


    
    public void loadEngine(string appId)
    {
        // start sdk
        Debug.Log("initializeEngine");
        AppID = appId;
        if (mRtcEngine != null)
        {
            Debug.Log("Engine exists. Please unload it first!");
            return;
        }

        // init engine
        mRtcEngine = IRtcEngine.GetEngine(appId);

        // enable log
        mRtcEngine.SetLogFilter(LOG_FILTER.DEBUG | LOG_FILTER.INFO | LOG_FILTER.WARNING | LOG_FILTER.ERROR | LOG_FILTER.CRITICAL);
    }

    public void join(string channel)
    {
        Debug.Log("calling join (channel = " + channel + ")");
        userInfo.userAccount = PhotonNetwork.NickName;
        if (mRtcEngine == null)
            return;

        // set callbacks (optional)
        mRtcEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
        mRtcEngine.OnUserJoined = onUserJoined;
        mRtcEngine.OnUserOffline = onUserOffline;
        mRtcEngine.OnRequestToken = OnRequestToken;
        mRtcEngine.OnUserMutedAudio = OnUserMutedAudio;
        mRtcEngine.OnUserMuteVideo = OnUserMutedVideo;
        mRtcEngine.OnWarning = (int warn, string msg) =>
        {
            Debug.LogWarningFormat("Warning code:{0} msg:{1}", warn, IRtcEngine.GetErrorDescription(warn));
        };
        mRtcEngine.OnError = HandleError;
        

        // enable video
        mRtcEngine.EnableVideo();
        // allow camera output callback
        mRtcEngine.EnableVideoObserver();

        // join channel
        /*  This API Assumes the use of a test-mode AppID
             mRtcEngine.JoinChannel(channel, null, 0);
        */

        /*  This API Accepts AppID with token; by default omiting info and use 0 as the local user id */
        int i = mRtcEngine.RegisterLocalUserAccount(AppID, PhotonNetwork.NickName);
        mRtcEngine.JoinChannelWithUserAccount(token, channel, PhotonNetwork.NickName);
        Debug.Log("RegisterUserAccount Result"+i);
        //JoinChannelByKey(channelKey: token, channelName: channel);

    }


    public string getSdkVersion()
    {
        string ver = IRtcEngine.GetSdkVersion();
        return ver;
    }






    public void leave()
    {
        Debug.Log("calling leave");

        if (mRtcEngine == null)
            return;

        // leave channel
        mRtcEngine.LeaveChannel();
        // deregister video frame observers in native-c code
        mRtcEngine.DisableVideoObserver();
        //연결을 끊으면서 channelJoined를 하나 줄임.
    }





    // unload agora engine
    public void unloadEngine()
    {
        Debug.Log("calling unloadEngine");

        // delete
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();  // Place this call in ApplicationQuit
            mRtcEngine = null;
        }
    }




    public void EnableVideo(bool pauseVideo)
    {
        if (mRtcEngine != null)
        {
            GameObject screen = GameObject.Find("Screen");
            VideoSurface vs = screen.GetComponent<VideoSurface>();
            if(pauseVideo)
            {
                vs.SetEnable(false);
                mRtcEngine.MuteLocalVideoStream(true);
            }
            else
            {
                vs.SetEnable(true);
                mRtcEngine.MuteLocalVideoStream(false);
            }
        }
    }

    public void EnableAudio(bool muteAudio)
    {
        if (mRtcEngine != null)
        {
            if(muteAudio)
            {
                mRtcEngine.MuteLocalAudioStream(true);
            }
            else
            {
                mRtcEngine.MuteLocalAudioStream(false);
            }
        }
    }





    // accessing GameObject in Scnene1
    // set video transform delegate for statically created GameObject
    public void onSceneHelloVideoLoaded()
    {

        GameObject screen = GameObject.Find("Screen");
        if(ReferenceEquals(screen, null))
        {
            Debug.Log("failed to find screen");
            return;
        }
        else
        {
            screen.AddComponent<VideoSurface>();
        }



        /*GameObject text = GameObject.Find("MessageText");
        if (!ReferenceEquals(text, null))
        {
            MessageText = text.GetComponent<Text>();
        }

        GameObject bobj = GameObject.Find("HelpButton");
        if (bobj != null)
        {
            Button button = bobj.GetComponent<Button>();
            if (button!=null)
            {
                button.onClick.AddListener(HandleHelp);
	        }
	    }*/

        GameObject NickNameField = screen.transform.Find("NickNameField").gameObject;
        GameObject text = NickNameField.transform.Find("MyNickName").gameObject;
        if(!ReferenceEquals(text, null))
        {
            NickName = text.GetComponent<Text>();
            NickName.text = PhotonNetwork.NickName;
        }
    }





    void HandleHelp()
    {
#if UNITY_2020_3_OR_NEWER && PLATFORM_STANDALONE_OSX
        // this very easy to forget for MacOS
        HandleError(-2, "if you don't see any video, did you set the MacOS plugin bundle to AnyCPU?");
#else
        HandleError(-1, "if you don't see any video, please check README for help");
#endif
    }

    // implement engine callbacks
    private void onJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {
        Debug.Log("JoinChannelSuccessHandler: uid = " + uid);
        
        //기본적으로 Room에 있는 모든 사람들이 channelJoined를 PunRPC로 공유하도록 할 것
        //본인이 들어올 때는 channelJoined를 하나 늘리면서 들어옴.
        //GameObject textVersionGameObject = GameObject.Find("VersionText");
        //textVersionGameObject.GetComponent<Text>().text = "SDK Version : " + getSdkVersion();
    }







    // When a remote user joined, this delegate will be called. Typically
    // create a GameObject to render video on it
    //다른 유저가 들어올 때는 makeImageSurface가 작동한다.
    private void onUserJoined(uint uid, int elapsed)
    {
        SetButtonActive();
        Debug.Log("onUserJoined: uid = " + uid + " elapsed = " + elapsed);
        channelJoined+=1;
        SetButtonActive();

        UserInfo newUserInfo = mRtcEngine.GetUserInfoByUid(uid);
        Debug.Log("유저이름: "+newUserInfo.userAccount + "UID: "+newUserInfo.uid.ToString());
        // this is called in main thread
        
        //일단 이걸로 버튼 active해야하는지부터 확인함
        // find a game object to render video stream from 'uid'
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            return; // reuse
        }

        // create a GameObject and assign to this new user
        
        int num = channelJoined%6;
        //여기에서 channelJoined가 6명 이상이면 새 패널을 만들고 거기에 VideoSurface를 추가해줌
        //버튼은 그냥 사람수보고 위에서 Active한다
        if (num==1 && channelJoined>1)
        {
            Debug.Log("패널이 생성되는지 확인하기");
            panelNum+=1;
            GameObject panel = new GameObject("VideoPanel"+panelNum.ToString());
            
            panel.AddComponent<CanvasRenderer>();
            panel.AddComponent<RectTransform>();
            RectTransform rt = panel.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector3(80, 45, 0);
            GameObject VideoCanvas = GameObject.Find("VideoCanvas");
            if(VideoCanvas!=null)
            {
                panel.transform.parent = VideoCanvas.transform;
            }
            


        }
        VideoSurface videoSurface = makeImageSurface(uid.ToString());
        //makePlaneSurface("Plane");
        if (!ReferenceEquals(videoSurface, null))
        {
            // configure videoSurface
            videoSurface.SetForUser(uid);
            videoSurface.SetEnable(true);
            videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
        }

        GameObject remoteUserNickName = GameObject.Find(uid.ToString()).transform.Find("NickNameField").transform.Find("MyNickName").gameObject;
        if(!ReferenceEquals(remoteUserNickName, null))
        {
            remoteUserNickNameText = remoteUserNickName.GetComponent<Text>();
            remoteUserInfo = mRtcEngine.GetUserInfoByUid(uid);
            remoteUserNickNameText.text = remoteUserInfo.userAccount;
        }
        uidList.Add(uid);
        Debug.Log(channelJoined+"ChannelJoined");
    }



    public void SetButtonActive()
    {

        GameObject LeftButton = GameObject.Find("VideoCanvas").transform.Find("LeftButton").gameObject;
        GameObject RightButton = GameObject.Find("VideoCanvas").transform.Find("RightButton").gameObject;


        if(currentPage==1)
        {   
            //첫 번째 페이지
            LeftButton.SetActive(false);
            if(channelJoined>6)
            {
                RightButton.SetActive(true);
            }
        }
        else if(currentPage==panelNum)
        {
            //마지막 페이지
            RightButton.SetActive(false);
            LeftButton.SetActive(true);
            //else if이므로 첫 번째 페이지 일 때는 작동X
            //마지막 페이지에서는 반드시 LeftButton만 활성화 되어있을 것
        }
        else
        {
            //첫 번째도, 마지막도 아닌 중간 페이지
            RightButton.SetActive(true);
            LeftButton.SetActive(true);
            //반드시 두 버튼이 모두 활성화 되어 있어야 한다.
        }
    }

    public void ClickRightButton()
    {
        GameObject currentPanel = GameObject.Find("VideoPanel"+currentPage.ToString());
        currentPanel.SetActive(false);
        currentPage+=1;
        currentPanel = GameObject.Find("VideoCanvas").transform.Find("VideoPanel"+currentPage.ToString()).gameObject;
        currentPanel.SetActive(true);
        SetButtonActive();
    }

    public void ClickLeftButton()
    {
        GameObject currentPanel = GameObject.Find("VideoPanel"+currentPage.ToString());
        currentPanel.SetActive(false);
        Debug.Log(currentPanel+"1빼기 전");
        currentPage = currentPage-1;
        currentPanel = GameObject.Find("VideoCanvas").transform.Find("VideoPanel"+currentPage.ToString()).gameObject;
        currentPanel.SetActive(true);
        SetButtonActive();
    }



    public VideoSurface makePlaneSurface(string goName)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);

        if (go == null)
        {
            return null;
        }
        go.name = goName;
        // set up transform
        go.transform.Rotate(-90.0f, 0.0f, 0.0f);
        int num = channelJoined%6;
        if(num==1)
        {
            xPos = -220;
            yPos -= 100;
        }
        else if(num==4)
        {
            xPos = -220;
            yPos -= 100;
        }
        else if(num==2 || num==3 || num==5 || num==0)
        {
            xPos+=50;
        } 
        //channel
        go.transform.position = new Vector3(xPos, yPos, 0f);
        go.transform.localScale = new Vector3(0.25f, 0.5f, .5f);
        go.AddComponent<RectTransform>();
        RectTransform rectTran = go.GetComponent<RectTransform>();
        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150);
        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 150);
        // configure videoSurface
        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        return videoSurface;
    }







    private const float Offset = 100;
    public VideoSurface makeImageSurface(string goName)
    {
        GameObject go = (GameObject)Instantiate(Resources.Load("Screen"));
        int num = channelJoined%6; //나머지를 통해 각 화면의 위치 잡아줌
        int num2 = channelJoined/6; //각 화면이 몇 번째 패널에 들어갈지 잡아줌
        if (go == null)
        {
            return null;
        }

        go.name = goName;

        // to be renderered onto
        go.AddComponent<RawImage>();

        // make the object draggable
        go.AddComponent<UIElementDragger>();
        GameObject canvas = GameObject.Find("VideoPanel"+panelNum.ToString());
        if (canvas != null)
        {
            go.transform.parent = canvas.transform;
        }
        // set up transform
        go.transform.Rotate(0f, 0.0f, 0f);
        
        if(num==1)
        {
            //new Panel생성, Panel안에 화면 띄움, 기존 Panel에 setActive false설정,
            xPos = -60;
            yPos = 110;
        }
        else if(num==2)
        {
            xPos = 120;
            yPos = 110;
        }
        else if(num==3)
        {
            xPos = 300;
            yPos = 110;
            //이거 이런 식으로 하는게 아니라 숫자 별로 위치 다 지정해주기
            //한명 나가면 ReRendering하도록 만들어야 하나? (Userleave어쩌고에서)
        }
        else if(num==4)
        {
            xPos = -60;
            yPos = -70;
        }
        else if(num==5)
        {
            xPos = 120;
            yPos = -70;
        }
        else if(num==0)
        {
            xPos = 300;
            yPos = -70;
        }
        go.transform.localPosition = new Vector3(xPos, yPos, 0f);
        go.transform.localScale = new Vector3(1f, 1f, 1f);
        // configure videoSurface
        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        return videoSurface;
    }
    // When remote user is offline, this delegate will be called. Typically
    // delete the GameObject for this user







    private void onUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        // remove video stream
        Debug.Log("onUserOffline: uid = " + uid + " reason = " + reason);
        channelJoined-=1;
        // this is called in main thread
       /* GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            Object.Destroy(go);
        }*/
        Debug.Log(channelJoined+"ChannelJoined");
        foreach(uint remoteUid in uidList)
        {
            GameObject remoteUserScreen = GameObject.Find(remoteUid.ToString());
            Destroy(remoteUserScreen);
        }
        leave();
        channelJoined = 1;
        unloadEngine();
        GameObject videoScreen = GameObject.Find("Screen");
        VideoSurface videoSurface = videoScreen.GetComponent<VideoSurface>();
        Destroy(videoSurface);
        loadEngine(AppID);
        join(staticChannelName);
        onSceneHelloVideoLoaded();
    }








    #region Error Handling
    private int LastError { get; set; }
    private void HandleError(int error, string msg)
    {
        if (error == LastError)
        {
            return;
        }

        if (string.IsNullOrEmpty(msg))
        {
            msg = string.Format("Error code:{0} msg:{1}", error, IRtcEngine.GetErrorDescription(error));
        }

        switch (error)
        {
            case 101:
                msg += "\nPlease make sure your AppId is valid and it does not require a certificate for this demo.";
                break;
            
        }

        Debug.LogError(msg);
        if (MessageText != null)
        {
            if (MessageText.text.Length > 0)
            {
                msg = "\n" + msg;
            }
            MessageText.text += msg;
        }

        LastError = error;
    }

    public void OnUserMutedAudio(uint uid, bool muted)
    {
        Debug.Log(uid+"OnUserMutedAudio 동작하는지");
        if(muted)
        {
            //GameObject audioButton = GameObject.Find("AudioButton"+uid.ToString());
            //Destroy(audioButton);
            Debug.Log(uid+"가 음소거를 합니다.");
        }
        else
        {

        }
    }

    public void OnUserMutedVideo(uint uid, bool muted)
    {
        Debug.Log(uid+"OnUserMutedVideo 동작하는지");
        if(muted)
        {
            Debug.Log(uid+"비디오 중지함.");
        }
        else
        {

        }
    }


    void OnRequestToken()
    {
        mRtcEngine.RenewToken(token);
    }

    #endregion
}
