using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour   
{
    private GameObject Player;
    public GameObject EndPosition;
    private Vector3 startPosition;

    public float speed;
    private bool isInitDir = true; //movement direction
    private bool IsPlayerOnBlock;

    // Start is called before the first frame update
    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        startPosition = transform.position;

    }

    private void OnTriggerEnter(Collider other)
    {
        IsPlayerOnBlock = true;

        //Player moves with the block
        Player.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        IsPlayerOnBlock = false;

        Player.transform.parent = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Block movement
        if(IsPlayerOnBlock == true)
        {
            if (isInitDir) //Block heading initial direction;
            {
                MoveInitialDirection();
            }
            else //Block heading opposite direction;
            {
                MoveOppositeDirection();
            }
        }
    }

    void MoveInitialDirection()
    {
        if (Mathf.Abs(transform.position.x - EndPosition.transform.position.x) > 0.1f
            || Mathf.Abs(transform.position.z - EndPosition.transform.position.z) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, EndPosition.transform.position, Time.deltaTime * speed);
        }
        else
        {
            isInitDir = false;
        }
    }
    void MoveOppositeDirection()
    {
        if (Mathf.Abs(transform.position.x - startPosition.x) > 0.1f
            || Mathf.Abs(transform.position.z - startPosition.z) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, Time.deltaTime * speed);
        }
        else
        {
            isInitDir = true;
        }
    }
}
