using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameZonePanel : MonoBehaviour
{
    public GameObject _interactionCanvas;
    public GameObject _player;
    public GameObject gameZoneSpawnPoint;
    public CharacterController _playerController;
    private Vector3 targetDirection;

    public void OnClickYes()
    {
        _playerController.enabled = false;
        _player.transform.position = gameZoneSpawnPoint.transform.position;
        _playerController.enabled = true;
        //Hide();
    }
    public void OnClickNo()
    {
        Hide();
    }
    void Hide()
    {
        _interactionCanvas.SetActive(false);
        gameObject.SetActive(false);
    }
}
