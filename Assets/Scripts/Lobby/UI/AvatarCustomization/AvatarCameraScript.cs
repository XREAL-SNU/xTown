using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCameraScript : MonoBehaviour
{
    public Transform CentralAxis;
    public Transform Cam;
    public Transform Avatar;

    public float CamSpeed;
    public float SmoothRate;

    private float _mouseX;
    private float _mouseY;
    private float _wheel;
    private bool _isMoving;

    private void Start()
    {
        _wheel = -6;
        _mouseX = 0;
        _mouseY = 0;
        _isMoving = false;
    }

    private void Update()
    {
        if (!_isMoving)
        {
            _camMove();
            _zoom();
        }
        
    }

    private void _camMove()
    {
        if (Input.GetMouseButton(1))
        {
            _mouseX += Input.GetAxis("Mouse X");
            _mouseY += Input.GetAxis("Mouse Y") * -1;

            CentralAxis.rotation = Quaternion.Euler(
                new Vector3(CentralAxis.rotation.x + _mouseY, CentralAxis.rotation.y + _mouseX, 0) * CamSpeed);
        }
    }

    private void _zoom()
    {
        _wheel += Input.GetAxis("Mouse ScrollWheel");
        if (_wheel >= -1)
        {
            _wheel = -1;
        }
        if (_wheel <= -9)
        {
            _wheel = -9;
        }
        Cam.localPosition = new Vector3(0, 0, _wheel);
    }

    public void ResetCam()
    {
        _isMoving = true;
        _wheel = -6;
        _mouseX = 0;
        _mouseY = 0;
        StartCoroutine("_smoothMoving");
        //CentralAxis.rotation = Quaternion.Euler(new Vector3(0, Avatar.eulerAngles.y - 180, 0));
    }

    IEnumerator _smoothMoving()
    {
        float avatarcurrAngle = Avatar.eulerAngles.y;
        float camcurrAngle = CentralAxis.eulerAngles.y;
        float posZ = Cam.localPosition.z;
        int count = 0;
        while (CentralAxis.rotation.y != Avatar.rotation.y - 1 || CentralAxis.localPosition.x != 0 || CentralAxis.localPosition.y != 1.7f)
        {
            count++;
            avatarcurrAngle = Mathf.LerpAngle(avatarcurrAngle, -180, SmoothRate * Time.deltaTime);
            camcurrAngle = Mathf.LerpAngle(camcurrAngle, 0, SmoothRate * Time.deltaTime);
            posZ = Mathf.Lerp(posZ, _wheel, SmoothRate * Time.deltaTime);
            Avatar.rotation = Quaternion.Euler(0, avatarcurrAngle, 0);
            CentralAxis.rotation = Quaternion.Euler(0, camcurrAngle, 0);
            Cam.localPosition = new Vector3(0, 0, posZ);
            if (count > 80)
            {
                break;
            }
            yield return null;
        }
        Avatar.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
        CentralAxis.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Debug.Log(CentralAxis.rotation);
        Debug.Log("(x,y)=" + _mouseX + _mouseY);
        Cam.localPosition = new Vector3(0, 0, _wheel);
        _isMoving = false;
    }
}
