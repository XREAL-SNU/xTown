using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SY_BlockMoveVertical : MonoBehaviour   
{
    public float speed;
    public GameObject TargetPosition;

    private Vector3 startPosition;
    private Vector3 blockOffset; //정지위치 조정
    private bool isGoFront = true; //이동방향 전환

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        blockOffset = new Vector3(3, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!(transform.position == TargetPosition.transform.position))
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition.transform.position - blockOffset, Time.deltaTime * speed);
            //isGoFront = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition + blockOffset, Time.deltaTime * speed);
            //isGoFront = true;
        }
    }
}
