using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class Emotion : MonoBehaviour
{
    public RectTransform EmoticonPanel;
    private Camera _camera;
    public RectTransform EmoticonMenu;
    [Header("Emoticons")]
    public GameObject[] EmoticonItems;
    [Header("MenuUI")]
    public Image[] MenuSlice;

    private Vector2 _screenPoint;
    private Vector2 _emoticonCenterPosition;
    private Vector2 _currentMousePosition;
    private float _currentAngle;
    private int _currentMenu=0;
    private int _previousMenu=0;
    private float _rotSpeed = 100f;

    private PhotonView _view;


    private void Start()
    {
        EmoticonPanel = GameObject.Find("EmoticonPanel").GetComponent<RectTransform>();
        EmoticonMenu = GameObject.Find("EmoticonMenu").GetComponent<RectTransform>();
        for(int i = 0; i < 5; ++i)
        {
            MenuSlice[i] = EmoticonMenu.GetComponentsInChildren<Image>()[i];
        }
        _camera = Camera.main;
        EmoticonMenu.gameObject.SetActive(false);
        for (int i = 0; i < EmoticonItems.Length; i++)
        {
            EmoticonItems[i].gameObject.SetActive(false);
        }

        // netcode
        _view = GetComponent<PhotonView>();

    }

    private void Update()
    {
        if (_view is null || !_view.IsMine) return;
        if (Input.GetKeyDown(KeyCode.T))
        {
            // t를 처음 누른 위치 저장
            _emoticonCenterPosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(EmoticonPanel, _emoticonCenterPosition, _camera, out _screenPoint);
            EmoticonMenu.localPosition = _screenPoint;
        }

        // t를 누르고 있을 경우 이모티콘메뉴 활성화
        if(Input.GetKey(KeyCode.T)){
            EmoticonMenu.gameObject.SetActive(true);
            checkCurrentMenu();
            if(_currentMenu != _previousMenu){
                MenuSlice[_currentMenu].color = Color.gray;
                MenuSlice[_previousMenu].color = Color.white;
                _previousMenu = _currentMenu;
            }
        }
        // t를 떼면 이모티콘메뉴를 비활성화시키고 이모티콘 띄우기
        if(Input.GetKeyUp(KeyCode.T)){
            EmoticonMenu.gameObject.SetActive(false);
            if(_currentMenu != EmoticonItems.Length){
                MenuSlice[_currentMenu].color = Color.white;
                _view.RPC("DisplayEmoticon", RpcTarget.All, _currentMenu);
            }
        }
    }


    [PunRPC]
    private void DisplayEmoticon(int emojiId)
    {
        StartCoroutine(EmoticonShow(emojiId));
    }



    IEnumerator EmoticonShow(int num)
    {
        // 이모티콘을 코루틴으로 돌리면 위치추적이 처음 위치밖에 안 돼서 다음과 같이 짰습니다. 의견 있으면 알려주세요
        for (int i = 0; i < EmoticonItems.Length; i++)
        {
            EmoticonItems[i].gameObject.SetActive(false);
        }
        EmoticonItems[num].gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        EmoticonItems[num].gameObject.SetActive(false);
    }

    private void checkCurrentMenu()
    {
        _currentMousePosition = new Vector2(Input.mousePosition.x - _emoticonCenterPosition.x, Input.mousePosition.y - _emoticonCenterPosition.y);
        // 마우스의 현재위치가 많이 안 움직였다면 취소합니다.
        if(Mathf.Pow(_currentMousePosition.x,2) + Mathf.Pow(_currentMousePosition.y,2)>1000f){
            _currentAngle = Mathf.Atan2(_currentMousePosition.y, _currentMousePosition.x) * Mathf.Rad2Deg;
            _currentAngle = (_currentAngle + 360) % 360;
            _currentMenu = (int)_currentAngle / (360/(MenuSlice.Length-1));
        } else {
            _currentMenu = EmoticonItems.Length;
        }
    }
}