using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _ballPrefab;
    [SerializeField]
    private Vector3 _spawnPosition = new Vector3(0, 1f, 0.5f);

    private bool _canSpawnBall;
    private bool _spawnTimer;

    public static Spawner Instance;

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
    }

    private void Start()
    {
        GameManager.OnGameStateChanged += Initialize;
    }

    private void Update()
    {
        
    }

    private void Initialize()
    {
        if (GameManager.CurrentGameState == GameManager.GameState.GameReady)
        {
            _canSpawnBall = true;
        }
    }

    public void SpawnBall()
    {
        if (!_canSpawnBall)
        {
            return;
        }
        if (GameManager.ballEquipped)
        {
            return;
        }

        GameObject obj = Instantiate(_ballPrefab as GameObject, _spawnPosition, Random.rotation);
        obj.SetActive(true);
        _canSpawnBall = false;
        GameManager.ballEquipped = true;
        StartCoroutine(WaitBallSpawnTimer());
    }

    IEnumerator WaitBallSpawnTimer()
    {
        yield return new WaitForSeconds(1);
        _canSpawnBall = true;
    }
}
