using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using StarterAssets;

public class BoatSeat : MonoBehaviour
{
    [SerializeField]
    private BoatIconCanvas _iconCanvas;

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

        Embark();
    }

    public void OnMouseEnter()
    {
        if (_isOccupied)
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

    private void Embark()
    {
        _isOccupied = true;
        _iconCanvas.Hide();

        GameObject player = PlayerManager.Players.LocalPlayerGo;
        player.transform.parent = gameObject.transform;
        player.transform.localPosition = Vector3.zero;
        player.transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
        PlayerManager.Players.LocalPlayerGo.GetComponent<ThirdPersonControllerMulti>().enabled = false;
    }

    public void Disembark()
    {
        _isOccupied = true;
    }
}
