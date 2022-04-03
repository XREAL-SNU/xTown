using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerNameTag : MonoBehaviourPun
{
    [SerializeField] private TextMeshProUGUI nameText;
    void Start()
    {
        if (photonView.IsMine)
        {
            nameText.gameObject.SetActive(false);
            return;
        }

        SetName();
    }

    private void SetName() => nameText.text = photonView.Owner.NickName;
}
