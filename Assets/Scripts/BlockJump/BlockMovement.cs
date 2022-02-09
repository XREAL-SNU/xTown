using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    /* 플레이어가 블록에 접촉 시, 블록이 좌우(z축 방향) 또는 상하(x축 방향)로 반복하여 움직이게 합니다. */
    /* 블록이 이동할 때, 플레이어가 블록 위에서 블록을 따라 함께 움직이도록 합니다. */

    public GameObject EndPosition; //블록 방향 전환 지점
    private Vector3 startPosition; //블록 이동 시작 지점
    private GameObject Player;

    public float speed; //블록 이동 속도
    private bool isInitDir = true; //블록이 이동 중인 방향 (초기 방향이면 true, 전환된 방향이면 false)
    private bool isPlayerOnBlock; //플레이어의 블록 접촉 여부

    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        startPosition = transform.position; //블록의 초기 위치
    }

    private void OnTriggerEnter(Collider other)
    {
        isPlayerOnBlock = true;
        Player.transform.parent = transform; //플레이어가 이동하는 블록 위에서 블록과 함께 움직이게 함
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerOnBlock = false;
        Player.transform.parent = null; //플레이어가 블록과 상관없이 움직이게 함
    }

    void FixedUpdate()
    {
        /* 블록이 이동하게 하는 부분 */
        if (isPlayerOnBlock == true) //플레이어가 블록 위에 있는 경우에만 동작
        {
            if (isInitDir) //블록이 초기 방향으로 이동해야 하는 경우
            {
                MoveInitialDirection();
            }
            else //블록이 방향을 전환하여 이동해야 하는 경우
            {
                MoveOppositeDirection();
            }
        }
    }

    void MoveInitialDirection()
    {
        /* 블록이 초기 방향으로 움직이도록 하는 함수 */
        if (Mathf.Abs(transform.position.x - EndPosition.transform.position.x) > 0.1f //x축을 따라 움직이는 블록이, 초기 방향 이동 중인 경우
            || Mathf.Abs(transform.position.z - EndPosition.transform.position.z) > 0.1f) //z축을 따라 움직이는 블록이, 초기 방향 이동 중인 경우
        {
            //EndPosition까지 블록 이동
            transform.position = Vector3.MoveTowards(transform.position, EndPosition.transform.position, Time.deltaTime * speed);
        }
        else
        {
            isInitDir = false; //EndPosition에 도달하면 방향 전환
        }
    }
    void MoveOppositeDirection()
    {
        /* 블록이 전환된 방향으로 움직이도록 하는 함수 */
        if (Mathf.Abs(transform.position.x - startPosition.x) > 0.1f //x축을 따라 움직이는 블록이, 전환된 방향으로 이동 중
            || Mathf.Abs(transform.position.z - startPosition.z) > 0.1f) //z축을 따라 움직이는 블록이, 전환된 방향으로 이동 중
        {
            //startPosition까지 블록 이동
            transform.position = Vector3.MoveTowards(transform.position, startPosition, Time.deltaTime * speed);
        }
        else
        {
            isInitDir = true; //startPosition에 도달하면 방향 전환
        }
    }
}
