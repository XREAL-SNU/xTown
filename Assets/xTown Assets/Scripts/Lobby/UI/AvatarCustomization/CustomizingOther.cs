using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XReal.XTown.UI;

public class CustomizingOther : UIScene
{
    enum GameObjects
    {
        CameraController,
        BackButton,
        DoneButton,
        CentralAxis,
        AvatarViewCam,
        CharacterPrefabPreview
    }

    private static GameObject _previewAvatarObject;
    private GameObject _centralAxis;
    private GameObject _cam;
    private GameObject _avatar;

    [SerializeField]
    private float _camSpeed = 10;

    private float _mouseX;
    [SerializeField]
    private float _wheel;
    private bool _isEnter;
    private bool _isExit;

    private void Awake()
    {
        if (_previewAvatarObject is null)
        {
            _previewAvatarObject = GameObject.FindWithTag("Player");
            PlayerManager.Players.LocalPlayerGo = _previewAvatarObject;
        }
    }

    private void Start()
    {
        Init();
        CameraReset();
    }

    private void Update()
    {
        if (_isEnter)
        {
           CameraMove();
           CameraZoom();
        }
        if (_isExit && Input.GetMouseButtonUp(1))
        {
            _isEnter = false;
        }
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        _centralAxis = GetUIComponent<GameObject>((int)GameObjects.CentralAxis);
        _cam = GetUIComponent<GameObject>((int)GameObjects.AvatarViewCam);
        _avatar = GetUIComponent<GameObject>((int)GameObjects.CharacterPrefabPreview);

        GetUIComponent<GameObject>((int)GameObjects.CameraController).gameObject.BindEvent(CameraEnter, UIEvents.UIEvent.Enter);
        GetUIComponent<GameObject>((int)GameObjects.CameraController).gameObject.BindEvent(CameraExit, UIEvents.UIEvent.Exit);

        GetUIComponent<GameObject>((int)GameObjects.DoneButton).gameObject.BindEvent(DoneButton);
        GetUIComponent<GameObject>((int)GameObjects.DoneButton).gameObject.BindEvent(OnButtonExit);
        GetUIComponent<GameObject>((int)GameObjects.DoneButton).gameObject.BindEvent(OnButtonEnter, UIEvents.UIEvent.Enter);
        GetUIComponent<GameObject>((int)GameObjects.DoneButton).gameObject.BindEvent(OnButtonExit, UIEvents.UIEvent.Exit);
        
        GetUIComponent<GameObject>((int)GameObjects.BackButton).gameObject.BindEvent(CameraResetButton);
        GetUIComponent<GameObject>((int)GameObjects.BackButton).gameObject.BindEvent(BackButton);
        GetUIComponent<GameObject>((int)GameObjects.BackButton).gameObject.BindEvent(OnButtonExit);
        GetUIComponent<GameObject>((int)GameObjects.BackButton).gameObject.BindEvent(OnButtonEnter, UIEvents.UIEvent.Enter);
        GetUIComponent<GameObject>((int)GameObjects.BackButton).gameObject.BindEvent(OnButtonExit, UIEvents.UIEvent.Exit);

    }


    public void CameraEnter(PointerEventData data)
    {
        _isEnter = true;
        _isExit = false;
    }

    public void CameraExit(PointerEventData data)
    {
        _isExit = true;
        if (!Input.GetMouseButton(1))
        {
            _isEnter = false;
        }
    }

    public void CameraMove()
    {
        if (Input.GetMouseButton(1))
        {
            _mouseX += Input.GetAxis("Mouse X");

            _centralAxis.transform.rotation = Quaternion.Euler(new Vector3(_centralAxis.transform.rotation.x, _centralAxis.transform.rotation.y + _mouseX, 0) * _camSpeed);
        }
    }

    public void CameraZoom()
    {
        _wheel += Input.GetAxis("Mouse ScrollWheel");
        if (_wheel >= -0.8f)
        {
            _wheel = -0.8f;
        }
        if (_wheel <= -3.5f)
        {
            _wheel = -3.5f;
        }
        _cam.transform.localPosition = new Vector3(0, 0, _wheel);
    }

    public void CameraReset()
    {
        _wheel = -2;
        _mouseX = 0;
        _isEnter = false;
        _avatar.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
        _centralAxis.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        _cam.transform.localPosition = new Vector3(0, 0, _wheel);
    }

    public void CameraResetButton(PointerEventData data)
    {
        CameraReset();
    }

    public void DoneButton(PointerEventData data)
    {
        Debug.Log("Done");
        AvatarSelectionMenu menu = GetComponentInParent<AvatarSelectionMenu>();
        if(menu is null)
        {
            Debug.LogError("AvatarSelectionMenu is null");
            return;
        }
        menu.OnClick_JoinWorld();
    }

    public void BackButton(PointerEventData data)
    {
        Debug.Log("Back");
        AvatarSelectionMenu menu = GetComponentInParent<AvatarSelectionMenu>();
        if (menu is null)
        {
            Debug.LogError("AvatarSelectionMenu is null");
            return;
        }
        menu.OnClick_BackPlayerNameInputMenu();
    }

    public void OnButtonEnter(PointerEventData data)
    {
        GameObject btn = data.pointerEnter.transform.parent.gameObject;
        if (btn.GetComponent<Image>() != null)
        {
            btn.GetComponent<Image>().color = XTownColor.XTownGrey.ToColor();
        }
    }

    public void OnButtonExit(PointerEventData data)
    {
        GameObject btn = data.pointerEnter.transform.parent.gameObject;
        if (btn.GetComponent<Image>() != null)
        {
            btn.GetComponent<Image>().color = XTownColor.XTownWhite.ToColor();
        }
    }
}
