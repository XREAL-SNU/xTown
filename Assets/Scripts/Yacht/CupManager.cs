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

        protected Animator anim;
        protected BoxCollider ceiling;
        public GameObject Wall1;

        protected virtual void Awake()
        {
            // DELETED singleton.
            instance = this;
        
            int i = 0;

            Transform spawnPositions = transform.Find("DiceInCupPositions");
            foreach (Transform child in spawnPositions)
            {
                inCupSpawnTransforms[i] = child;
                i += 1;
            }
        }


        // Start is called before the first frame update
        protected virtual void Start()
        {
            anim = GetComponent<Animator>();
            ceiling = transform.Find("Ceiling").GetComponent<BoxCollider>();
        }

        /// <summary>
        /// some addition
        /// </summary>
        /// 

        public void EnableAnimator()
        {
            Debug.Log("enable animator!");
            if (!anim.enabled) anim.enabled = true;
        }

        public void DisableAnimator()
        {
            Debug.Log("disable animator!");
            if (anim.enabled) anim.enabled = false;
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
            Wall1.SetActive(true);
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
            Wall1.SetActive(false);
        }

        public void OnAnimationStart()
        {
            ceiling.enabled = true;
            playingAnim = true;
        }

        public void OnAnimationFinish()
        {
            playingAnim = false;
        }
    }
}

