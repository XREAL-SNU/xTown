using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleClick : MonoBehaviour, IPointerClickHandler 
{ 
    float clickTime = 0;
    public RectTransform ChatPanel;
    public RectTransform ChannelPanel;
    public void OnPointerClick(PointerEventData eventData)
    { 
        if ((Time.time - clickTime) < 0.3f)
        {
            OnMouseDoubleClick();
            clickTime = -1;
        }
        else
        {
            clickTime = Time.time;
        }
    }


    //Double Click활성화 시 실행되는 OnMouseDoubleClick
    //ChatPanel을 열고 닫는 작업을 진행함
    public void OnMouseDoubleClick()
    {
        if (ChatPanel != null)
            {
                if(ChatPanel.gameObject.activeSelf==true)
                {
                    ChatPanel.gameObject.SetActive(false);
                    ChannelPanel.gameObject.SetActive(true);
                    Debug.Log(ChatPanel.gameObject.activeSelf);
                }
                else if(ChatPanel.gameObject.activeSelf==false)
                {
                    ChatPanel.gameObject.SetActive(true);
                    ChannelPanel.gameObject.SetActive(false);
                    GameObject XbuttonRegion = GameObject.Find("XbuttonRegion");
                    XbuttonRegion.transform.SetAsFirstSibling();
                    Debug.Log(ChatPanel.gameObject.activeSelf);
                }
            }
    }
}

//DoubleClick기능을 따로 구현함, 3D Object에 먹히지는 않고 UI전용이며
//클릭하고 0.3초 이내에 다시 클릭하면 더블클릭이 활성화되면서 챗 패널을
//열고 닫을 수 있게 됨.
