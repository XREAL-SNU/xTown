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
            _interactionCanvas.gameObject.SetActive(true);
            _gameZonePanel.gameObject.SetActive(true);
            _gameZonePanel._player = col.gameObject;
        }
    }
}
