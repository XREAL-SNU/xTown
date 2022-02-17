using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace XReal.XTown.Basketball
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _ballPrefab;
        [SerializeField]
        private Transform[] _vcamTransforms;
        private Vector3 _spawnPosition; // Vector3(-2.87f, 0f, -0.56f);
        private float _forwardOffset = 0.55f;
        private float _upwardOffset = 0.3f;

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
            //_spawnPosition = Camera.main.GetComponent<CinemachineBrain>().
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
            else if (GameManager.CurrentGameState == GameManager.GameState.RoundWaiting)
            {
                if (GameManager.round < 4)
                {
                    int index = GameManager.round - 1;
                    _spawnPosition = _vcamTransforms[index].position + _vcamTransforms[index].forward * _forwardOffset - _vcamTransforms[index].up * _upwardOffset;
                }
                else
                {
                    _spawnPosition = _vcamTransforms[0].position + _vcamTransforms[0].forward * _forwardOffset - _vcamTransforms[0].up * _upwardOffset;
                }

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
}
