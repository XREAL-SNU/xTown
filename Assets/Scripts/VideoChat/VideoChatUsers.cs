using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VideoChatUsers
{ 

    public static List<User> ChatUsers = new List<User>();
    static User _me;
    public static User Me
    {
        get => _me;
        set
        {
            _me = value;
        }
    }
    public static IEnumerator GetEnumeratorAt(User user, int offset)
    {
        int idx = ChatUsers.IndexOf(user);
        if (idx < 0) return null;
        idx += offset;
        idx = idx < 0 ? 0 : idx;
        idx = idx >= ChatUsers.Count ? ChatUsers.Count - 1 : idx;
        user = ChatUsers[idx];
        
        var enumerator = ChatUsers.GetEnumerator();
        while (enumerator.Current != user && enumerator.MoveNext()) { }
        return enumerator;
    }

    public static bool AddUser(User user, bool isLocal = false)
    {
        Debug.Log($"ChatUsers/ adding {user.Name}");
        if (ChatUsers.Contains(user)) return false;
        ChatUsers.Add(user);
        if (isLocal) Me = user;
        if(AddUserHandler != null) AddUserHandler.Invoke(user);
        return true;
    }

    public static bool RemoveUser(User user, bool isLocal = false)
    {
        Debug.Log($"ChatUsers/ removing {user.Name}");
        if (!ChatUsers.Contains(user)) return false;
        ChatUsers.Remove(user);
        if (RemoveUserHandler != null) RemoveUserHandler.Invoke(user);
        Debug.Log("the line after invokation");
        return true;
    }

    public static Action<User> AddUserHandler = null;
    public static Action<User> RemoveUserHandler = null;

    public enum UserEvents
    {
        Add, Remove
    }

    public static void AddListener(Action<User> action, UserEvents eventType)
    {
        switch (eventType)
        {
            case UserEvents.Add:
                AddUserHandler -= action;
                AddUserHandler += action;
                break;
            case UserEvents.Remove:
                RemoveUserHandler -= action;
                RemoveUserHandler += action;
                break;
        }
    }
    public static void RemoveListener(Action<User> action, UserEvents eventType)
    {
        switch (eventType)
        {
            case UserEvents.Add:
                AddUserHandler -= action;
                break;
            case UserEvents.Remove:
                RemoveUserHandler -= action;
                break;
        }
    }

}

public class User
{
    public string Name;
    public User(string str) { Name = str; }
    public User() { }
}