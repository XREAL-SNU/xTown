using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DragAndShoot : MonoBehaviour
{
    [SerializeField]
    private AudioClip _bounceSound;
    [SerializeField]
    private AudioClip _rimSound;
    [SerializeField]
    private float _forceMultiplier = 2f;

    private float _minForceMultiplier = 1f;
    private float _maxForceMultiplier = 3f;
    private float _forceMultiplierDelta = 0.1f;

    private Vector3 _mousePressDownPos;
    private Vector3 _mouseReleasePos;
    
    private Rigidbody _rb;

    private bool _isShoot;

    private bool _triggeredFirst;
    private bool _triggeredSecond;

    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta == 0)
        {
            return;
        }
        else
        {
            float newForceMultiplier = _forceMultiplier + _forceMultiplierDelta * scrollDelta;
            _forceMultiplier = Mathf.Clamp(newForceMultiplier, _minForceMultiplier, _maxForceMultiplier);
        }
        
    }

    private void OnMouseDown()
    {
        _mousePressDownPos = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        Vector3 forceInit = (Input.mousePosition - _mousePressDownPos);
        Vector3 forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.y)) * _forceMultiplier;

        if (!_isShoot)
        {
            DrawTrajectory.Instance.UpdateTrajectory(forceV, _rb, transform.position);
        }
    }

    private void OnMouseUp()
    {
        DrawTrajectory.Instance.HideLine();
        _mouseReleasePos = Input.mousePosition;
        Shoot(_mouseReleasePos - _mousePressDownPos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(DestroyAfterSeconds(5f));

        if (collision.gameObject.CompareTag("Rim"))
        {
            float soundVolume = Mathf.Clamp(collision.relativeVelocity.magnitude / 4, 0, 1);
            SoundManager.PlaySound(SoundManager.Sound.RimHit, transform.position, soundVolume);
        }
        else
        {
            float soundVolume = Mathf.Clamp(collision.relativeVelocity.y / 6, 0, 1);
            SoundManager.PlaySound(SoundManager.Sound.BallBounce, transform.position, soundVolume);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "First")
        {
            _triggeredFirst = true;
            return;
        }
        if (other.gameObject.name == "Second" && _triggeredFirst)
        {
            GameManager.Instance.OnGoal();
        }
    }

    private void Shoot(Vector3 force)
    {
        if (_isShoot)
        {
            return;
        }

        Vector3 forceV = (new Vector3(force.x, force.y, force.y)) * _forceMultiplier;

        _rb.isKinematic = false;
        _rb.AddForce(forceV);
        GameManager.ballEquipped = false;
        StartCoroutine(DestroyAfterSeconds(5));
    }

    IEnumerator DestroyAfterSeconds(float i)
    {
        yield return new WaitForSeconds(i);

        Destroy(gameObject);
    }

}
