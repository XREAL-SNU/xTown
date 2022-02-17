using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Photon.Pun;

public class StarAnimation : MonoBehaviour
{
    Animator animator;
    Transform big;
    Transform small;

    public AnimationCurve punch;
    [Space]
    [Header("Particles")]
    public ParticleSystem glow;
    public ParticleSystem charge;
    public ParticleSystem explode;
    public ParticleSystem smoke;

    // notes on networking.
    PhotonView _view;

    private void Start()
    {
        animator = GetComponent<Animator>();
        big = transform.GetChild(0);
        small = transform.GetChild(1);

        // netcode
        _view = GetComponent<PhotonView>();
    }


    public Sequence Reset(float time)
    {
        animator.enabled = false;
        Sequence s = DOTween.Sequence();
        s.Append(big.DOLocalRotate(Vector3.zero, time).SetEase(Ease.InOutSine));
        s.Join(small.DOLocalRotate(Vector3.zero, time).SetEase(Ease.InOutSine));

        _view.RPC("ResetRPC", RpcTarget.Others, time);
        return s;
    }

    public Sequence PullStar(float pullTime)
    {
        glow.Play();
        charge.Play();

        Sequence s = DOTween.Sequence();

        s.Append(big.DOLocalRotate(new Vector3(0, 0, 360 * 2), pullTime, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart));
        s.Join(small.DOLocalRotate(new Vector3(0, 0, 360 * 2), pullTime, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart));
        s.Join(small.DOLocalMoveZ(-4.2f, pullTime));

        _view.RPC("PullStarRPC", RpcTarget.Others, pullTime);

        return s;
    }

    public Sequence PunchStar(float punchTime)
    {
        CinemachineImpulseSource[] impulses = FindObjectsOfType<CinemachineImpulseSource>();

        animator.enabled = false;

        Sequence s = DOTween.Sequence();

        s.AppendCallback(() => explode.Play());
        s.AppendCallback(() => smoke.Play());
        s.AppendCallback(() => impulses[0].GenerateImpulse());
        s.Append(small.DOLocalMove(Vector3.zero, .8f).SetEase(punch));
        s.Join(small.DOLocalRotate(new Vector3(0, 0, 360 * 2), .8f).SetEase(Ease.OutBack));
        s.AppendInterval(.8f);
        s.AppendCallback(() => animator.enabled = true);

        _view.RPC("PunchStarRPC", RpcTarget.Others);
        return s;
    }


    // networking
    [PunRPC]
    public void ResetRPC(float time)
    {
        // just animate, do NOT touch animator because that is controlled by Photon Animator view.
        DOTween.Sequence()
            .Append(big.DOLocalRotate(Vector3.zero, time).SetEase(Ease.InOutSine))
            .Join(small.DOLocalRotate(Vector3.zero, time).SetEase(Ease.InOutSine));
    }

    [PunRPC]
    public void PullStarRPC(float pullTime)
    {
        glow.Play();
        charge.Play();

        DOTween.Sequence()
            .Append(big.DOLocalRotate(new Vector3(0, 0, 360 * 2), pullTime, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart))
            .Join(small.DOLocalRotate(new Vector3(0, 0, 360 * 2), pullTime, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart))
            .Join(small.DOLocalMoveZ(-4.2f, pullTime));
    }

    [PunRPC]
    public void PunchStarRPC()
    {
        CinemachineImpulseSource[] impulses = FindObjectsOfType<CinemachineImpulseSource>();
        DOTween.Sequence()
            .AppendCallback(() => explode.Play())
            .AppendCallback(() => smoke.Play())
            .AppendCallback(() => impulses[0].GenerateImpulse())
            .Append(small.DOLocalMove(Vector3.zero, .8f).SetEase(punch))
            .Join(small.DOLocalRotate(new Vector3(0, 0, 360 * 2), .8f).SetEase(Ease.OutBack));
    }
}


