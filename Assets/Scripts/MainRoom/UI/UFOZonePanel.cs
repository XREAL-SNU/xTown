using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;

public class UFOZonePanel : MonoBehaviour
{
    public GameObject _interactionCanvas;
    public GameObject _player;
    public GameObject ufoZoneSpawnPoint;
    public CharacterController _playerController;
    private Vector3 targetDirection;
    private float elapsed_time = 0f;
    //public CameraControl cameraControl;
    public void OnClickYes()
    {
        _playerController.enabled = false;
        //Vector3 prev_position = _player.transform.position;
        //
        
        _player.transform.DOLocalMove(ufoZoneSpawnPoint.transform.position,2);
        _player.transform.position = ufoZoneSpawnPoint.transform.position;
        //

        _playerController.enabled = true;
        Hide();
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
