using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamManager : MonoBehaviour
{
    public bool IsCurrentFp = false;

    private GameObject _player;

    public GameObject FirstPersonCamObj;
    public GameObject ThirdPersonCamObj;

    private CinemachineVirtualCamera _vCam;


    // Update is called once per frame
    void Update()
    {
        if (_player == null)
        {
            _player = PlayerManager.Players.LocalPlayerGo;
            FirstPersonCamObj = _player.transform.Find("FirstPersonCam").gameObject;
            _vCam = FirstPersonCamObj.GetComponent<CinemachineVirtualCamera>();
            _vCam.Priority = 20;
        }
        else
        {
            CamChange();
        }
    }

    private void CamChange()
    {
        /*if (PlayerKeyboard.KeyboardInput("Camera", KeyboardInput.CameraViewChange))
        {
            IsCurrentFp = !IsCurrentFp;
            FirstPersonCamObj.SetActive(IsCurrentFp);

            ThirdPersonCamObj.SetActive(!IsCurrentFp);
            FirstPersonCamObj.transform.rotation = new Quaternion(0,0,0,0);
        }*/
    }
}
