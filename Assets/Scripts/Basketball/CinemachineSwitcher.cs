using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XReal.XTown.Basketball
{
    public class CinemachineSwitcher : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            GameManager.OnGameStateChanged += SwitchCameraState;
        }

        private void SwitchCameraState()
        {
            if (GameManager.CurrentGameState != GameManager.GameState.RoundWaiting)
            {
                return;
            }
            if (GameManager.round == 1)
            {
                _animator.Play("vcam1");
            }
            else if (GameManager.round == 2)
            {
                _animator.Play("vcam2");
            }
            else if (GameManager.round == 3)
            {
                _animator.Play("vcam3");
            }
            else if (GameManager.round == 4)
            {
                _animator.Play("vcam1");
            }
        }
    }
}
