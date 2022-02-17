using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class BoatMovement : MonoBehaviour
{
    private CinemachineDollyCart _dollyCart;
    private int _moveDirection;
    private float _moveSpeed = 0.008f;
    private bool _isWaiting;

    // Start is called before the first frame update
    void Start()
    {
        _dollyCart = GetComponent<CinemachineDollyCart>();
        _moveDirection = 1;
    }

    void FixedUpdate()
    {
        if (_isWaiting)
        {
            return;
        }
        _dollyCart.m_Position += _moveDirection * _moveSpeed * Time.deltaTime;
        if (_dollyCart.m_Position <= 0 || _dollyCart.m_Position >= 1)
        {
            StartCoroutine(WaitAndChangeDirection());
        }
    }

    IEnumerator WaitAndChangeDirection()
    {
        _isWaiting = true;
        yield return new WaitForSeconds(3);
        _isWaiting = false;
        _moveDirection *= -1;

    }

    private void CartSpeed(float speed)
    {
        _dollyCart.m_Speed = speed;
    }
}
