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
            OnOffStatusTable();
        }
    }

    void OnOffStatusTable()
    {
        if (_statusPanel != null)
        {
              bool isActive=_statusPanel.gameObject.activeSelf;
              _statusPanel.gameObject.SetActive(!isActive);
        }
        _statusPanel.UpdateTime();
    }
}
