using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSelectionCanvas : MonoBehaviour
{
    [SerializeField]
    private AvatarSelectionMenu _avatarSelectionMenu;

    private RoomsCanvases _roomCanvases;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
        _avatarSelectionMenu.FirstInitialize(canvases);
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
