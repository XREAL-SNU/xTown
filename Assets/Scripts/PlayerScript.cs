using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScript : MonoBehaviourPun, IPunObservable
{

    public int value, valuePerClick, clickUpgradeCost, clickUpgradeAdd,
        autoUpgradeCost, autoUpgradeAdd, valuePerSecond;
    NetworkManager NM;
    PhotonView PV;


    void Start()
    {
        PV = photonView;
        NM = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(value);
        }
        else value = (int)stream.ReceiveNext();
    }

    void Update()
    {
        if (!PV.IsMine) return;

        NM.ValueText.text = value.ToString();
    }

}
