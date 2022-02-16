using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatButtonCanvas : MonoBehaviour
{
    public void OnClick_Disembark()
    {
        BoatSeat boatSeat = PlayerManager.Players.LocalPlayerGo.transform.parent.GetComponent<BoatSeat>();
        boatSeat.Disembark();
    }
}
