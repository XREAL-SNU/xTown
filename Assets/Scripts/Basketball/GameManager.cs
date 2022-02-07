using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private NDigitNumber _scoreDisplay;
    [SerializeField]
    private NDigitNumber _leftTimeDisplay;


    private static int _scorePerGoal = 30;

    public static int _totalScore;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

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
        _totalScore = 0;
        _scoreDisplay.SetNumber(0);
        _leftTimeDisplay.SetNumber(0);
    }

    public void OnGoal()
    {
        _totalScore += _scorePerGoal;
        _scoreDisplay.SetNumber(_totalScore);
    }
}
