using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public CinemachineFreeLook FreeLookCam;
    public float zoomSpeed = 0.1f;
    public float xRotateSpeed = 2.0f;
    public float yRotateSpeed = 0.1f;
    private Vector2 m_Input;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Plus))
        {
            if (FreeLookCam.m_Lens.FieldOfView < 80)
            {
                FreeLookCam.m_Lens.FieldOfView += zoomSpeed;
            }
        }

        if (Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.Minus))
        {
            if (FreeLookCam.m_Lens.FieldOfView > 5)
            {
                FreeLookCam.m_Lens.FieldOfView -= zoomSpeed;
            }
        }

        Rotate();
    }

    private void Rotate()
    {
        if (Input.GetMouseButton(1))
        {
            m_Input.x = Input.GetAxis("Mouse X");
            m_Input.y = Input.GetAxis("Mouse Y");
            FreeLookCam.m_XAxis.Value += m_Input.x * xRotateSpeed;
            FreeLookCam.m_YAxis.Value += -1 * m_Input.y * yRotateSpeed;
        }
    }
}
