using System.Collections;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Photon.Pun;

namespace StarterAssets
{

    public class StarLauncher : MonoBehaviour
    {
        Animator animator;
        StarterAssetsInputs movement;
        StarAnimation starAnimation;
        TrailRenderer trail;

        ThirdPersonControllerMulti TPC;
        public AnimationCurve pathCurve;

        [Range(0,50)]
        public float speed = 10f;
        float speedModifier = 1;

        [Space]
        [Header("Booleans")]
        public bool insideLaunchStar;
        public bool flying;
        public bool almostFinished;

        private Transform launchObject;

        [Space]
        [Header("Public References")]
        public CinemachineFreeLook thirdPersonCamera;
        public CinemachineDollyCart dollyCart;
        float cameraRotation;
        public Transform playerParent;

        [Space]
        [Header("Launch Preparation Sequence")]
        public float prepMoveDuration = .15f;
        public float launchInterval = .5f;

        [Space]
        [Header("Particles")]
        public ParticleSystem followParticles;
        public ParticleSystem smokeParticle;

        // networking
        PhotonView _view;

        void Start()
        {
            animator = GetComponent<Animator>();
            movement = GetComponent<StarterAssetsInputs>();
            TPC = GetComponent<ThirdPersonControllerMulti>();

            // NETWORKING
            _view = GetComponent<PhotonView>();
            //
        }

        void Update()
        {
            // NETWORKING
            if (!_view.IsMine) return;
            //

            if (insideLaunchStar)
            {
                if (Input.GetKeyDown(KeyCode.L))
                {
                    int dollyCartViewId = -1;
                    int playerParentViewId = -1;
                    int launchObjectViewId = launchObject.GetComponentInChildren<PhotonView>().ViewID;

                    if (dollyCart == null)
                    {
                        GameObject dollyCartGo = Instantiate(Resources.Load<GameObject>("Monorail/Dolly_Character"), Vector3.zero, Quaternion.identity);
                        /* Network instantiation
                        GameObject dollyCartGo = PhotonNetwork.Instantiate("Monorail/Dolly_Character", Vector3.zero, Quaternion.identity);
                        */
                        dollyCart = dollyCartGo.GetComponent<CinemachineDollyCart>();
                        trail = dollyCart.GetComponentInChildren<TrailRenderer>();
                    }
                
                    if (playerParent == null)
                    {
                        //GameObject playerParentGo = Instantiate(Resources.Load<GameObject>("Monorail/Player_Parent"), Vector3.zero, Quaternion.identity);
                        // NETWORKING
                        GameObject playerParentGo = PhotonNetwork.Instantiate("Monorail/Player_Parent", Vector3.zero, Quaternion.identity);
                        playerParentViewId = playerParentGo.GetComponent<PhotonView>().ViewID;
                        //
                        playerParent = playerParentGo.transform;
                    }
                
                    StartCoroutine(CenterLaunch());
                    // NETWORKING
                    _view.RPC("GetOnCartRPC", RpcTarget.Others, playerParentViewId, launchObjectViewId);
                    //
                }
            }

            if (dollyCart is null) return;
            if (playerParent is null) return;

            if (flying)
            {
                animator.SetFloat("Path", dollyCart.m_Position);
                // playerParent transform setters -> automatically synced
                playerParent.transform.position = dollyCart.transform.position;
                if (!almostFinished)
                {
                    playerParent.transform.rotation = dollyCart.transform.rotation;
                }

                if (Input.GetKeyDown(KeyCode.L))
                {
                    playerParent.DOComplete();
                    dollyCart.enabled = false;
                    dollyCart.m_Position = 0;
                    animator.applyRootMotion = true;
                    movement.enabled = true;
                    TPC.enabled = true;
                    transform.parent = null;

                    flying = false;
                    almostFinished = false;
                    animator.SetBool("flying", false);
                    followParticles.Stop();
                    trail.emitting = false;
                }
            }

            if(dollyCart.m_Position > .7f && !almostFinished && flying)
            {
                almostFinished = true;
                //thirdPersonCamera.m_XAxis.Value = cameraRotation;

                playerParent.DORotate(new Vector3(360 + 180, 0, 0), .5f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear)
                    .OnComplete(() => playerParent.DORotate(new Vector3(-90, playerParent.eulerAngles.y, playerParent.eulerAngles.z), .2f));
            }
        }



        IEnumerator CenterLaunch()
        {
            animator.applyRootMotion = false;
            movement.enabled = false;
            transform.parent = null;
            TPC.enabled = false;
            DOTween.KillAll();


            //Checks to see if there is a Camera Trigger at the DollyTrack object - if there is activate its camera
            if (launchObject.GetComponent<SpeedModifier>() != null)
                speedModifier = launchObject.GetComponent<SpeedModifier>().modifier;

            //Checks to see if there is a Star Animation at the DollyTrack object
            if (launchObject.GetComponentInChildren<StarAnimation>() != null)
                starAnimation = launchObject.GetComponentInChildren<StarAnimation>();

    
            dollyCart.m_Position = 0;
            dollyCart.m_Path = null;
            dollyCart.m_Path = launchObject.GetComponent<CinemachineSmoothPath>();
            dollyCart.enabled = true;

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            // all sequence elements are synced through animator and transform view.
            Debug.LogWarning("StarLauncher/ CenterLaunch");
            Sequence CenterLaunch = DOTween.Sequence();
            CenterLaunch.Append(transform.DOMove(dollyCart.transform.position, .2f));
            CenterLaunch.Join(transform.DORotate(dollyCart.transform.eulerAngles + new Vector3(90, 0, 0), .2f));
            CenterLaunch.Join(starAnimation.Reset(.2f));
            CenterLaunch.OnComplete(() => LaunchSequence());
        }

