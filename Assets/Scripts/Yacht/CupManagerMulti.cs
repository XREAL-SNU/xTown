using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XReal.XTown.Yacht
{
    public class CupManagerMulti : CupManager, IPunOwnershipCallbacks
    {


        private static PhotonTransformView transformView;
        private static PhotonAnimatorView animView;
        private static PhotonView view;


        /// <summary>
        /// Monobehaviour callbacks
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            Debug.Log("CupManagerMulti/Awake");
            transformView = GetComponent<PhotonTransformView>();
            animView = GetComponent<PhotonAnimatorView>();
            view = GetComponent<PhotonView>();
        }

        void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        protected override void Start()
        {
            base.Start();
            if (!PhotonNetwork.InRoom) return;
            Debug.Log("CupManagerMulti/Start, registering callbacks");
            // register event callbacks
            GameManagerMulti.instance.onReadyStart.AddListener(OnReadyStart);
            GameManagerMulti.instance.onShakingStart.AddListener(OnShakingStart);
            GameManagerMulti.instance.onPouringStart.AddListener(OnPouringStart);
            GameManagerMulti.instance.onReadyToSelect.AddListener(OnReadyToSelect);
            GameManagerMulti.instance.onRollingFinish.AddListener(OnRollingFinish);
        }


        public static void DisableCupView()
        {
            if (transformView.enabled) transformView.enabled = false;
            if (animView.enabled) animView.enabled = false;
            if (view.enabled) view.enabled = false;
        }
        public bool IsMine
        {
            get => view.IsMine;
        }

        // ownership methods
        public static void RequestCupOwnership()
        {
            if (view.OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber) return;
            Debug.Log($"CupManager/ Player{PhotonNetwork.LocalPlayer.ActorNumber} requesting ownership to player{view.OwnerActorNr}");
            view.RequestOwnership();
        }

        /// photon callbacks
        public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
        {
            if (targetView != view) return;
            if (view.OwnerActorNr != PhotonNetwork.LocalPlayer.ActorNumber) return;
            Debug.Log("handing over cup control to: player#" + requestingPlayer.ActorNumber);
            view.TransferOwnership(requestingPlayer);
        }

        public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
        {
            if (targetView != view) return;
            GameManagerMulti.CheckAllMine();
        }

        public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
        {
            if (targetView != view) return;
        }
        public void OnOwnershipRequest(object[] viewAndPlayer)
        {
            PhotonView view = viewAndPlayer[0] as PhotonView;
            Player requestingPlayer = viewAndPlayer[1] as Player;

            if (view.OwnerActorNr != PhotonNetwork.LocalPlayer.ActorNumber) return;

            try
            {
                Debug.Log("handing over cup control to: player#" + requestingPlayer.ActorNumber);
                view.TransferOwnership(requestingPlayer);
            }
            catch
            {
                Debug.LogError("Couldn't transfer control of cup.");
            }
        }

    }
}

