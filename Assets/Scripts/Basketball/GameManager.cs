using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static int score;

    public static Action OnGoal;

    private void Awake()
    {
        Initialize();
    }

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

    private void Initialize()
    {
        score = 0;
    }

    public static void UpdateScore(int i)
    {
        score += i;
    }

}
