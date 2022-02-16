using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSit : MonoBehaviour
{
    public GameObject _player;
    public Animator anim;
    public bool isWalkingTowards = false;
    public bool isSitting = false;

    /*private void OnMouseDown()
    {
        if (!isSitting)
        {
            anim.SetTrigger("WalkTowardChair");
            isWalkingTowards = true;
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == _player)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isSitting)
            {
                anim.SetTrigger("WalkTowardChair");
                isWalkingTowards = true;
            }
        }
    }

    private void Start()
    {
        _player = PlayerManager.Players.LocalPlayerGo;
        anim = _player.GetComponent<Animator>();
    }

    private void Update()
    {
        if (isWalkingTowards)
        {
            Vector3 targetDir;
            targetDir = new Vector3(transform.position.x - _player.transform.position.x, 0f, transform.position.z - _player.transform.position.z);
            Quaternion rot = Quaternion.LookRotation(targetDir);
            _player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, rot, 0.05f);
            _player.transform.Translate(Vector3.forward * 0.03f);

            if (Vector3.Distance(_player.transform.position, this.transform.position) < 0.5)
            {
                anim.SetTrigger("SittingFront");
                _player.transform.rotation = this.transform.rotation;

                isWalkingTowards = false;
                isSitting = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && isSitting)
        {
            anim.SetTrigger("Stand");
            isSitting = false;
        }
    }
}
