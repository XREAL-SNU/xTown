using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!Spawner.Instance.ballExisting)
            {
                Spawner.Instance.SpawnBall();
            }
        }
    }

}
