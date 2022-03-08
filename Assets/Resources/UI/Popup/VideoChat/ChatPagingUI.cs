using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPagingUI : PagingUI
{

    Image _prevBtnImage;
    public Image PrevBtnImage
    {
        get => GetUIComponent<Image>((int)Navigation_Buttons.PrevButton);
    }

    Image _nextBtnImage;

    public Image NextBtnImage
    {
        get => GetUIComponent<Image>((int)Navigation_Buttons.NextButton);
    }

}
