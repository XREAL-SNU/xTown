 using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XReal.XTown.Yacht
{

    public class DiceScriptMulti : DiceScript
    {
        PhotonTransformView view;
        protected void Awake()
        {
            view = GetComponent<PhotonTransformView>();
        }
        protected override void Start()
        {
            if (!NetworkManager.Instance.networked)
            {
                view.enabled = false;
                base.Start();
                return;
            }

            base.Start();
        }

        public void BeginSyncDice()
        {
            if (!view.enabled) view.enabled = true;
        }
        // Update is called once per frame
        protected override void Update()
        {
            if (!NetworkManager.Instance.networked)
            {
                base.Update();
                return;
            }
            if (NetworkManager.Instance.MeDone) return;

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


    }
}

