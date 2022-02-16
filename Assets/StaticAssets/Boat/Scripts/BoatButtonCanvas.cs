using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BoatButtonCanvas : MonoBehaviour
{
    [SerializeField]
    private Button _disembarkButton;

    private float _maxDistanceFromPlayer = 10f;
    private Vector3 _disembarkLocation;
    private int _layerMask;

    private void Start()
    {
        _disembarkButton.onClick.AddListener(OnClick_Disembark);
        _layerMask = 1 << LayerMask.NameToLayer("Ground");
        Hide();
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
                {
                    return;
                }
                if ((PlayerManager.Players.LocalPlayerGo.transform.position - hit.point).magnitude < _maxDistanceFromPlayer)
                {
                    _disembarkLocation = hit.point;
                    Show();
                }
            }
        }
    }

    public void OnClick_Disembark()
    {
        BoatSeat boatSeat = PlayerManager.Players.LocalPlayerGo.transform.parent.GetComponent<BoatSeat>();
        boatSeat.Disembark(_disembarkLocation);
    }

    public void Show()
    {
        Sequence showSequence = DOTween.Sequence()
            .OnStart(() =>
            {
                _disembarkButton.gameObject.SetActive(true);
            })
            .AppendInterval(3)
            .OnComplete(() =>
            {
                Hide();
            });
        // StartCoroutine(WaitAndHide());
    }

    public void Hide()
    {
        _disembarkButton.gameObject.SetActive(false);
    }

    IEnumerator WaitAndHide()
    {
        yield return new WaitForSeconds(3);
        Hide();
    }
}
