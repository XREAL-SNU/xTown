using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using StarterAssets;
using Photon.Pun;

public class BoatSeat : MonoBehaviour
{
    [SerializeField]
    private BoatIconCanvas _iconCanvas;
    [SerializeField]
    private BoatButtonCanvas _buttonCanvas;

    private float _embarkRange = 10;
    private bool _isOccupied;

    public bool IsOccupied
    {
        get => _isOccupied;
        set
        {
            Debug.Log($"Boatseat/ spot#{_index} occuppied? {value}");
            _isOccupied = value;
        }
    }


    private int _index;
    private Boat _boat;
    private void Start()
    {
        Debug.Log(transform.rotation.eulerAngles.y);
        _iconCanvas = transform.GetChild(0).GetComponent<BoatIconCanvas>();
        _iconCanvas.seat = this;
        _iconCanvas.gameObject.SetActive(false);

        _index = transform.GetSiblingIndex();
        _boat = GetComponentInParent<Boat>();
    }

    public void OnMouseDown()
    {
        if (_isOccupied)
        {
            return;
        }

        GameObject player = PlayerManager.Players.LocalPlayerGo;

        // 이미 배에 타고있으면 다른 자리에 못 탐. 내리고 타야 함
        if (player.transform.parent != null)
        {
            return;
        }

        // 배와 플레이어 사이의 거리가 일정 거리 안에 있어야 배에 탈 수 있음
        if (InsideEmbarkRange() == false)
        {
            return;
        }

        Embark();
    }

    public void OnMouseEnter()
    {
        if (_isOccupied)
        {
            return;
        }

        if (InsideEmbarkRange() == false)
        {
            return;
        }

        _iconCanvas.Show();
    }

    public void OnMouseExit()
    {
        if (_isOccupied)
        {
            return;
        }

        _iconCanvas.Hide();
    }

    private bool InsideEmbarkRange()
    {
        GameObject player = PlayerManager.Players.LocalPlayerGo;
        bool b = (player.transform.position - transform.position).magnitude < _embarkRange;

        return b;
    }

    private void Embark()
    {
        _isOccupied = true;
        // netcode
        Boat boat = GetComponentInParent<Boat>();
        boat.SyncIsOccupied(true, _index);
        //
        _iconCanvas.Hide();

        GameObject player = PlayerManager.Players.LocalPlayerGo;
        player.transform.parent = gameObject.transform;
        // netcode
        boat.SyncEmbark(true, _index);
        //
        player.transform.localPosition = Vector3.zero;
        player.transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
        player.GetComponent<ThirdPersonControllerMulti>().enabled = false;
        player.GetComponent<Animator>().applyRootMotion = false;
        player.GetComponent<Animator>().SetBool("Grounded", true);
        player.GetComponent<Animator>().SetBool("Sitting", true);

        _buttonCanvas.gameObject.SetActive(true);
    }

    public void Disembark(Vector3 disembarkLocation)
    {
        _isOccupied = false;
        // netcode
        Boat boat = GetComponentInParent<Boat>();
        boat.SyncIsOccupied(false, _index);
        //

        GameObject player = PlayerManager.Players.LocalPlayerGo;
        player.transform.SetParent(null);
        // netcode
        boat.SyncEmbark(false, _index);
        //
        player.GetComponent<ThirdPersonControllerMulti>().enabled = true;
        player.GetComponent<Animator>().SetBool("Sitting", false);
        Debug.Log(player.transform.parent);
        player.transform.position = disembarkLocation;
        player.GetComponent<Animator>().applyRootMotion = true;

        _buttonCanvas.gameObject.SetActive(false);
        _buttonCanvas.Hide();
    }


}
