using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOZone : MonoBehaviour
{
    public GameObject _interactionCanvas;
    public UFOZonePanel _ufoZonePanel;
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            _interactionCanvas.gameObject.SetActive(!_interactionCanvas.gameObject.activeSelf);
            _ufoZonePanel.gameObject.SetActive(!_ufoZonePanel.gameObject.activeSelf);
            _ufoZonePanel._player = col.gameObject;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            _interactionCanvas.gameObject.SetActive(false);
            _ufoZonePanel.gameObject.SetActive(false);
        }
    }
}
