using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Emotion : MonoBehaviour
{
    public RectTransform EmoticonPanel;
    private Camera camera;
    public RectTransform EmoticonMenu;
    public GameObject[] EmoticonItems;
    public Image[] menuSlice;
    private Vector2 screenPoint;
    private Vector2 emoticonCenterPosition;
    private Vector2 currentMousePosition;
    private float currentAngle;
    private int currentMenu=0;
    private int previousMenu=0;
    IEnumerator emoticonShow(int num)
    {
        // 이모티콘을 코루틴으로 돌리면 위치추적이 처음 위치밖에 안 돼서 다음과 같이 짰습니다. 의견 있으면 알려주세요
        for(int i=0;i<EmoticonItems.Length;i++){
            EmoticonItems[i].gameObject.SetActive(false);
        }
        EmoticonItems[num].gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        EmoticonItems[num].gameObject.SetActive(false);
    }
    private void Start()
    {
        EmoticonPanel = GetComponent<RectTransform>();
        camera = Camera.main;
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
            // t를 처음 누른 위치 저장
            emoticonCenterPosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(EmoticonPanel, emoticonCenterPosition, camera, out screenPoint);
            EmoticonMenu.localPosition = screenPoint;
        }
        // t를 누르고 있을 경우 이모티콘메뉴 활성화
        if(Input.GetKey(KeyCode.T)){
            EmoticonMenu.gameObject.SetActive(true);
            currentMenu = getCurrentMenu();
            if(currentMenu != previousMenu){
                menuSlice[currentMenu].color = Color.gray;
                menuSlice[previousMenu].color = Color.white;
                previousMenu = currentMenu;
            }
        }
        // t를 떼면 이모티콘메뉴를 비활성화시키고 이모티콘 띄우기
        if(Input.GetKeyUp(KeyCode.T)){
            EmoticonMenu.gameObject.SetActive(false);
            if(currentMenu != EmoticonItems.Length){
                menuSlice[currentMenu].color = Color.black;
                StartCoroutine(emoticonShow(currentMenu));
            }
        }
    }
    private int getCurrentMenu(){
        currentMousePosition = new Vector2(Input.mousePosition.x - emoticonCenterPosition.x, Input.mousePosition.y - emoticonCenterPosition.y);
        // 마우스의 현재위치가 많이 안 움직였다면 취소합니다.
        if(Mathf.Pow(currentMousePosition.x,2) + Mathf.Pow(currentMousePosition.y,2)>1000f){
            currentAngle = Mathf.Atan2(currentMousePosition.y, currentMousePosition.x) * Mathf.Rad2Deg;
            currentAngle = (currentAngle + 360) % 360;
            currentMenu = (int)currentAngle / 90;
        } else {
            currentMenu = EmoticonItems.Length;
        }
        Debug.Log(currentMousePosition);
        return currentMenu;
    }
}