using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;


public class CreateRoomManager : MonoBehaviour
{
    private StorageManager storageManager;
    private List<string> userList = new List<string>();
    public List<Image> userBGList = new List<Image>();
    public List<Button> userButtonList = new List<Button>();
    public List<Image> userButtonBackImageList = new List<Image>();
    public List<Text> userTextList = new List<Text>();
    public List<string> chosenUserList = new List<string>();

    public InputField roomName;
    public Dropdown roomStatus;
    public InputField roomMember;


    // Start is called before the first frame update
    void Start()
    {
        storageManager = GameObject.Find("Storage").GetComponent<StorageManager>();
        userList = storageManager.GetUserList();
        for (int i = 0; i < userButtonList.Count; i++)
        {
            if (i % 3 == 0) 
            {
                userBGList[i / 3].gameObject.SetActive(false);
            }
            userButtonList[i].gameObject.SetActive(false);
        }
        
        for (int i = 0; i < userList.Count; i++)
        {
            int index = i;
            userTextList[i].text = userList[i];
            userButtonList[i].gameObject.SetActive(true);
            userButtonList[i].onClick.AddListener(() => { ChooseUser(userList[index], userButtonBackImageList[index]); });
            if (i % 3 == 0)
            {
                userBGList[i / 3].gameObject.SetActive(true);
            }
        }
        CreateRoom();
    }


    void ChooseUser(string user, Image backImage)
    {
        if (chosenUserList.Contains(user))
        {
            Color color = backImage.color;
            chosenUserList.Remove(user);
            backImage.color = new Color32(170, 244, 83, 255);
            Debug.Log(user + "is removed.");
        }
        else
        {
            chosenUserList.Add(user);
            backImage.color = Color.green;
            Debug.Log(user + "is selected.");
        }
    }

    void CreateRoom()
    {
        string RoomName = roomName.text;
        string RoomMember = roomMember.text;
    }
}
