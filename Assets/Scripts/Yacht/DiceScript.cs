using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XReal.XTown.Yacht
{
    public class DiceInfo
    {
        public int diceIndex;
        public int diceNumber;
        public int sortedIndex;
        public bool rolling;
        public bool keeping;
    }

    public class DiceScript : MonoBehaviour
    {

        public bool showTrigger = false;
        public bool pickTrigger = false;
        public bool takeOutTrigger = false;
        protected Vector3 prevPosition;
        protected Vector3 targetPosition;
        protected Quaternion prevRotation;
        protected Quaternion targetRotation;

        protected float timeStartedLerping;
        protected float lerpTime = 0.35f;

        public Rigidbody rb;
        public Vector3 diceVelocity;
        public int diceIndex;
        public DiceInfo diceInfo;

        public static List<DiceInfo> diceInfoList = new List<DiceInfo>();


        // Start is called before the first frame update
        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody>();
            /* NOT HERE!!
            DiceInfo info = new DiceInfo();
            // are we just setting dice Index to 0?
            info.diceIndex = diceIndex;
            info.diceNumber = 0;
            info.sortedIndex = 0;
            info.rolling = false;
            info.keeping = false;
            diceInfoList.Add(info);
            diceInfo = info;
            */
        }

        public void InitDice()
        {
            DiceInfo info = new DiceInfo();
            // are we just setting dice Index to 0?
            info.diceIndex = diceIndex;
            info.diceNumber = 0;
            info.sortedIndex = 0;
            info.rolling = false;
            info.keeping = false;
            diceInfoList.Add(info);
            diceInfo = info;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            diceVelocity = rb.velocity;

            if (Input.GetMouseButtonDown(0) && GameManager.turnCount <= 3 && GameManager.currentGameState == GameState.selecting)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == gameObject)
                {
                    if (diceInfo.keeping != true)
                    {
                        PickedSlotController.instance.PutIntoEmptySlot(diceIndex);
                    }
                    else
                    {
                        PickedSlotController.instance.GetOutOccupiedSlot(diceIndex);
                    }
                }
            }

            if (!diceInfo.rolling && showTrigger == true)
            {
                float currentLerpTime = Time.time - timeStartedLerping;
                float t = currentLerpTime / lerpTime;
                if (t >= 1.0f)
                {
                    showTrigger = false;
                    return;
                }

                t = Mathf.Clamp(Mathf.Sin(t * Mathf.PI * 0.5f), 0f, 1f);
                transform.rotation = Quaternion.Lerp(prevRotation, targetRotation, t);
                transform.position = Vector3.Lerp(prevPosition, targetPosition, t);
            }

            if (!diceInfo.rolling && pickTrigger == true)
            {
                float currentLerpTime = Time.time - timeStartedLerping;
                float t = currentLerpTime / lerpTime;
                if (t >= 1.0f)
                {
                    pickTrigger = false;
                    return;
                }

                t = Mathf.Clamp(Mathf.Sin(t * Mathf.PI * 0.5f), 0f, 1f);
                transform.position = Vector3.Lerp(prevPosition, targetPosition, t);
            }

            if (!diceInfo.rolling && takeOutTrigger == true)
            {
                float currentLerpTime = Time.time - timeStartedLerping;
                float t = currentLerpTime / lerpTime;
                if (t >= 1.0f)
                {
                    takeOutTrigger = false;
                    return;
                }

                t = Mathf.Clamp(Mathf.Sin(t * Mathf.PI * 0.5f), 0f, 1f);
                transform.position = Vector3.Lerp(prevPosition, targetPosition, t);
            }
        }

        public void Ready()
        {
            rb.isKinematic = false;
            Debug.LogWarning("CheckError:" + CupManager.instance.inCupSpawnTransforms[diceIndex].position);
            transform.position = CupManager.instance.inCupSpawnTransforms[diceIndex].position;
            transform.rotation = Random.rotation;
        }

        public void Roll()
        {
            if (!diceInfo.rolling)
            {
                rb.isKinematic = false;
                diceInfo.rolling = true;
                Vector3 dir = new Vector3(-1.732f, -1f, 0).normalized;
                rb.AddForce(dir * 900);
            }
        }

        public void OnRollingFinish()
        {
            rb.isKinematic = true;
            prevPosition = transform.position;
            prevRotation = transform.rotation;
            targetPosition = ShownSlotController.shownSlots[diceInfo.sortedIndex].transform.position;
            targetRotation = GameManager.rotArray[diceInfo.diceNumber - 1];
            showTrigger = true;
            timeStartedLerping = Time.time;
        }

        public void OnPicked(Transform slotTransform)
        {
            diceInfo.keeping = true;
            prevPosition = transform.position;
            targetPosition = slotTransform.position;
            pickTrigger = true;
            timeStartedLerping = Time.time;
        }

        public void OnTakeOut()
        {
            diceInfo.keeping = false;
            prevPosition = transform.position;
            targetPosition = ShownSlotController.shownSlots[diceInfo.sortedIndex].transform.position;
            takeOutTrigger = true;
            timeStartedLerping = Time.time;
        }

        public Vector3 GenerateRandomPos()
        {
            return new Vector3(Random.Range(-3, 3), 2, Random.Range(-3, 3));
        }
    }
}

