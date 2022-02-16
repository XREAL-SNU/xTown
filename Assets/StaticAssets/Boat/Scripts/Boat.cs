using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour, IPunObservable
{
    PhotonView _view;
    GameObject _seats;

    CinemachineDollyCart _dollyCartGo;
    void Start()
    {
        _view = GetComponent<PhotonView>();
        _seats = transform.Find("Boat DollyCart/Boat/Seats").gameObject;
        _dollyCartGo = transform.Find("Boat DollyCart").GetComponent<CinemachineDollyCart>(); ;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Boat is a "scene object", and the master client controls this.
            stream.SendNext(_dollyCartGo.m_Position);
        }
        else
        {
            // Non-network players will receive the cart position
            _dollyCartGo.m_Position = (float)stream.ReceiveNext();
        }
    }

    public void SyncIsOccupied(bool isOccupied, int spot)
    {
        Debug.Log($"Boat/SyncOccupied: spot#{spot} occuppied? {isOccupied}");
        _view.RPC("SyncIsOccupiedRPC", RpcTarget.Others, isOccupied, spot);
    }

    [PunRPC]
    public void SyncIsOccupiedRPC(bool isOccupied, int spot)
    {
        Debug.Log($"Boat/RPC: spot#{spot} occuppied? {isOccupied}");
        _seats.transform.GetChild(spot).GetComponent<BoatSeat>().IsOccupied = isOccupied;
    }

    public void SyncEmbark(bool riding, int spot)
    {
        _view.RPC("SyncEmbarkRPC", RpcTarget.Others, riding, spot);
    }

    [PunRPC]
    public void SyncEmbarkRPC(bool riding, int spot, PhotonMessageInfo info){
        Debug.Log($"RPC/sender{info.Sender.NickName} riding? {riding} #{spot}");
        Transform playerTransform = RoomManager.Room.GetComponentInPlayerById<Transform>(info.Sender.ActorNumber);
        if (riding)
        {
            Transform seatTransform = _seats.transform.GetChild(spot);
            playerTransform.SetParent(seatTransform);
        }
        else
        {
            playerTransform.SetParent(null);
        }
    }
}
