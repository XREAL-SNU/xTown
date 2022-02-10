using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
public class CuePhotonScript : MonoBehaviour
{
    public PhotonView _view;
    public PhotonTransformView _transformView;
    void Start()
    {
       _view = GetComponent<PhotonView>();
       _transformView = GetComponent<PhotonTransformView>();
    }
}
