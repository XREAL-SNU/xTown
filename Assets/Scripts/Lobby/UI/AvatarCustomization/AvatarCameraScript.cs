using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AvatarCameraScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    private bool _isEnter;
    private bool _isExit;

    private void Start()
    {
        if(Avatar is null)
        {
            Avatar = GameObject.FindGameObjectWithTag("Player").transform;
            if (!Avatar) Debug.LogError("AvatarCameraScript/ make sure to assign Avatar");
        }
        ResetCam();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isEnter = true;
        _isExit = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isExit = true;
        if (!Input.GetMouseButton(1))
        {
            _isEnter = false;
        }
    }

    private void Update()
    {
        if (!_isMoving && _isEnter)
        {
            _camMove();
            _zoom();
        }
        if (_isExit && Input.GetMouseButtonUp(1))
        {
            _isEnter = false;
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
        _wheel = -6;
        _mouseX = 0;
        _mouseY = 0;
        _isMoving = false;
        _isEnter = false;
        Avatar.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
        CentralAxis.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Cam.localPosition = new Vector3(0, 0, _wheel);
    }

    public void ResetCamBtn()
    {
        _isMoving = true;
        _wheel = -6;
        _mouseX = 0;
        _mouseY = 0;
        StartCoroutine("_smoothMoving");
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
        Cam.localPosition = new Vector3(0, 0, _wheel);
        _isMoving = false;
    }
}
