using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameZone : MonoBehaviour
{
    public GameObject _interactionCanvas;
    public GameZonePanel _gameZonePanel;
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            _interactionCanvas.gameObject.SetActive(!_interactionCanvas.gameObject.activeSelf);
            _gameZonePanel.gameObject.SetActive(!_gameZonePanel.gameObject.activeSelf);
            _gameZonePanel._player = col.gameObject;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            _interactionCanvas.gameObject.SetActive(false);
            _gameZonePanel.gameObject.SetActive(false);
        }
    }
}
