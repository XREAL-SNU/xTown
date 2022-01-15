using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;

public class StorageManager : MonoBehaviour
{
    public List<string> userlist = new List<string>();
    public Text outputText;
    public static int A = 3;
    private string[] temp;

    // Start is called before the first frame update
    void Start()
    {
        AddUser();
        for (int i = 0; i < userlist.Count; i++)
        {
            outputText.text += userlist[i] + "\n";
        }
        temp = outputText.text.Split('\n');
        for (int i = 0; i < temp.Length; i++)
        {
            Debug.Log(temp[i]);
        }
    }

    public List<string> GetUserList()
    {
        AddUser();
        return userlist;
    }
    void AddUser()
    {
        userlist.Add("Alice");
        userlist.Add("Bob");
        userlist.Add("Peter");
        userlist.Add("Chuu");
        userlist.Add("Spider");
        userlist.Add("Peach");
        userlist.Add("Celin");
        userlist.Add("Dann");
        userlist.Add("Ed");
        userlist.Add("Heaker");
    }

    // Update is called once per frame
    void Update()
    {

    }





}
