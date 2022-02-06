using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class Emotion : MonoBehaviour
{
    private Camera _camera;
    public RectTransform EmoticonMenu;
    [Header("MenuUI")]
    public Image[] MenuSlice;
    public static int _currentMenu=-1;
    private bool _isMenuActive = false;

    private PhotonView _view;


    private void Start()
    {
        EmoticonMenu = GameObject.Find("EmoticonMenu").GetComponent<RectTransform>();
        EmoticonMenu.gameObject.SetActive(false);
        _camera = Camera.main;
        // for (int i = 0; i < EmoticonItems.Length; i++)
        // {
        //     Debug.Log(EmoticonItems[i].name);
        //     EmoticonItems[i].gameObject.SetActive(false);
        // }

        // netcode
        _view = GetComponent<PhotonView>();

    }
    private void OnEnable() {
        for(int i = 0;i<4;i++){
            // MenuSlice[i].transform.GetChild(0).GetComponent<Image>().sprite = PlayerAvatar.localPlayer.emoticon[i];
        }
    }

    private void Update()
    {
        if (_view is null || !_view.IsMine) return;
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(!_isMenuActive){
                EmoticonMenu.gameObject.SetActive(true);
                _isMenuActive = true;
            }else{
                EmoticonMenu.gameObject.SetActive(false);
                _isMenuActive = false;
            }
        }
        if(_isMenuActive){
            if(Input.GetKeyDown(KeyCode.Alpha1)){
                _currentMenu = 1;
            } else if(Input.GetKeyDown(KeyCode.Alpha2)){
                _currentMenu = 2;
            } else if(Input.GetKeyDown(KeyCode.Alpha3)){
                _currentMenu = 3;
            } else if(Input.GetKeyDown(KeyCode.Alpha4)){
                _currentMenu = 4;
            }
            Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x,transform.position.y+2f,transform.position.z));
            EmoticonMenu.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(screenPos.x,screenPos.y,transform.position.z);
            if(_currentMenu!=-1){
                EmoticonSelect(_currentMenu);
            }
        }

        // // t를 누르고 있을 경우 이모티콘메뉴 활성화
        // if(Input.GetKey(KeyCode.T)){
        //     EmoticonMenu.gameObject.SetActive(true);
        //     checkCurrentMenu();
        //     if(_currentMenu != _previousMenu){
        //         MenuSlice[_currentMenu].color = Color.gray;
        //         MenuSlice[_previousMenu].color = Color.white;
        //         _previousMenu = _currentMenu;
        //     }
        // }
        // t를 떼면 이모티콘메뉴를 비활성화시키고 이모티콘 띄우기
        // if(Input.GetKeyUp(KeyCode.T)){
            // EmoticonMenu.gameObject.SetActive(false);
            // if(_currentMenu != EmoticonItems.Length){
            //     MenuSlice[_currentMenu].color = Color.white;
            //     _view.RPC("DisplayEmoticon", RpcTarget.All, _currentMenu);
            // }
        // }
    }
    private void EmoticonSelect(int num){
        Debug.Log(num);
        StartCoroutine(ChangeFace(num));
        EmoticonMenu.gameObject.SetActive(false);
        _isMenuActive = false;
        _currentMenu = -1;
    }


    // [PunRPC]
    // private void DisplayEmoticon(int emojiId)
    // {
    //     StartCoroutine(EmoticonShow(emojiId));
    // }



    IEnumerator ChangeFace(int num)
    {
        // for (int i = 0; i < EmoticonItems.Length; i++)
        // {
        //     EmoticonItems[i].gameObject.SetActive(false);
        // }
        // EmoticonItems[num].gameObject.SetActive(true);
        yield return new WaitForSeconds(10f);
        // EmoticonItems[num].gameObject.SetActive(false);
    }

    // private void checkCurrentMenu()
    // {
    //     _currentMousePosition = new Vector2(Input.mousePosition.x - _emoticonCenterPosition.x, Input.mousePosition.y - _emoticonCenterPosition.y);
    //     // 마우스의 현재위치가 많이 안 움직였다면 취소합니다.
    //     if(Mathf.Pow(_currentMousePosition.x,2) + Mathf.Pow(_currentMousePosition.y,2)>1000f){
    //         _currentAngle = Mathf.Atan2(_currentMousePosition.y, _currentMousePosition.x) * Mathf.Rad2Deg;
    //         _currentAngle = (_currentAngle + 360) % 360;
    //         _currentMenu = (int)_currentAngle / (360/(MenuSlice.Length-1));
    //     } else {
    //         _currentMenu = EmoticonItems.Length;
    //     }
    // }
} 