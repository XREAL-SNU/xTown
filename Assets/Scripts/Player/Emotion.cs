using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class Emotion : MonoBehaviour
{
    private Transform _player;
    public RectTransform EmoticonMenu;
    [Header("MenuUI")]
    public Image[] MenuSlice;
    public GameObject Face; //Scroll view from AvatarInteractionCanvas to find fav list
    private int _currentMenu=0;
    private bool _isMenuActive = false;
    private List<AvatarFaceButton> _faceList; // fav list
    private bool _isSelected;
    private AvatarFaceControl _avatarFaceControl; //from player suit prefab change face
    private GameObject _camManager; // to get whether first-person view or third-person view
    private CameraControl _characterCam; // main camera when third-person view
    private Transform _faceCam; // face camera when first-person view
    private bool _crRunning;
    private IEnumerator _coroutine;
    private PhotonView _view;

    private void Start()
    {
        EmoticonMenu.gameObject.SetActive(false);
        _player = PlayerManager.Players.LocalPlayerGo.transform;
        //_player = PlayerManager.Players.LocalPlayerGo.transform;
        _avatarFaceControl = _player.GetComponentInChildren<AvatarFaceControl>();
        _view = GetComponent<PhotonView>();
        _camManager = GameObject.Find("CamManager");
        _characterCam = GameObject.Find("CharacterCam").GetComponent<CameraControl>();
        _faceCam = GameObject.Find("MainCanvases").transform.GetChild(4).GetChild(1);
    }

    private void Update()
    {
        
        if (_view is null || !_view.IsMine)
        {
            //Debug.Log("View Error");
            // return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _faceList = Face.GetComponent<AvatarFaceManagement>()._favList;
            for(int i = 0;i<4;i++){
                MenuSlice[i].transform.GetChild(1).GetComponent<Text>().text = _faceList[i].GetButtonText().text;
            }
            if(!_isMenuActive){
                if(_camManager.GetComponent<CamManager>().IsCurrentFp){
                    _faceCam.gameObject.SetActive(true);
                } else {
                    _characterCam.SetFront();
                }
                EmoticonMenu.gameObject.SetActive(true);
                _isMenuActive = true;
            }else{
                if(_camManager.GetComponent<CamManager>().IsCurrentFp){
                    _faceCam.gameObject.SetActive(false);
                }else{
                    _characterCam.SetFront();
                }
                EmoticonMenu.gameObject.SetActive(false);
                _isMenuActive = false;
            }
        }

        if(_isMenuActive){
            // when menu on, calculate menu position
            if(!_camManager.GetComponent<CamManager>().IsCurrentFp){
                Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(_player.position.x,_player.position.y+2f,_player.position.z));
                EmoticonMenu.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(screenPos.x,screenPos.y,_player.position.z);
            }else{
                EmoticonMenu.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(1400,400);
            }
            
            if(Input.GetKeyDown(KeyCode.Alpha1)){
                MenuSlice[0].color = Color.black;
                _currentMenu = 0;
            } else if(Input.GetKeyDown(KeyCode.Alpha2)){
                MenuSlice[1].color = Color.black;
                _currentMenu = 1;
            } else if(Input.GetKeyDown(KeyCode.Alpha3)){
                MenuSlice[2].color = Color.black;
                _currentMenu = 2;
            } else if(Input.GetKeyDown(KeyCode.Alpha4)){
                MenuSlice[3].color = Color.black;
                _currentMenu = 3;
            }
            if(Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.Alpha3) || Input.GetKeyUp(KeyCode.Alpha4)){       
                _isSelected = true;
            }

            if(_isSelected){
                for(int i=0;i<4;i++){
                    MenuSlice[i].color = Color.white;
                }
                EmoticonSelect(_currentMenu);
            }
        }
        if(Input.GetKeyDown(KeyCode.G) && _camManager.GetComponent<CamManager>().IsCurrentFp){
            _faceCam.gameObject.SetActive(false);
        }
    }
    public void EmoticonSelect(int num){
        // if coroutine exists stop it and run new coroutine
        if(_crRunning){
            StopCoroutine(_coroutine);
        }
        _coroutine = ChangeFace(_faceList[num].GetImageIndex());
        StartCoroutine(_coroutine);
        EmoticonMenu.gameObject.SetActive(false);
        _isMenuActive = false;
        _isSelected = false;
        _currentMenu = 0;
    }


    // [PunRPC]
    // private void DisplayEmoticon(int emojiId)
    // {
    //     StartCoroutine(EmoticonShow(emojiId));
    // }



    IEnumerator ChangeFace(int faceIndex)
    {
        _crRunning = true;
        _avatarFaceControl.ChangeFace(faceIndex);
        yield return new WaitForSeconds(10f);
        _avatarFaceControl.ChangeFace(11);
        _faceCam.gameObject.SetActive(false);
        _crRunning = false;
    }
} 