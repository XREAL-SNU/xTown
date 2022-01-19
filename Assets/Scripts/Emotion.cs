using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emotion : MonoBehaviour
{
    public RectTransform EmoticonPanel;
    public Camera uiCamera;
    public RectTransform EmoticonMenu;
    public GameObject[] EmoticonItems;
    private Vector2 screenPoint;
    private Vector2 emoticonCenterPosition;
    private EmoticonShow emoShow;
    IEnumerator emoticonShow(int num)
    {
        EmoticonItems[num].gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        EmoticonItems[num].gameObject.SetActive(false);
    }
    private void Start()
    {
        EmoticonPanel = GetComponent<RectTransform>();
        uiCamera = Camera.main;
        EmoticonMenu.gameObject.SetActive(false);
        for (int i = 0; i < EmoticonItems.Length; i++)
        {
            EmoticonItems[i].gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            // t를 눌렀을 때 그 위치에 바로 메뉴를 띄우기 위해 마우스포인터 위치를 잡아줍니다.
            emoticonCenterPosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(EmoticonPanel, emoticonCenterPosition, uiCamera, out screenPoint);
            EmoticonMenu.localPosition = screenPoint;
        }
        // t를 누르고 있을 경우 이모티콘메뉴 활성화
        if(Input.GetKey(KeyCode.T)){
            EmoticonMenu.gameObject.SetActive(true);
        }
        // t를 떼면 이모티콘메뉴를 비활성화시키고 이모티콘 띄우기
        if(Input.GetKeyUp(KeyCode.T)){
            EmoticonMenu.gameObject.SetActive(false);
            StartCoroutine(emoticonShow(1));
        }
    }
}
