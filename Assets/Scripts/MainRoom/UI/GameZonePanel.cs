using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameZonePanel : MonoBehaviour
{
    public GameObject _interactionCanvas;
    public GameObject _player;
    public GameObject gameZoneSpawnPoint;
    private CharacterController _playerController;
    private float _speed = 50f;
    private Vector3 targetDirection;
    public void OnClickYes()
    {
        _playerController = _player.GetComponent<CharacterController>();
        _interactionCanvas.SetActive(false);
        gameObject.SetActive(false);
        //while((_player.transform.position-gameZoneSpawnPoint.transform.position).magnitude>0.1f)
        //{
            targetDirection = _player.transform.position - gameZoneSpawnPoint.transform.position;
            _playerController.SimpleMove(new Vector3(0,50,0));
        //}
    }
    public void OnClickNo()
    {
        _interactionCanvas.SetActive(false);
        gameObject.SetActive(false);
    }
}
