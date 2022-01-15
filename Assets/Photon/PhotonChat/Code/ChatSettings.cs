// ----------------------------------------------------------------------------
// <copyright file="ChatSettings.cs" company="Exit Games GmbH">
//   Photon Cloud Account Service - Copyright (C) 2012 Exit Games GmbH
// </copyright>
// <summary>
//   Scriptable Object to store Chat App Id
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------

using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ChatSettings : ScriptableObject
{
    public string AppId;
    #if UNITY_EDITOR
    [HideInInspector]
    public bool WizardDone;
    #endif

    private static ChatSettings instance;
    public static ChatSettings Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = Load();
            return instance;
        }
    }

    public static ChatSettings Load()
    {
        ChatSettings settings = (ChatSettings)Resources.Load("ChatSettingsFile", typeof (ChatSettings));
        if (settings != null)
        {
            return settings;
        }
        else
        {
            return Create();
        }
    }
     

    private static ChatSettings Create()
    {
        ChatSettings settings = (ChatSettings)ScriptableObject.CreateInstance("ChatSettings");
#if UNITY_EDITOR
        if (!Directory.Exists("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
            AssetDatabase.ImportAsset("Assets/Resources");
        }

        AssetDatabase.CreateAsset(settings, "Assets/Resources/ChatSettingsFile.asset");
        EditorUtility.SetDirty(settings);

        settings = (ChatSettings)Resources.Load("ChatSettingsFile", typeof(ChatSettings));
#endif
        return settings;
    }
}