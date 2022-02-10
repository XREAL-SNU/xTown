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
    private GameObject _camTarget;
    private GameObject _firstPersonCam;
    public float TopClamp = 0.6f;
    public float BottomClamp = 0.1f;
    private float _cinemachineTargetPitch;
    private float _cinemachineTargetYaw;
    private float _defaultY = 0.0f;

    private void Start()
    {
        _camTarget = LoadCharacter.Instance.PlayerControl.CinemachineCameraTarget;
        _input = LoadCharacter.Instance.PlayerControl.GetComponent<StarterAssetsInputs>();
        _useMouseToRotateTp = false;
        _useMouseToRotateFp = false;
        _firstPersonCam = CameraManager.FirstPersonCamObj;
    }
    void Update()
    {
        if (_firstPersonCam == null)
        {
            _firstPersonCam = CameraManager.FirstPersonCamObj;
        }

        if (Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.Equals))
        {
            if (FreeLookCam.m_Lens.FieldOfView < 80)
            {
                FreeLookCam.m_Lens.FieldOfView += zoomSpeed;
            }
        }

        if (Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Plus))
        {
            if (FreeLookCam.m_Lens.FieldOfView > 5)
            {
                FreeLookCam.m_Lens.FieldOfView -= zoomSpeed;
            }
        }

        if (CameraManager.IsCurrentFp)
        {
            _useMouseToRotateTp = false;
            FreeLookCam.m_XAxis.Value = 0;
            FreeLookCam.m_YAxis.Value = 0;

            if (Input.GetKey(KeyCode.Escape))
            {
                _useMouseToRotateFp = false;
            }
            if (Input.GetKey(KeyCode.R))
            {
                _useMouseToRotateFp = true;
            }
        }
        else
        {
            _useMouseToRotateFp = false;
            _firstPersonCam.transform.eulerAngles = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.Escape))
            {
                _useMouseToRotateTp = false;
            }
            if (Input.GetKey(KeyCode.R))
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

        float xRotate = Mathf.Clamp(_firstPersonCam.transform.eulerAngles.x - m_Input.x * turnSpeed, -60, 60);
        float yRotate = Mathf.Clamp(_firstPersonCam.transform.eulerAngles.y + m_Input.y * turnSpeed, -45, 80);

        _firstPersonCam.transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
    }
    private void RotateTp()
    {
        // look based on absolute screen position
        m_Input.x = Input.GetAxis("Mouse X");
        m_Input.y = Input.GetAxis("Mouse Y");
        if(m_Input.sqrMagnitude > _threshold) {
            _cinemachineTargetYaw += m_Input.x * xRotateSpeed;
            _cinemachineTargetPitch += -1 * m_Input.y * yRotateSpeed;

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
}
