using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using StarterAssets;

public class BoatSeat : MonoBehaviour
{
    [SerializeField]
    private BoatIconCanvas _iconCanvas;
    [SerializeField]
    private BoatButtonCanvas _buttonCanvas;

    private float _embarkRange = 10;
    private bool _isOccupied;

    private void Start()
    {
        Debug.Log(transform.rotation.eulerAngles.y);
        _iconCanvas = transform.GetChild(0).GetComponent<BoatIconCanvas>();
        _iconCanvas.seat = this;
        _iconCanvas.gameObject.SetActive(false);
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
        _iconCanvas.Hide();

        GameObject player = PlayerManager.Players.LocalPlayerGo;
        player.transform.parent = gameObject.transform;
        player.transform.localPosition = Vector3.zero;
        player.transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
        player.GetComponent<ThirdPersonControllerMulti>().enabled = false;
        player.GetComponent<Animator>().SetBool("Grounded", true);
        player.GetComponent<Animator>().SetBool("Sitting", true);

        _buttonCanvas.gameObject.SetActive(true);
    }

    public void Disembark(Vector3 disembarkLocation)
    {
        _isOccupied = false;

        GameObject player = PlayerManager.Players.LocalPlayerGo;
        player.transform.SetParent(null);
        player.GetComponent<ThirdPersonControllerMulti>().enabled = true;
        player.GetComponent<Animator>().SetBool("Sitting", false);
        Debug.Log(player.transform.parent);
        player.transform.position = disembarkLocation;

        _buttonCanvas.gameObject.SetActive(false);
        _buttonCanvas.Hide();
    }
}
