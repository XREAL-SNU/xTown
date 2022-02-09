using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameInputCanvas : MonoBehaviour
{
    [SerializeField]
    private PlayerNameInputMenu _playerNameInputMenu;

    private RoomsCanvases _roomCanvases;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
        _playerNameInputMenu.FirstInitialize(canvases);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
