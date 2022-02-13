using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XReal.XTown.Yacht
{
    public class CupManager : MonoBehaviour
    {
        public static CupManager instance;
        public static bool playingAnim = false;

        public Transform[] inCupSpawnTransforms = new Transform[5];

        private Animator anim;
        private BoxCollider ceiling;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (instance != this)
                {
                    Destroy(this.gameObject);
                }
            }

            int i = 0;

            Transform spawnPositions = transform.Find("DiceInCupPositions");
            foreach (Transform child in spawnPositions)
            {
                inCupSpawnTransforms[i] = child;
                i += 1;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            ceiling = transform.Find("Ceiling").GetComponent<BoxCollider>();
        }


        public void OnReadyStart()
        {
            playingAnim = true;
            anim.SetTrigger("Ready");
        }

        public void OnReadyToSelect()
        {
            anim.SetTrigger("Rest");
        }

        public void OnShakingStart()
        {
            anim.SetTrigger("Shake");
        }

        public void OnPouringStart()
        {
            anim.SetTrigger("Pour");
        }

        public void OnRollingStart()
        {
            GameManager.SetGameState(GameState.rolling);
            GameManager.rollTrigger = true;
            ceiling.enabled = false;
        }

        public void OnRollingFinish()
        {
            ceiling.enabled = true;
        }

        public void OnAnimationStart()
        {
            playingAnim = true;
        }

        public void OnAnimationFinish()
        {
            playingAnim = false;
        }
    }
}

