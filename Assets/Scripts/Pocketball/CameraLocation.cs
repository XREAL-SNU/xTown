using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
namespace JK
{
    public class CameraLocation : MonoBehaviour
    {
        public Transform _whiteCamTransform;
        public WhiteBallMovement _whiteBall;
        public PhotonView _view;
        void Start()
        {
            _view = GetComponent<PhotonView>();
        }
        void Update()
        {
            if(!PocketDyeNetworkManager.Instance.networked || _whiteBall._view.IsMine)
            this.transform.localPosition = _whiteCamTransform.localPosition;        
        }
    }
}
