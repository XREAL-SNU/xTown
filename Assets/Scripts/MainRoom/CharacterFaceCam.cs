using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFaceCam : MonoBehaviour
{
    public GameObject _player;
    private float CameraDistance = 0.9f;
    void Start()
    {
        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player");
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = _player.transform.position + _player.transform.forward * CameraDistance;
        transform.LookAt(_player.transform.position);
        transform.position = new Vector3 (transform.position.x, transform.position.y + 1.0f, transform.position.z);
    }
}
