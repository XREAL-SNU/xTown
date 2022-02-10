using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class Emotion : MonoBehaviour
{
    private Transform Player;
    public RectTransform EmoticonMenu;
    [Header("MenuUI")]
    public Image[] MenuSlice;
    public GameObject Face;
    private int _currentMenu=0;
    private bool _isMenuActive = false;
    private List<AvatarFaceButton> _faceList;
    private bool _isSelected;
    private AvatarFaceControl _avatarFaceControl;
    private GameObject _camManager;
    private CameraControl _characterCam;
    private bool _isCurrentFp;
    public Transform _faceCam;
    private bool _crRunning;
    private IEnumerator _coroutine;
    private PhotonView _view;

    private void Start()
    {
        EmoticonMenu.gameObject.SetActive(false);
        Player = GameObject.FindWithTag("Player").transform;
        _avatarFaceControl = Player.GetChild(2).GetChild(0).GetChild(1).GetComponent<AvatarFaceControl>();
        _view = GetComponent<PhotonView>();
        _camManager = GameObject.Find("CamManager");
        _characterCam = GameObject.Find("CharacterCam").GetComponent<CameraControl>();
        _faceCam = GameObject.Find("MainCanvases").transform.GetChild(4).GetChild(1);
    }

    private void Update()
    {
        // if (_view is null || !_view.IsMine) return;
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
                }
                EmoticonMenu.gameObject.SetActive(false);
                _isMenuActive = false;
            }
        }
        if(_isMenuActive){
            if(!_camManager.GetComponent<CamManager>().IsCurrentFp){
                Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(Player.position.x,Player.position.y+2f,Player.position.z));
                EmoticonMenu.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(screenPos.x,screenPos.y,Player.position.z);
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
        // if(Input.GetKeyDown(KeyCode.G)){
        //     _faceCam.gameObject.SetActive(!_faceCam.gameObject.activeSelf);
        // }
    }
    public void EmoticonSelect(int num){
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