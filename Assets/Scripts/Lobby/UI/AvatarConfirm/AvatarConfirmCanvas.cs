using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarConfirmCanvas : MonoBehaviour
{
    [SerializeField]
    private NickNameEditMenu _nickNameEditMenu;

    [SerializeField]
    private AvatarSetMenu _avatarSetMenu;
    [SerializeField]
    private AvatarViewMenu _avatarViewMenu;
    private RoomsCanvases _roomCanvases;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
        _nickNameEditMenu.FirstInitialize(canvases);
        _avatarViewMenu.FirstInitialize(canvases);
        _avatarSetMenu.FirstInitialize(canvases);
    }
    public void Show()
    {
        gameObject.SetActive(true);
        _nickNameEditMenu.SetNickName();
        _avatarViewMenu.ViewAvatar();

        PlayerManager.Players.LocalPlayerGo = GameObject.FindGameObjectWithTag("Player");
    }

    public void Hide()
    {
        _avatarViewMenu.DestroyAvatar();
        gameObject.SetActive(false);
    }
}
