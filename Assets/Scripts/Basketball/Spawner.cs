using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _ballPrefab;
    [SerializeField]
    private Vector3 _spawnPosition = new Vector3(0, 1f, 0.5f);

    private bool _ballExisting;
    public bool ballExisting { get { return _ballExisting; } }


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

        _ballExisting = false;
    }

    public void SpawnBall()
    {
        if (!_ballExisting)
        {
            GameObject obj = Instantiate(_ballPrefab as GameObject, _spawnPosition, Quaternion.identity);
            obj.SetActive(true);
            _ballExisting = true;
        }
    }

    public void OnShootFinished()
    {
        _ballExisting = false;
    }
}
