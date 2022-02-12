using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JK
{
    public class CameraLocation : MonoBehaviour
    {
        public Transform _whiteCamTransform;
        public WhiteBallMovement _whiteBall;
        void Update()
        {
            if(!PocketDyeNetworkManager.Instance.networked || _whiteBall._view.IsMine)
            this.transform.localPosition = _whiteCamTransform.localPosition;        
        }
    }
}
