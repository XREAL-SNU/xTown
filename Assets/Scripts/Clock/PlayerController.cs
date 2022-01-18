using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private StatusPanel _statusPanel;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            OnStatusTable();
        }
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            OffStatusTable();
        }
    }

    void OnStatusTable()
    {
        _statusPanel.gameObject.SetActive(true);
        _statusPanel.Show();
    }

    void OffStatusTable()
    {
        _statusPanel.gameObject.SetActive(false);
    }
}
