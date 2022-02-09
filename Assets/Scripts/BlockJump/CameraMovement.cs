using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 offset; // 변위차
    public float FollowSpeed; // 따라가는 속력
    public float turnSpeed = 4.0f; // 마우스 회전 속도    
    private float xRotate = 0.0f; // 내부 사용할 X축 회전량은 별도 정의 ( 카메라 위 아래 방향 )
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CharacterFollow();
        
    }

    private void CharacterFollow()
    {
        Vector3 CameraPosition = player.transform.position + offset;
        Vector3 Pos = Vector3.Lerp(transform.position, CameraPosition, FollowSpeed);
        transform.position = Pos;
        transform.LookAt(player.transform);
    }
}
