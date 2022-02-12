using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
namespace JK
{
    public class CameraScript : MonoBehaviour
    {
        public CinemachineVirtualCamera virCam = null;
        public void SetCameraWhole()
        {
            virCam.Priority = 11;
        }
        public void SetCameraCue()
        {
            virCam.Priority = 9;
        }
    }
}