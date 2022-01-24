 using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XReal.XTown.Yacht
{

    public class DiceScriptMulti : DiceScript, IPunOwnershipCallbacks
    {
        PhotonTransformView _transformView;
        PhotonView _view;

        
        protected override void Start()
        {
            _transformView = GetComponent<PhotonTransformView>();
            _view = GetComponent<PhotonView>();

            if (DiceManager.dices.Count != 5) // list not full: because local copies destroyed
            {
                diceIndex = DiceManager.dices.Count;
                DiceManager.dices.Add(this);
                Debug.Log("DiceScript/Start: " + diceIndex);
            }
            base.Start();

            if (!NetworkManager.Instance.networked)
            {
                _transformView.enabled = false;
                _view.enabled = false;
                return;
            }


        }

        void OnEnable()
        {
            if (NetworkManager.Instance is null) return;
            if (!NetworkManager.Instance.networked) return;
            // this is needed to receive ownership transfer messages.
            // maybe we should collect all ownership related functions to a single interface(ITransferable)
            PhotonNetwork.AddCallbackTarget(this);
        }

        void OnDisable()
        {
            if(NetworkManager.Instance is null)
            return;
            if (!NetworkManager.Instance.networked) return;
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        // Update is called once per frame
        protected override void Update()
        {
            if(NetworkManager.Instance is null)
            return;
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


        /// Ownership
        public void RequestOwnership()
        {
            if (_view.OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber) return;
            Debug.Log("DiceScript/ Request ownership " + diceIndex);
            _view.RequestOwnership();
        }

        public bool IsMine
        {
            get => _view.IsMine;
        }

        /// photon callbacks
        public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
        {
            if (targetView != _view) return;
            if (_view.OwnerActorNr != PhotonNetwork.LocalPlayer.ActorNumber) return;
            Debug.Log("handing over dice" + diceIndex + "control to: player#" + requestingPlayer.ActorNumber);
            _view.TransferOwnership(requestingPlayer);
        }

        public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
        {
            if (targetView != _view) return;
            GameManagerMulti.CheckAllMine();
        }

        public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
        {
            if (targetView != _view) return;
            Debug.Log("DiceScript/OnOwnershipTransferFailed" + diceIndex);
        }
    }
}

