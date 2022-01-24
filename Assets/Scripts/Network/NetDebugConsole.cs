using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NetDebugConsole : MonoBehaviour
{
    public static NetDebugConsole Instance;
    void Start()
    {
        if (Instance is null) Instance = this;
        DisplayMessage("Beginning network debug");
    }

    private string _displayText = "";
    private Vector2 _scrollPosition;
    public void DisplayMessage(string msg)
    {
        _displayText = _displayText + "\n" + msg;
    }

    void OnGUI()
    {
        GUI.backgroundColor = Color.green;
        using (var scrollViewScope = new GUILayout.ScrollViewScope(_scrollPosition, GUILayout.Width(300), GUILayout.Height(100)))
        {
            _scrollPosition = scrollViewScope.scrollPosition;
            GUILayout.Label(_displayText);

        }

        if (GUILayout.Button("Clear"))
            _displayText = "";
    }
}
