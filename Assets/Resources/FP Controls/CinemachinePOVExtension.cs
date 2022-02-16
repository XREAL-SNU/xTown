using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] float horizontalSpeed = 10f;
    [SerializeField] float verticalSpeed = 10f;
    [SerializeField] float clampAngle = 60f;

    private FPManager _fpManager;
    private Camera _mainCamera;
    private Vector3 _initRotation;

    protected override void Awake()
    {
        _fpManager = FPManager.Instance;

        base.Awake();
        _mainCamera = Camera.main;
        _initRotation = _mainCamera.transform.localRotation.eulerAngles;
        this.enabled = false;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if(vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                if (_initRotation == null) _initRotation = transform.localRotation.eulerAngles;
                Vector2 deltaInput = _fpManager.GetMouseDelta();
                _initRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                _initRotation.y += -deltaInput.y * horizontalSpeed * Time.deltaTime;
                _initRotation.y = Mathf.Clamp(_initRotation.y, -clampAngle, clampAngle);
                state.RawOrientation = Quaternion.Euler(_initRotation.y, _initRotation.x, 0f);
            }
        }
    }
}
