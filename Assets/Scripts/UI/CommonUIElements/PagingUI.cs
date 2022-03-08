using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XReal.XTown.UI;

public class PagingUI : UIPopup
{
    // the page that has focus on.
    public int CurrentPageNum = 0;

    // total number of pages
    public int FinalPageNum = 0;

    protected enum Navigation_Buttons
    {
        PrevButton, NextButton, CloseButton
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Navigation_Buttons));

        GetUIComponent<Image>((int)Navigation_Buttons.CloseButton).gameObject.BindEvent(OnClick_Close);
        GetUIComponent<Image>((int)Navigation_Buttons.NextButton).gameObject.BindEvent(OnClick_PageNext);
        GetUIComponent<Image>((int)Navigation_Buttons.PrevButton).gameObject.BindEvent(OnClick_PagePrev);

        GetUIComponent<Image>((int)Navigation_Buttons.NextButton).enabled = false;
        GetUIComponent<Image>((int)Navigation_Buttons.PrevButton).enabled = false;

    }



    #region UICallbacks

    public enum PageEvents
    {
        Prev, Next
    }


    public void OnClick_PagePrev(PointerEventData data)
    {
        CurrentPageNum--;
        OnPaged(PageEvents.Prev);
    }

    public void OnClick_PageNext(PointerEventData data)
    {
        CurrentPageNum++;
        OnPaged(PageEvents.Next);
    }

    public void OnClick_Close(PointerEventData data)
    {
        ClosePopup();
    }
    #endregion

    #region PageHandlers
    Action<PageEvents> OnPagedHandler = null;
    public virtual void OnPaged(PageEvents eventType)
    {
        if(OnPagedHandler != null)
        {

            OnPagedHandler.Invoke(eventType);
        }
    }

    public void AddListener(Action<PageEvents> action)
    {
        OnPagedHandler -= action;
        OnPagedHandler += action;
    }
    public void RemoveListener(Action<PageEvents> action)
    {
        OnPagedHandler -= action;
    }
    #endregion
}
