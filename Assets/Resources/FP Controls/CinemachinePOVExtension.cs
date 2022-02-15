using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] float horizontalSpeed = 10f;
    [SerializeField] float verticalSpeed = 10f;
    [SerializeField] float clampAngle = 60f;

    private FPManager fpManager;
    private Vector3 initRotation;

    protected override void Awake()
    {
        fpManager = FPManager.Instance;

        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if(vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                if (initRotation == null) initRotation = transform.localRotation.eulerAngles;
                Vector2 deltaInput = fpManager.GetMouseDelta();
                initRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                initRotation.y += -deltaInput.y * horizontalSpeed * Time.deltaTime;
                initRotation.y = Mathf.Clamp(initRotation.y, -clampAngle, clampAngle);
                state.RawOrientation = Quaternion.Euler(initRotation.y, initRotation.x, 0f);
            }
        }
    }
}
