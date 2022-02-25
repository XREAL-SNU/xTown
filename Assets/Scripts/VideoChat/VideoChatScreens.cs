using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoChatScreens: MonoBehaviour
{
    List<VideoChatScreen> _screens = new List<VideoChatScreen>();

    public bool PinMyVideoFirst;
    public int PinnedItemCount;
    public int ItemsPerPage;

    Transform _poolParent;

    #region MonobehaviourCallbacks
    private void Awake()
    {
        // I would prefer to construct this parent as early as possible.
        _poolParent = new GameObject("ScreenPool").transform;
        _poolParent.SetParent(transform.parent);
    }
    private void Start()
    {
        PagingUI pagingUI = GetComponentInParent<PagingUI>();
        // pagingUI.FinalPageNum = [Get count of users here!!!] / (ItemsPerPage - PinnedItemCount);
        ReloadScreensOnPage(0);
    }

    private void OnEnable()
    {
        PagingUI pagingUI = GetComponentInParent<PagingUI>();
        pagingUI.AddListener(OnPaged);
    }
    private void OnDisable()
    {
        PagingUI pagingUI = GetComponentInParent<PagingUI>();
        pagingUI.RemoveListener(OnPaged);
    }
    #endregion

    public int CurrentPageNum = 0;

    // callbacks
    public void OnPaged(int newPageNum)
    {
        CurrentPageNum = newPageNum;
        ReloadScreensOnPage(newPageNum);
    }

    // update the current page
    public void ReloadScreensOnPage(int currentPageNum)
    {
        Debug.Log("RELOADING PAGE "+ currentPageNum);
        // test parameters
        int usersCount = 20;
        //

        GetComponentInParent<PagingUI>().FinalPageNum = usersCount / (ItemsPerPage - PinnedItemCount);
        int screensToDisplay = currentPageNum == usersCount / (ItemsPerPage - PinnedItemCount) ? usersCount % ItemsPerPage: ItemsPerPage;

        /* TODO
         * set number of screens to be displayed using AddEmptyScreen or RemoveScreen(predicate)
         * set which screens will be used by whom (set for user n)
         */
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
        screen.enabled = false;

        _screens.Add(screen);
        GetComponentInParent<PagingUI>().FinalPageNum = _screens.Count / (ItemsPerPage - PinnedItemCount);

        Debug.Log($"VideoChatScreens/ Added screen, #{_screens.Count}");
        return screen;
    }

    public int RemoveScreen(Predicate<VideoChatScreen> predicate)
    {
        List<VideoChatScreen> screensToRemove =  _screens.FindAll(predicate);
        if (screensToRemove.Count == 0)
        {
            Debug.LogWarning("VideoChatScreens/ Nothing to remove!");
            return 0;
        }
        
        // remove from list
        _screens.RemoveAll(predicate);

        // remove the screen itself.
        screensToRemove.ForEach((item) => {
            item.Destroy();
        });

        // return number of screens removed
        return screensToRemove.Count;
    }


    // Test
    static int count = 0;
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 50, 30), "Add"))
        {
            AddEmptyScreen(false).SetNickName("newUser" + count++);
        }
    }
}
