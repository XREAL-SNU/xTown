using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class CameraControl : MonoBehaviour
{
    public CinemachineFreeLook FreeLookCam;
    public CamManager CameraManager;
    public float zoomSpeed = 0.1f;
    public float xRotateSpeed = 2.0f;
    public float yRotateSpeed = 0.1f;
    public float xRotateDecelerate = 2.0f;
    public float turnSpeed = 2.0f;

    // inputs
    private Vector2 m_Input;
    private StarterAssetsInputs _input;
    private float _threshold = 0.01f;
    private bool _useMouseToRotateTp;
    private bool _useMouseToRotateFp;

    // camera settings
    private GameObject _player;
    private GameObject _camTarget;
    private GameObject _firstPersonCam;
    public float TopClamp = 0.6f;
    public float BottomClamp = 0.1f;
    private float _cinemachineTargetPitch;
    private float _cinemachineTargetYaw;
    private float _defaultY = 0.0f;
    private bool _alreadyFront;

    private void Start()
    {
        // Initial Rotation Speed
        // Edit Here
        xRotateSpeed = 7f;
        yRotateSpeed = 5f;
        xRotateDecelerate = 2f;

        //_player = GameObject.FindWithTag("Player");
        _player = PlayerManager.Players.LocalPlayerGo;
        _camTarget = _player.GetComponent<ThirdPersonControllerMulti>().CinemachineCameraTarget;
        _input = _player.GetComponent<ThirdPersonControllerMulti>().GetComponent<StarterAssetsInputs>();
        _useMouseToRotateTp = false;
        _useMouseToRotateFp = false;
        _firstPersonCam = CameraManager.FirstPersonCamObj;

        // Set initial values for rotation
        _cinemachineTargetYaw = FreeLookCam.m_XAxis.Value;
        _cinemachineTargetPitch = FreeLookCam.m_YAxis.Value;
    }
    void Update()
    {
        if (_firstPersonCam == null)
        {
            _firstPersonCam = CameraManager.FirstPersonCamObj;
            return;
        }

        if (Input.mouseScrollDelta.y < 0 && PlayerMouse.MouseAvailable)
        {
            if (FreeLookCam.m_Lens.FieldOfView < 80)
            {
                Debug.Log("Scroll Up");
                FreeLookCam.m_Lens.FieldOfView += 5f;
            }
        }

        if (Input.mouseScrollDelta.y > 0 && PlayerMouse.MouseAvailable)
        {
            if (FreeLookCam.m_Lens.FieldOfView > 5)
            {
                Debug.Log("Scroll Down");
                FreeLookCam.m_Lens.FieldOfView -= 5f;
            }
        }

        if (CameraManager.IsCurrentFp)
        {
            _useMouseToRotateTp = false;

            if (PlayerMouse.MouseInputSet(MouseInput.CameraDragExit))
            {
                _cinemachineTargetYaw =   FreeLookCam.m_XAxis.Value;
                _cinemachineTargetPitch = FreeLookCam.m_YAxis.Value;

                _useMouseToRotateFp = false;
            }
            if (PlayerMouse.MouseInputSet(MouseInput.CameraDrag))
            {
                _useMouseToRotateFp = true;
            }
        }
        else
        {
            _useMouseToRotateFp = false;
            _firstPersonCam.transform.eulerAngles = new Vector3(0, 0, 0);
            if (PlayerMouse.MouseInputSet(MouseInput.CameraDragExit))
            {
                _cinemachineTargetYaw =   FreeLookCam.m_XAxis.Value;
                _cinemachineTargetPitch = FreeLookCam.m_YAxis.Value;

                _useMouseToRotateTp = false;
            }
            if (PlayerMouse.MouseInputSet(MouseInput.CameraDrag))
            {
                _useMouseToRotateTp = true;
            }
        }
    }

    private void LateUpdate()
    {
        if (_useMouseToRotateTp) RotateTp();
        //if (_useMouseToRotateFp) RotateFp();
    }

    private void RotateFp()
    {
        m_Input.x = Input.GetAxis("Mouse X");
        m_Input.y = Input.GetAxis("Mouse Y");

        float xRotate = Mathf.Clamp(_firstPersonCam.transform.eulerAngles.x - m_Input.x * turnSpeed * Time.deltaTime, -60, 60);
        float yRotate = Mathf.Clamp(_firstPersonCam.transform.eulerAngles.y + m_Input.y * turnSpeed * Time.deltaTime, -45, 80);

        _firstPersonCam.transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
    }
    private void RotateTp()
    {
        // look based on absolute screen position
        m_Input.x = Input.GetAxis("Mouse X");
        m_Input.y = Input.GetAxis("Mouse Y");
        if(m_Input.sqrMagnitude > _threshold) {
            _cinemachineTargetYaw += m_Input.x * xRotateSpeed * Time.deltaTime;
            _cinemachineTargetPitch += -1 * m_Input.y * yRotateSpeed * Time.deltaTime;

            // clamp to 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
        }
        else if (_cinemachineTargetYaw > 0.1f)
        {
            _cinemachineTargetYaw -= xRotateDecelerate * Time.deltaTime;
        }
        else if (_cinemachineTargetYaw < -0.1f)
        {
            _cinemachineTargetYaw += xRotateDecelerate * Time.deltaTime;
        }
        else
        {
            _cinemachineTargetYaw = 0f;
        }
        

        /*
        // look based on delta position
        if (_input.look.sqrMagnitude >= _threshold)
        {
            _cinemachineTargetYaw += _input.look.x * xRotateSpeed * Time.deltaTime;
            _cinemachineTargetPitch += _input.look.y * yRotateSpeed * Time.deltaTime;

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        }
        else if (_cinemachineTargetYaw > 0.1f)
        {
            _cinemachineTargetYaw -= xRotateDecelerate * Time.deltaTime;
        }
        else if (_cinemachineTargetYaw < -0.1f)
        {
            _cinemachineTargetYaw += xRotateDecelerate * Time.deltaTime;
        }
        else
        {
            _cinemachineTargetYaw = 0f;
        }
        */
        FreeLookCam.m_XAxis.Value = _cinemachineTargetYaw;
        FreeLookCam.m_YAxis.Value = _cinemachineTargetPitch;
    }
    
    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void SetFront(){
        if(_alreadyFront){
            _alreadyFront = false;
            return;
        }
        _useMouseToRotateTp = false;
        Vector3 _playerPos = _camTarget.gameObject.transform.position;
        Vector3 _playerForward = _camTarget.gameObject.transform.forward;
        StartCoroutine(camSetFront(_playerPos,_playerForward));
        // FreeLookCam.ForceCameraPosition(_playerPos + _playerForward * FreeLookCam.m_Orbits[1].m_Radius,_camTarget.gameObject.transform.rotation);
        // FreeLookCam.VirtualCameraGameObject.transform.position = _playerPos + (_playerForward * FreeLookCam.m_Orbits[1].m_Radius);
        // FreeLookCam.m_YAxis.Value = 0.4f;
        _alreadyFront = true;
        
    }
    IEnumerator camSetFront(Vector3 _playerPos, Vector3 _playerForward){
        // float _angle = (180/Mathf.PI) * Mathf.Atan2(FreeLookCam.VirtualCameraGameObject.transform.position.x-(_playerPos + (_playerForward * FreeLookCam.m_Orbits[1].m_Radius)).x,FreeLookCam.VirtualCameraGameObject.transform.position.z-(_playerPos + (_playerForward * FreeLookCam.m_Orbits[1].m_Radius)).z);
        // if(_angle>180f) _angle-=360f;
        // float _distance = Mathf.Abs(FreeLookCam.VirtualCameraGameObject.transform.position.y - _playerPos.y);
        Vector3 _camPos=FreeLookCam.VirtualCameraGameObject.transform.position;
        while(Vector3.Distance(FreeLookCam.VirtualCameraGameObject.transform.position,_playerPos + _playerForward * FreeLookCam.m_Orbits[1].m_Radius)>1f){
            _camPos = Vector3.Lerp(_camPos,_playerPos + _playerForward * FreeLookCam.m_Orbits[1].m_Radius,Time.deltaTime*3f);
            FreeLookCam.ForceCameraPosition(_camPos,_camTarget.gameObject.transform.rotation);
            // FreeLookCam.m_XAxis.Value = Mathf.Lerp(FreeLookCam.m_XAxis.Value,_angle,Time.deltaTime*2f);
            // FreeLookCam.m_YAxis.Value = Mathf.Lerp(FreeLookCam.m_YAxis.Value,0.33f,Time.deltaTime*_distance);
            yield return null;
        }
        
    }
}
