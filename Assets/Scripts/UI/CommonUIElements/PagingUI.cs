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

    enum Navigation_Buttons
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
    }



    #region UICallbacks
    public void OnClick_PagePrev(PointerEventData data)
    {
        if(CurrentPageNum < 1)
        {
            Debug.LogError("PagingUI/ page # outside range");
            return;
        }
        CurrentPageNum--;
        OnPaged();
    }

    public void OnClick_PageNext(PointerEventData data)
    {
        if (CurrentPageNum >= FinalPageNum)
        {
            Debug.LogError("PagingUI/ page # outside range");
            return;
        }
        CurrentPageNum++;
        OnPaged();
    }

    public void OnClick_Close(PointerEventData data)
    {
        ClosePopup();
    }
    #endregion

    #region PageHandlers
    Action<int> OnPagedHandler = null;
    public virtual void OnPaged()
    {
        GetUIComponent<Image>((int)Navigation_Buttons.PrevButton).gameObject.SetActive(CurrentPageNum > 0);
        GetUIComponent<Image>((int)Navigation_Buttons.NextButton).gameObject.SetActive(CurrentPageNum < FinalPageNum);

        if(OnPagedHandler != null)
        {
            OnPagedHandler.Invoke(CurrentPageNum);
        }
    }

    public void AddListener(Action<int> action)
    {
        OnPagedHandler -= action;
        OnPagedHandler += action;
    }
    public void RemoveListener(Action<int> action)
    {
        OnPagedHandler -= action;
    }
    #endregion
}
