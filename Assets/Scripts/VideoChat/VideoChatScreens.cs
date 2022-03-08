using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoChatScreens: MonoBehaviour
{

    // reference to the parent paging UI
    ChatPagingUI pagingUI;

    #region MonobehaviourCallbacks

    private void Start()
    {
        if(pagingUI is null) pagingUI = GetComponentInParent<ChatPagingUI>();
        // test: add me
        User me = new User("Alice");
        VideoChatUsers.AddUser(me, true);
        pinnedUsers.Add(me);
        //
        InitPage();
    }

    private void OnEnable()
    {
        if (pagingUI is null) pagingUI = GetComponentInParent<ChatPagingUI>();
        pagingUI.AddListener(OnPaged);
        VideoChatUsers.AddListener(OnUserAdd, VideoChatUsers.UserEvents.Add);
        VideoChatUsers.AddListener(OnUserRemove, VideoChatUsers.UserEvents.Remove);

        InitPage();
    }
    private void OnDisable()
    {
        pagingUI.RemoveListener(OnPaged);
        VideoChatUsers.RemoveListener(OnUserAdd, VideoChatUsers.UserEvents.Add);
        VideoChatUsers.RemoveListener(OnUserRemove, VideoChatUsers.UserEvents.Remove);
        for (int i = 0; i < ItemsPerPage; ++i)
        {
            this[i].Destroy();
        }
    }
    #endregion


    VideoChatScreen this[int i]
    {
        get
        {
            if (i >= transform.childCount) return null;
            return transform.GetChild(i).GetComponent<VideoChatScreen>();
        }
    }

    public int CurrentPageNum = 0;
    public int ActiveScreenCount = 0;
    public int ItemsPerPage;

    List<VideoChatScreen> _screens = new List<VideoChatScreen>();
    int StartIndex, EndIndex;
    /*
    public void InitPage()
    {
        for(int i = 0; i < ActiveScreenCount; ++i)
        {
            this[i].Destroy();
        }
        User me = VideoChatUsers.Me;
        if (me != null)
        {
            AddEmptyScreen(true).ScreenUser = me;
        }

        foreach(User user in pinnedUsers)
        {
            if (user == me) continue;
            AddEmptyScreen().ScreenUser = user;
        }

        for(int i = 0; ActiveScreenCount < ItemsPerPage && i < VideoChatUsers.ChatUsers.Count; ++i)
        {
            if (pinnedUsers.Contains(VideoChatUsers.ChatUsers[i])) continue;
            AddEmptyScreen().ScreenUser = VideoChatUsers.ChatUsers[i];
        }

        StartIndex = VideoChatUsers.ChatUsers.IndexOf(this[PinnedItemCount].ScreenUser);
        int lastScreenIndex = Math.Min(VideoChatUsers.ChatUsers.Count, ItemsPerPage);
        EndIndex = VideoChatUsers.ChatUsers.IndexOf(this[lastScreenIndex - 1].ScreenUser);

        Debug.Log($"Display {StartIndex}~{EndIndex}");
        UpdateButtons();
    }
    */

    public void InitPage()
    {
        foreach(var screen in _screens)
        {
            screen.gameObject.SetActive(false);
        }
        ActiveScreenCount = 0;
        if (_screens.Count < 1)
        {
            for (int i = 0; i < ItemsPerPage; ++i)
            {
                // instantiate local screen for i == 0
                var screen = AddEmptyScreen(i == 0);
                _screens.Add(screen);
                screen.gameObject.SetActive(false);
            }
        }

        User me = VideoChatUsers.Me;
        if (me != null)
        {
            _screens[0].ScreenUser = me;
            ActiveScreenCount++;
        }

        foreach (User user in pinnedUsers)
        {
            if (user == me) continue;
            _screens[ActiveScreenCount++].ScreenUser = user;
        }

        for (int i = 0; ActiveScreenCount < ItemsPerPage && i < VideoChatUsers.ChatUsers.Count; ++i)
        {
            if (pinnedUsers.Contains(VideoChatUsers.ChatUsers[i])) continue;
            _screens[ActiveScreenCount++].ScreenUser = VideoChatUsers.ChatUsers[i];
        }

        for(int i = 0; i < ActiveScreenCount; ++i)
        {
            _screens[i].gameObject.SetActive(true);
        }
        if(ActiveScreenCount - PinnedItemCount < 1)
        {
            StartIndex = EndIndex = -1;
        }
        else
        {
            StartIndex = VideoChatUsers.ChatUsers.IndexOf(this[PinnedItemCount].ScreenUser);
            EndIndex = VideoChatUsers.ChatUsers.IndexOf(this[ActiveScreenCount - 1].ScreenUser);
        }

        Debug.Log($"Display {StartIndex}~{EndIndex}");
        Debug.Log($"After Init: Pinned #{PinnedItemCount}, Active #{ActiveScreenCount}");
        UpdateButtons();

    }

    #region Pagecallbacks
    // callbacks

    public void OnPaged(PagingUI.PageEvents eventType)
    {
        if (eventType == PagingUI.PageEvents.Next) OnPagedUp();
        if (eventType == PagingUI.PageEvents.Prev) OnPagedDown();
    }

    public void OnPagedUp()
    {
        int userIdx = EndIndex + 1;
        ActiveScreenCount = PinnedItemCount;
        for (int i = PinnedItemCount; i < ItemsPerPage && userIdx < VideoChatUsers.ChatUsers.Count;)
        {
            if (pinnedUsers.Contains(VideoChatUsers.ChatUsers[userIdx]))
            {
                userIdx++;
                continue;
            }
            this[i].ScreenUser = VideoChatUsers.ChatUsers[userIdx];
            if (!this[i].gameObject.activeInHierarchy) this[i].gameObject.SetActive(true);
            i++;
            userIdx++;
            ActiveScreenCount++;
        }


        for (int i = ActiveScreenCount; i < ItemsPerPage; ++i) this[i].gameObject.SetActive(false);

        StartIndex = VideoChatUsers.ChatUsers.IndexOf(this[PinnedItemCount].ScreenUser);
        EndIndex = VideoChatUsers.ChatUsers.IndexOf(this[ActiveScreenCount - 1].ScreenUser);
        Debug.Log($"Display {StartIndex}~{EndIndex} Active #{ActiveScreenCount} Pinned # {PinnedItemCount}");

        UpdateButtons();

    }

    public void OnPagedDown()
    {
        List<User> usersToDisplay = VideoChatUsers.ChatUsers.FindAll((item) => !pinnedUsers.Contains(item));

        // get the index to display from,
        int userIdx = usersToDisplay.IndexOf(VideoChatUsers.ChatUsers[StartIndex]);
        userIdx = userIdx - (ItemsPerPage - PinnedItemCount);
        if (userIdx < 0) userIdx = 0;

        // convert to index in the full list.
        userIdx = VideoChatUsers.ChatUsers.IndexOf(usersToDisplay[userIdx]);

        ActiveScreenCount = PinnedItemCount;
        for (int i = PinnedItemCount; i < ItemsPerPage && userIdx < VideoChatUsers.ChatUsers.Count;)
        {
            if (pinnedUsers.Contains(VideoChatUsers.ChatUsers[userIdx]))
            {
                userIdx++;
                continue;
            }
            this[i].ScreenUser = VideoChatUsers.ChatUsers[userIdx];
            if (!this[i].gameObject.activeInHierarchy) this[i].gameObject.SetActive(true);
            i++;
            userIdx++;
            ActiveScreenCount++;
        }
        for (int i = ActiveScreenCount; i < ItemsPerPage; ++i) this[i].gameObject.SetActive(false);

        StartIndex = VideoChatUsers.ChatUsers.IndexOf(this[PinnedItemCount].ScreenUser);
        EndIndex = VideoChatUsers.ChatUsers.IndexOf(this[ActiveScreenCount - 1].ScreenUser);
        Debug.Log($"Display {StartIndex}~{EndIndex} Active #{ActiveScreenCount} Pinned # {PinnedItemCount}");

        UpdateButtons();
    }
    /*
    public void OnPagedUp()
    {
        int screenIdx = PinnedItemCount;
        int userIdx = VideoChatUsers.ChatUsers.IndexOf(this[ActiveScreenCount - 1].ScreenUser);
        if (userIdx < 0) return;

        userIdx++;

        while (screenIdx < ItemsPerPage && userIdx < VideoChatUsers.ChatUsers.Count)
        {
            if (pinnedUsers.Contains(VideoChatUsers.ChatUsers[userIdx])) continue;
            this[screenIdx++].ScreenUser = VideoChatUsers.ChatUsers[userIdx++];
        }

        for(int i = screenIdx; i < ItemsPerPage; ++i)
        {
            this[i].Destroy();
        }
        for(int i = 0; i < ActiveScreenCount; ++i)
        {
            Debug.Log($"Remaining({i}) {this[i].ScreenUser.Name}");
        }
        UpdateButtons();
    }
    public void OnPagedDown()
    {
        List<User> usersToDisplay = VideoChatUsers.ChatUsers.FindAll((item) => !pinnedUsers.Contains(item));

        int userIdx = VideoChatUsers.ChatUsers.IndexOf(this[PinnedItemCount].ScreenUser);
        userIdx -= ItemsPerPage - PinnedItemCount;
        if (userIdx < 0) userIdx = 0;

        Debug.Log($"Display {ItemsPerPage - PinnedItemCount} users from idx {userIdx}");
        for(int screenIdx = PinnedItemCount; screenIdx < ItemsPerPage && userIdx < usersToDisplay.Count; ++screenIdx)
        {
            VideoChatScreen screen = this[screenIdx];
            if (screen is null) screen = AddEmptyScreen();
            screen.ScreenUser = usersToDisplay[userIdx++];
        }

        UpdateButtons();
    }
    */

    #endregion

    #region Buttons

    void UpdateButtons()
    {

        if (pagingUI.NextBtnImage is null || pagingUI.PrevBtnImage is null) return;
        pagingUI.NextBtnImage.enabled = !IsLastPage();
        pagingUI.PrevBtnImage.enabled = !IsFirstPage();
    }
    bool IsFirstPage()
    {
        int userIndex = StartIndex - 1;
        Debug.Log($"IsFirstPage: scanning from #{userIndex}");
        // usually will return within one iteration, or when last user, maximum <PinnedUserCount> iterations

        while (userIndex >= 0)
        {
            if (!pinnedUsers.Contains(VideoChatUsers.ChatUsers[userIndex--])) return false;
        }
        return true;
    }

    bool IsLastPage()
    {
        
        int userIndex = EndIndex + 1;
        Debug.Log($"IsLastPage: scanning from #{userIndex}");

        // usually will return within one iteration, or when last user, maximum <PinnedUserCount> iterations
        int lastIndex = VideoChatUsers.ChatUsers.Count;
        while (userIndex < lastIndex)
        {
            if (!pinnedUsers.Contains(VideoChatUsers.ChatUsers[userIndex++])) return false;
        } 
        return true;
    }

    #endregion

    #region Pin

    // list of pinned users(includes my video)
    List<User> pinnedUsers = new List<User>();

    // this include my video
    public int PinnedItemCount
    {
        get { return pinnedUsers.Count; }
    }
    public int MaxPinnedItemCount = 3;


    public void PinScreen(int screenIndex)
    {
        VideoChatScreen pinScreen = this[screenIndex];
        pinnedUsers.Add(pinScreen.ScreenUser);

        if(screenIndex != PinnedItemCount - 1)
        {
            // swap position - to the last of pinned list.
            this[PinnedItemCount - 1].transform.SetSiblingIndex(screenIndex);
            pinScreen.transform.SetSiblingIndex(PinnedItemCount - 1);

        }
        
        Debug.Log($"Display {StartIndex}~{EndIndex} Active #{ActiveScreenCount} Pinned # {PinnedItemCount}");
    }

    public void UnpinScreen(int screenIndex)
    {
        User user = this[screenIndex].ScreenUser;
        pinnedUsers.Remove(user);
        this[screenIndex].transform.SetSiblingIndex(PinnedItemCount);

        Debug.Log($"Display {StartIndex}~{EndIndex} Active #{ActiveScreenCount} Pinned # {PinnedItemCount}");
    }

    #endregion

    #region users
    public void OnUserAdd(User user)
    {
        Debug.Log($"VideoChatScreens/Adduser Local?{user == VideoChatUsers.Me} : {user.Name}");
        if (ActiveScreenCount == ItemsPerPage)
        {
            UpdateButtons();
            return;
        }
        ActiveScreenCount++;
        this[ActiveScreenCount - 1].ScreenUser = user;
        this[ActiveScreenCount - 1].gameObject.SetActive(true);
        StartIndex = VideoChatUsers.ChatUsers.IndexOf(this[PinnedItemCount].ScreenUser);
        EndIndex = VideoChatUsers.ChatUsers.IndexOf(this[ActiveScreenCount - 1].ScreenUser);

        Debug.Log($"Active # {ActiveScreenCount} Display {StartIndex}~{EndIndex}");
    }

    public void OnUserRemove(User user)
    {

        // check if the user is being displayed
        int screenIdx = -1;
        for(int i = 0; i < ActiveScreenCount; ++i)
        {
            if (this[i].ScreenUser == user) screenIdx = i;
        }
        if (screenIdx < 0) return;
        Debug.Log($"Removed user {user.Name}, idx #{screenIdx} from screen");

        if (pinnedUsers.Contains(user))
        {
            VideoChatScreen screen = this[screenIdx];
            screen.ForceUnpin();
        }

        int userIdx = StartIndex;

        ActiveScreenCount = PinnedItemCount;

        if (userIdx < VideoChatUsers.ChatUsers.Count)
        {
            Debug.Log("print from #" + userIdx + " user " + VideoChatUsers.ChatUsers[userIdx].Name);

            for (int i = PinnedItemCount; i < ItemsPerPage && userIdx < VideoChatUsers.ChatUsers.Count;)
            {
                if (pinnedUsers.Contains(VideoChatUsers.ChatUsers[userIdx]))
                {
                    userIdx++;
                    continue;
                }
                this[i].ScreenUser = VideoChatUsers.ChatUsers[userIdx];
                if (!this[i].gameObject.activeInHierarchy) this[i].gameObject.SetActive(true);
                i++;
                userIdx++;
                ActiveScreenCount++;
            }


            for (int i = ActiveScreenCount; i < ItemsPerPage; ++i) this[i].gameObject.SetActive(false);
        }

        for (int i = ActiveScreenCount; i < ItemsPerPage; ++i) this[i].gameObject.SetActive(false);

        if (ActiveScreenCount - PinnedItemCount < 1)
        {
            StartIndex = StartIndex - 1;
            Debug.Log("Removed the only item: StartIndex #" + StartIndex);
        }
        else
        {
            StartIndex = VideoChatUsers.ChatUsers.IndexOf(this[PinnedItemCount].ScreenUser);
            EndIndex = VideoChatUsers.ChatUsers.IndexOf(this[ActiveScreenCount - 1].ScreenUser);
        }
        Debug.Log($"Display {StartIndex}~{EndIndex} Active #{ActiveScreenCount} Pinned # {PinnedItemCount}");

        if (ActiveScreenCount - PinnedItemCount <= 0 && !IsFirstPage())
        {
            Debug.Log("Automatic page DOWN");
            OnPagedDown();
            return;
        }

        UpdateButtons();



    }
    // this creates an empty screen, not bound to any user, and adds it to display, returning the added screen.
    public VideoChatScreen AddEmptyScreen(bool isLocal = false)
    {
        VideoChatScreen screen;
        if (isLocal)
        {
            screen = Instantiate(Resources.Load<VideoChatScreen>("UI/Popup/VideoChat/Screen"), transform);
        }
        else
        {
            screen = Instantiate(Resources.Load<VideoChatScreen>("UI/Popup/VideoChat/remoteScreen"),transform);
        }
        
        screen.IsLocal = isLocal;
        return screen;
    }

    #endregion

    // Test
    static int count = 0;
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 50, 30), "Add"))
        {
            VideoChatUsers.AddUser(new User("newUser" + count++));
        }
    }
}