        Sequence LaunchSequence()
        {
            Debug.LogWarning("StarLauncher/ launch seq #0");

            float distance;
            CinemachineSmoothPath path = launchObject.GetComponent<CinemachineSmoothPath>();
            Debug.LogWarning("StarLauncher/ launch seq #1");
            float finalSpeed = path.PathLength / (speed * speedModifier);

            cameraRotation = transform.eulerAngles.y;
            Debug.LogWarning("StarLauncher/ launch seq #2");

            playerParent.transform.position = launchObject.position;
            playerParent.transform.rotation = transform.rotation;
            Debug.LogWarning("StarLauncher/ launch seq #3");

            flying = true;
            Debug.LogWarning($"StarLauncher/ animator ? {animator != null}. {animator.enabled}");

            animator.SetBool("flying", true);

            _view.RPC("LaunchSequenceRPC", RpcTarget.Others);

            Sequence s = DOTween.Sequence();

            // SET prarent transform
            s.AppendCallback(() => transform.parent = playerParent.transform);                                           // Attatch the player to the empty gameObject
            s.Append(transform.DOMove(transform.localPosition - transform.up, prepMoveDuration));                        // Pull player a little bit back
            s.Join(transform.DOLocalRotate(new Vector3(0, 360 * 2, 0), prepMoveDuration, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart));
            s.Join(starAnimation.PullStar(prepMoveDuration));
            s.AppendInterval(launchInterval);                                                                            // Wait for a while before the launch
            s.AppendCallback(() => trail.emitting = true);
            s.AppendCallback(() => followParticles.Play());
            // set dolly track
            s.Append(DOVirtual.Float(dollyCart.m_Position, 1, finalSpeed, PathSpeed).SetEase(pathCurve));                // Lerp the value of the Dolly Cart position from 0 to 1
            s.Join(starAnimation.PunchStar(.5f));
            s.Join(transform.DOLocalMove(new Vector3(0,0,-.5f), .5f));                                                   // Return player's Y position
            s.Join(transform.DOLocalRotate(new Vector3(0, 360, 0),                                                       // Slow rotation for when player is flying
                (finalSpeed/1.3f), RotateMode.LocalAxisAdd)).SetEase(Ease.InOutSine); 
            s.AppendCallback(() => Land());                                                                              // Call Land Function

            return s;
        }

        void Land()
        {
            PlaySmoke();
            playerParent.DOComplete();
            dollyCart.enabled = false;
            dollyCart.m_Position = 0;
            animator.applyRootMotion = true;
            movement.enabled = true;
            TPC.enabled = true;
            transform.parent = null;

            flying = false;
            almostFinished = false;
            animator.SetBool("flying", false);

            followParticles.Stop();
            trail.emitting = false;

            _view.RPC("LandRPC", RpcTarget.Others);
        }

        // to be networked
        public void PathSpeed(float x)
        {
            dollyCart.m_Position = x;
        }

        public void PlaySmoke()
        {
            CinemachineImpulseSource[] impulses = FindObjectsOfType<CinemachineImpulseSource>();
            for (int i = 0; i < impulses.Length; i++)
                impulses[i].GenerateImpulse();
            smokeParticle.Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Launch"))
            {
                insideLaunchStar = true;
                launchObject = other.transform;
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Launch"))
            {
                insideLaunchStar = false;
            }
        }


        #region RPC

        // called just after invoking Start Coroutine CenterLaunch
        [PunRPC]
        public void GetOnCartRPC(int playerParentViewId, int launchObjectViewId)
        {
            Debug.Log("GetOnCartRPC: looking for playerParent and launchObject");
            // FREEZE movement
            movement.enabled = false;
            transform.parent = null;
            TPC.enabled = false;

            // FIND playerParent, and launch object
            playerParent = PhotonView.Find(playerParentViewId).transform;
            if (playerParent is null) Debug.LogError("StarLauncher/ cannot find parent");
            launchObject = PhotonView.Find(launchObjectViewId).transform;
            if (launchObject is null) Debug.LogError("StarLauncher/ cannot find launcher");

        }

        // called inside LaunchSequence
        [PunRPC]
        public void LaunchSequenceRPC()
        {
            Debug.Log("LaunchSequencePRC/ LAUNCH!!! setting parent");
            //cameraRotation = transform.eulerAngles.y;

            // SET parent to playerParent.
            transform.SetParent(playerParent.transform);

            // PLAY particles
            followParticles.Play();
        }

        // called by right after Land
        [PunRPC]
        public void LandRPC()
        {
            Debug.Log("LandRPC/ LAND!! detaching parent");
            PlaySmoke();
            // ENABLE controls
            movement.enabled = true;
            TPC.enabled = true;

            // SET parent to null
            transform.parent = null;

            // STOP particles
            followParticles.Stop();
        }

        /* 
        // IPunObservables impl
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(dollyCart.m_Position);
            }
            else
            {
                if (dollyCart != null) this.dollyCart.m_Position = (float)stream.ReceiveNext();
            }
        }
        */
        #endregion
    }
}
