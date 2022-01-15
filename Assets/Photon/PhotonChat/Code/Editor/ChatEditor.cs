// ----------------------------------------------------------------------------
// <copyright file="ChatEditor.cs" company="Exit Games GmbH">
//   Photon Cloud Account Service - Copyright (C) 2012 Exit Games GmbH
// </copyright>
// <summary>
//   Editor wizards to fill in the Chat App Id
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------


using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using System;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class ChatEditor : EditorWindow
{
    static ChatEditor()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    private static void OnEditorUpdate()
    {
        if (currentSettings == null)
        {
            currentSettings = ChatSettings.Load();
        }
        if (!currentSettings.WizardDone)
        {
            OpenWizard();
        }
    }


    [MenuItem("Window/Photon Chat/Setup")]
    public static void OpenWizard()
    {
        currentSettings = ChatSettings.Load();
        currentSettings.WizardDone = true;
        EditorUtility.SetDirty(currentSettings);

        ChatEditor editor = (ChatEditor)EditorWindow.GetWindow(typeof(ChatEditor), false, "Photon Chat");
        editor.minSize = editor.preferredSize;
        editor.mailOrAppId = currentSettings.AppId;
    }


    private static ChatSettings currentSettings;
    internal string mailOrAppId;
    internal bool showDashboardLink = false;
    internal bool showRegistrationDone = false;
    internal bool showRegistrationError = false;
    private readonly Vector2 preferredSize = new Vector2(350, 400);

    internal static string UrlCloudDashboard = "https://www.photonengine.com/Dashboard/Chat?email=";

    public string WelcomeText = "Thanks for importing Photon Chat.\nThis window should set you up.\n\n<b>*</b> To use an existing Photon Chat App, enter your AppId.\n<b>*</b> To register or access an existing account, enter the account’s mail address.";
    public string AlreadyRegisteredInfo = "The email is registered so we can't fetch your AppId (without password).\n\nPlease login online to get your AppId and paste it above.";
    public string RegisteredNewAccountInfo = "We created a (free) account and fetched you an AppId.\nWelcome. Your Photon Chat project is setup.";
    public string FailedToRegisterAccount = "This wizard failed to register an account right now. Please check your mail address or try via the Dashboard.";
    public string AppliedToSettingsInfo = "Your AppId is now applied to this project.";
    public string SetupCompleteInfo = "<b>Done!</b>\nYour Chat AppId is now stored in the <b>ChatSettingsFile</b> now.\nHave a look.";
    public string CloseWindowButton = "Close";
    public string OpenCloudDashboardText = "Cloud Dashboard Login";
    public string OpenCloudDashboardTooltip = "Review Cloud App information and statistics.";


    public void OnGUI()
    {
        GUI.skin.label.wordWrap = true;
        GUI.skin.label.richText = true;
        if (string.IsNullOrEmpty(mailOrAppId))
        {
            mailOrAppId = string.Empty;
        }

        GUILayout.Label("Chat Settings", EditorStyles.boldLabel);
        GUILayout.Label(this.WelcomeText);
        GUILayout.Space(15);


        GUILayout.Label("AppId or Email");
        string input = EditorGUILayout.TextField(this.mailOrAppId);


        if (GUI.changed)
        {
            this.mailOrAppId = input.Trim();
        }

        bool isMail = false;
        bool minimumInput = false;
        bool isAppId = false;

        if (AccountService.IsValidEmail(this.mailOrAppId))
        {
            // this should be a mail address
            minimumInput = true;
            isMail = true;
        }
        else if (IsAppId(this.mailOrAppId))
        {
            // this should be an appId
            minimumInput = true;
            isAppId = true;
        }


        EditorGUI.BeginDisabledGroup(!minimumInput);


        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        bool setupBtn = GUILayout.Button("Setup", GUILayout.Width(205));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (setupBtn)
        {
            this.showDashboardLink = false;
            this.showRegistrationDone = false;
            this.showRegistrationError = false;

            if (isMail)
            {
                RegisterWithEmail(this.mailOrAppId);
            }
            else if (isAppId)
            {
                currentSettings.AppId = this.mailOrAppId;
                EditorUtility.SetDirty(currentSettings);
                showRegistrationDone = true;
            }

            EditorGUIUtility.PingObject(currentSettings);
        }
        EditorGUI.EndDisabledGroup();

        if (this.showDashboardLink)
        {
            // button to open dashboard and get the AppId
            GUILayout.Space(15);
            GUILayout.Label(AlreadyRegisteredInfo);


            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent(OpenCloudDashboardText, OpenCloudDashboardTooltip), GUILayout.Width(205)))
            {
                EditorUtility.OpenWithDefaultApp(UrlCloudDashboard + Uri.EscapeUriString(this.mailOrAppId));
                this.mailOrAppId = string.Empty;
                this.showDashboardLink = false;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        if (this.showRegistrationError)
        {
            GUILayout.Space(15);
            GUILayout.Label(FailedToRegisterAccount);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent(OpenCloudDashboardText, OpenCloudDashboardTooltip), GUILayout.Width(205)))
            {
                EditorUtility.OpenWithDefaultApp(UrlCloudDashboard + Uri.EscapeUriString(this.mailOrAppId));
                this.mailOrAppId = string.Empty;
                this.showDashboardLink = false;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

        }
        if (this.showRegistrationDone)
        {
            GUILayout.Space(15);
            GUILayout.Label("Registration done");
            if (isMail)
            {
                GUILayout.Label(RegisteredNewAccountInfo);
            }
            else
            {
                GUILayout.Label(AppliedToSettingsInfo);
            }

            // setup-complete info
            GUILayout.Space(15);
            GUILayout.Label(SetupCompleteInfo);


            // close window (done)
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(CloseWindowButton, GUILayout.Width(205)))
            {
                this.Close();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    public static bool IsAppId(string val)
    {
        if (string.IsNullOrEmpty(val) || val.Length < 16)
        {
            return false;
        }

        try
        {
            new Guid(val);
        }
        catch
        {
            return false;
        }
        return true;
    }

    protected virtual void RegisterWithEmail(string email)
    {
        AccountService client = new AccountService();
        List<ServiceTypes> types = new List<ServiceTypes>(1);
        types.Add(ServiceTypes.Chat);
        if (client.RegisterByEmail(email, types, RegisterWithEmailSuccessCallback, RegisterWithEmailErrorCallback))
        {
            EditorUtility.DisplayProgressBar("Fetching Account", "Trying to register a Photon Cloud Account.", 0.5f);
        }
    }

    private void RegisterWithEmailSuccessCallback(AccountServiceResponse res)
    {
        EditorUtility.ClearProgressBar();
        if (res.ReturnCode == AccountServiceReturnCodes.Success)
        {
            string key = ((int)ServiceTypes.Chat).ToString();
            string appId;
            if (res.ApplicationIds.TryGetValue(key, out appId))
            {
                currentSettings.AppId = appId;
                EditorUtility.SetDirty(currentSettings);
                this.showRegistrationDone = true;
                Selection.objects = new UnityEngine.Object[] { currentSettings };
            }
            else
            {
                DisplayErrorMessage("Registration successful, but no Chat AppId returned");
            }
        }
        else if (res.ReturnCode == 12) // todo: gather all error codes
        {
            this.showDashboardLink = true;
        }
        else
        {
            DisplayErrorMessage(res.Message);
        }
    }

    private void RegisterWithEmailErrorCallback(string error)
    {
        EditorUtility.ClearProgressBar();
        DisplayErrorMessage(error);
    }

    private void DisplayErrorMessage(string error)
    {
        Debug.LogErrorFormat("Registration error: {0}", error);
        this.showRegistrationError = true;
    }

    //https://forum.unity.com/threads/using-unitywebrequest-in-editor-tools.397466/#post-4485181
    public static void StartCoroutine(System.Collections.IEnumerator update)
    {
        EditorApplication.CallbackFunction closureCallback = null;

        closureCallback = () =>
        {
            try
            {
                if (update.MoveNext() == false)
                {
                    EditorApplication.update -= closureCallback;
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                EditorApplication.update -= closureCallback;
            }
        };

        EditorApplication.update += closureCallback;
    }
    
    public static System.Collections.IEnumerator HttpPost(string url, Dictionary<string, string> headers, byte[] payload, Action<string> successCallback, Action<string> errorCallback)
    {
        using (UnityWebRequest w = new UnityWebRequest(url, "POST"))
        {
            if (payload != null)
            {
                w.uploadHandler = new UploadHandlerRaw(payload);
            }
            w.downloadHandler = new DownloadHandlerBuffer();
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    w.SetRequestHeader(header.Key, header.Value);
                }
            }

            #if UNITY_2017_2_OR_NEWER
            yield return w.SendWebRequest();
            #else
            yield return w.Send();
            #endif

            while (w.isDone == false)
                yield return null;

            #if UNITY_2017_1_OR_NEWER
            if (w.isNetworkError || w.isHttpError)
            #else
            if (w.isError)
            #endif
            {
                if (errorCallback != null)
                {
                    errorCallback(w.error);
                }
            }
            else
            {
                if (successCallback != null)
                {
                    successCallback(w.downloadHandler.text);
                }
            }
        }
    }
}
