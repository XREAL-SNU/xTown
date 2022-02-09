using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace XReal.XTown.Basketball
{
    public class RimMovement : MonoBehaviour
    {
        [SerializeField]
        private Transform _rimTransform;

        private Sequence _rimSequence;
        private bool _shaking;

        // Start is called before the first frame update
        void Start()
        {
            _shaking = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_shaking)
            {
                Shake();
            }
        }

        private void Shake()
        {
            _rimSequence = DOTween.Sequence()
                .OnStart(() =>
                {
                    _shaking = true;
                })
                .Append(_rimTransform.DOShakeRotation(0.7f, new Vector3(10, 0, 0), 10, 10).SetEase(Ease.OutElastic))
                .OnComplete(() =>
                {
                    _shaking = false;
                });
        }
    }
}
