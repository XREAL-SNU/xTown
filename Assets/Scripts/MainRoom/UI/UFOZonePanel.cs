using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        //cameraControl.SetBeneath();
        _player.transform.DOLocalMove(ufoZoneSpawnPoint.transform.position,5);
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
