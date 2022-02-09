using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    /* 코인이 회전하도록 합니다. */
    /* 플레이어가 획득하면 사라지도록 합니다. */

    void FixedUpdate()
    {
        transform.Rotate(new Vector3(200f, 0f, 0f) * Time.deltaTime); //코인이 회전하도록 함
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false); //플레이어가 접촉(획득) 시 사라짐
    }

}
