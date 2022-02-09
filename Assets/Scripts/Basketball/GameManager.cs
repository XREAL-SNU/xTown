using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        GameReady,
        RoundWaiting,
        RoundOngoing,
        RoundFinished,
        GameFinished,
    }

    [SerializeField]
    private NDigitNumber _scoreDisplay;
    [SerializeField]
    private NDigitNumber _leftTimeDisplay;
    [SerializeField]
    private ParticleSystem _confettiParticle;

    private static int _scorePerGoal = 30;
    private bool _gameStarted;
    private static bool _ballEquipped;
    public static bool ballEquipped { get; set; }

    private static int _round;
    public static int round { get { return _round; } }

    private static int _totalScore;
    public static int totalScore { get { return _totalScore; } }
    private static int _roundTime = 30;
    public static int roundTime { get { return _roundTime; } }
    public static Action OnGameStateChanged;
    private static GameState _currentGameState;
    public static GameState CurrentGameState { get { return _currentGameState; } }

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

        Physics.bounceThreshold = 1;
    }

    private void Start()
    {
        OnGameStateChanged += GameStateChanged;
        Initialize();
        SetGameState(GameState.GameReady);
    }

    // Update is called once per frame
    void Update()
    {
        if (!TimerController.Instance.timerOn){
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Spawner.Instance.SpawnBall();
        }
    }

    private void Initialize()
    {
        _round = 1;
        _totalScore = 0;
        _scoreDisplay.SetNumber(0);
        _leftTimeDisplay.SetNumber(0);
    }

    public void OnGoal()
    {
        _totalScore += _scorePerGoal;
        _scoreDisplay.SetNumber(_totalScore);
        SoundManager.PlaySound(SoundManager.Sound.Goal, 0.5f);
        _confettiParticle.Play();
    }


    public void OnTimerFinished()
    {
        _gameStarted = false;
    }

    public static void SetGameState(GameState gameState)
    {
        if (Enum.IsDefined(typeof(GameState), gameState))
        {
            _currentGameState = gameState;
        }
        OnGameStateChanged();
    }

    private void GameStateChanged()
    {
        switch (CurrentGameState)
        {
            case (GameState.RoundWaiting):
                {
                    StartCoroutine(CountDownForStart());
                    // remove all balls;
                    _ballEquipped = false;

                    if (round == 1)
                    {
                        _totalScore = 0;
                        _scoreDisplay.SetNumber(0);
                    }
                    break;
                }
            case (GameState.RoundFinished):
                {
                    StartCoroutine(CountDownForNextRound());
                    break;
                }
            case (GameState.GameFinished):
                {
                    StartCoroutine(CountDownForGameEnd());

                    _round = 1;
                    break;
                }
        }
    }

    IEnumerator CountDownForStart()
    {
        yield return new WaitForSeconds(3);

        SetGameState(GameState.RoundOngoing);
    }

    IEnumerator CountDownForNextRound()
    {
        yield return new WaitForSeconds(3);

        if (round <= 2)
        {
            _round += 1;
            SetGameState(GameState.RoundWaiting);
        }
        else
        {
            SetGameState(GameState.GameFinished);
        }
    }

    IEnumerator CountDownForGameEnd()
    {
        yield return new WaitForSeconds(3);

        SetGameState(GameState.GameReady);
    }
}
