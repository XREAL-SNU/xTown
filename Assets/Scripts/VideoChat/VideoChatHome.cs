using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

#if(UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif
using System.Collections;
using agora_gaming_rtc;
using Photon.Pun;
/// <summary>
///    TestHome serves a game controller object for this application.
/// </summary>
public class VideoChatHome : MonoBehaviour
{

    // Use this for initialization
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList();
#endif
    static AgoraUnityVideo app = null;

    private string HomeSceneName = "MainRoom";

    private string PlaySceneName = "SceneHelloVideo";
    public RectTransform VideoPanel;
    public RawImage Screen;
    public Text myNickName;
    private bool _previewing = false;
    private bool audioEnable = false;
    private bool IsJoined = false;
    // PLEASE KEEP THIS App ID IN SAFE PLACE
    // Get your own App ID at https://dashboard.agora.io/
    [SerializeField]
    private string AppID = "your_appid";
    private string AppName = "Xreal";
    private bool isClicked = false;
    private bool isRegistered = false;
    void Awake()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
		permissionList.Add(Permission.Microphone);         
		permissionList.Add(Permission.Camera);               
#endif

        // keep this alive across scenes
        //DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        //CheckAppId();
        VideoPanel.gameObject.SetActive(false);
        
    }

    /*var engine = IRtcEngine.GetEngine(AppID);
        engine.RegisterLocalUserAccount(AppID, PhotonNetwork.NickName);
        UserInfo myInfo = engine.GetUserInfoByUserAccount(PhotonNetwork.NickName);
        Debug.Log(myInfo.uid+myInfo.userAccount+"내 정보");*/

    void Update()
    {
        CheckPermissions();
    }

    /*private void CheckAppId()
    {
        Debug.Assert(AppID.Length > 10, "Please fill in your AppId first on Game Controller object.");
        GameObject go = GameObject.Find("AppIDText");
        if (go != null)
        {
            Text appIDText = go.GetComponent<Text>();
            if (appIDText != null)
            {
                if (string.IsNullOrEmpty(AppID))
                {
                    appIDText.text = "AppID: " + "UNDEFINED!";
                }
                else
                {
                    appIDText.text = "AppID: " + AppID.Substring(0, 4) + "********" + AppID.Substring(AppID.Length - 4, 4);
                }
            }
        }
    }*/

    /// <summary>
    ///   Checks for platform dependent permissions.
    /// </summary>
    private void CheckPermissions()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        foreach(string permission in permissionList)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {                 
				Permission.RequestUserPermission(permission);
			}
        }
#endif
    }


    public void faceCallButtonClicked()
    {
        Debug.Log("FaceCallButtonClicked"+isClicked.ToString());
        if(isClicked==false)
        {
            onJoinButtonClicked();
            isClicked=true;
            _previewing = true;
            Debug.Log("VideoChat Start");
        }
        else
        {
            onLeaveButtonClicked();
            isClicked=false;
            _previewing = false;
            Debug.Log("VideoChat End");
        }
    }






    public void onJoinButtonClicked()
    {
        // get parameters (channel name, channel profile, etc.)
        //GameObject go = GameObject.Find("ChannelName");
        //InputField field = go.GetComponent<InputField>();
        GameObject videoCanvas = GameObject.Find("VideoChat").transform.Find("VideoCanvas").gameObject;
        videoCanvas.gameObject.SetActive(true);
        // create app if nonexistent
        if (ReferenceEquals(app, null))
        {
            app = new AgoraUnityVideo(); // create app
            
        }
        app.loadEngine(AppID); // load engine
        // join channel and jump to next scene
        
        app.join(AppName);
        app.onSceneHelloVideoLoaded();
        
        if(!isRegistered)
        {
            RegisterAccount();
        }
        //내가 만든 채널에 바로 접속하도록 설정해둠.


        //SceneManager.sceneLoaded += OnLevelFinishedLoading; // configure GameObject after scene is loaded
        //SceneManager.LoadScene(PlaySceneName, LoadSceneMode.Single);
    }

    public void onLeaveButtonClicked()
    {
        if (!ReferenceEquals(app, null))
        {
            app.leave(); // leave channel
            app.unloadEngine(); // delete engine
            app = null;
        }
        GameObject videoScreen = GameObject.Find("Screen");
        VideoSurface videoSurface = videoScreen.GetComponent<VideoSurface>();
        Destroy(videoSurface);
        GameObject VideoCanvas = GameObject.Find("VideoCanvas");
        VideoCanvas.SetActive(false);
    }

    public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == PlaySceneName)
        {
            if (!ReferenceEquals(app, null))
            {
                app.onSceneHelloVideoLoaded(); // call this after scene is loaded
            }
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
    }

    void OnApplicationPause(bool paused)
    {
        if (!ReferenceEquals(app, null))
        {
            app.EnableVideo(paused);
        }
    }

    void OnApplicationQuit()
    {
        if (!ReferenceEquals(app, null))
        {
            app.unloadEngine();
        }
    }

    //Button button
    public void VideoButtonClicked()
    {
        var engine = IRtcEngine.GetEngine(AppID);
        //Screen.GetComponent<VideoSurface>().SetEnable(_previewing);
        app.EnableVideo(_previewing);
        
        if (_previewing)
        {
            //button.GetComponentInChildren<Text>().text = "StopVideo";
            CheckDevices(engine);
        }
        else
        {
            //button.GetComponentInChildren<Text>().text = "StartVideo";
        }

        _previewing= !_previewing;
    }


    public void AudioButtonClicked()
    {
        var engine = IRtcEngine.GetEngine(AppID);

        app.EnableAudio(audioEnable);
        audioEnable=!audioEnable;

    }


    void CheckDevices(IRtcEngine engine)
    {
        VideoDeviceManager deviceManager = VideoDeviceManager.GetInstance(engine);
        deviceManager.CreateAVideoDeviceManager();

        int cnt = deviceManager.GetVideoDeviceCount();
        Debug.Log("Device count =============== " + cnt);
    }

    void RegisterAccount()
    {
        var engine = IRtcEngine.GetEngine(AppID);
        int success = engine.RegisterLocalUserAccount(AppID, PhotonNetwork.NickName);
        isRegistered = true;
        UserInfo myInfo = engine.GetUserInfoByUserAccount(PhotonNetwork.NickName);
        Debug.Log(myInfo.uid+myInfo.userAccount+"내 정보");
        Debug.Log(success+"Success Return값");
        if(success == 0)
        {
            
        }
    }
}
