using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatButtonCanvas : MonoBehaviour
{
    [SerializeField]
    private Button _disembarkButton;

    private void Start()
    {
        _disembarkButton.onClick.AddListener(OnClick_Disembark);
        Hide();
    }

    public void OnClick_Disembark()
    {
        BoatSeat boatSeat = PlayerManager.Players.LocalPlayerGo.transform.parent.GetComponent<BoatSeat>();
        boatSeat.Disembark();
    }

    public void Show()
    {
        _disembarkButton.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _disembarkButton.gameObject.SetActive(false);
    }
}
