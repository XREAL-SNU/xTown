using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class Emotion : MonoBehaviour
{
    private Transform Player;
    private Camera _camera;
    public RectTransform EmoticonMenu;
    [Header("MenuUI")]
    public Image[] MenuSlice;
    public static int _currentMenu=-1;
    private bool _isMenuActive = false;
    public GameObject _face;
    private List<AvatarFaceButton> _faceList;
    private PhotonView _view;
    private bool _isSelect;


    private void Start()
    {
        EmoticonMenu.gameObject.SetActive(false);
        Player = GameObject.FindWithTag("Player").transform;
        _camera = Camera.main;
        _view = GetComponent<PhotonView>();
        _faceList = _face.GetComponent<AvatarFaceManagement>()._favList;
    }

    private void Update()
    {
        if (_view is null || !_view.IsMine) return;
        if (Input.GetKeyDown(KeyCode.T))
        {
            for(int i = 0;i<4;i++){
                MenuSlice[i].transform.GetChild(1).GetComponent<Text>().text = _faceList[i].GetButtonText().text;
            }
            if(!_isMenuActive){
                EmoticonMenu.gameObject.SetActive(true);
                _isMenuActive = true;
            }else{
                EmoticonMenu.gameObject.SetActive(false);
                _isMenuActive = false;
            }
        }
        if(_isMenuActive){
            Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(Player.position.x,Player.position.y+2f,Player.position.z));
            EmoticonMenu.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(screenPos.x,screenPos.y,Player.position.z);
            
            if(Input.GetKeyDown(KeyCode.Alpha1)){
                MenuSlice[1].color = Color.black;
                _currentMenu = 1;
            } else if(Input.GetKeyDown(KeyCode.Alpha2)){
                _currentMenu = 2;
            } else if(Input.GetKeyDown(KeyCode.Alpha3)){
                _currentMenu = 3;
            } else if(Input.GetKeyDown(KeyCode.Alpha4)){
                _currentMenu = 4;
            }
            if(_currentMenu!=-1){
                EmoticonSelect(_currentMenu);
            }
        }
    }
    private void EmoticonSelect(int num){
        Debug.Log("FACE :: "+_faceList[num].GetButtonImage().sprite);

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
} 