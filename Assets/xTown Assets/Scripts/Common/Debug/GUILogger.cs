using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GUILayout;

public class GUILogger : MonoBehaviour
{

    static GUILogger _logger;
    public static GUILogger Logger
    {
        get => _logger;
    }


    void Awake()
    {
        if (_logger == null)
        {
            _logger = this;

        }
        else if (_logger != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public Vector2 scrollPosition;
    public string message = "Debugger enabled";

    void OnGUI()
    {

        using (var scrollViewScope = new ScrollViewScope(scrollPosition, GUILayout.Width(400), GUILayout.Height(400)))
        {
            scrollPosition = scrollViewScope.scrollPosition;
            GUI.backgroundColor = Color.black;

            GUILayout.Label(message);
            if (GUILayout.Button("Clear")) message = "";
        }
    }

    public void Log(string str)
    {
        str += "\n";
        message += str;
    }

    public void LogBlack(string str)
    {
        string newstr = "<color=black>" + str + "</color>";
        Log(newstr);
    }

}
