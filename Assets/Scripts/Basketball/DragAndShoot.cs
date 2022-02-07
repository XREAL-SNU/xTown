using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DragAndShoot : MonoBehaviour
{
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


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
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

    private void OnDestroy()
    {
        Spawner.Instance.OnShootFinished();
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
        if (collision.gameObject.name == "Floor")
        {
            StartCoroutine(DestroyAfterSeconds(1f));
            return;
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
        StartCoroutine(DestroyAfterSeconds(5));
    }

    IEnumerator DestroyAfterSeconds(float i)
    {
        yield return new WaitForSeconds(i);

        Destroy(gameObject);
    }

}
