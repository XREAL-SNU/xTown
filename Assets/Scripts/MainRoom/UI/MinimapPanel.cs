using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapPanel : MonoBehaviour
{
    private MainCanvases _mainCanvases;

    [SerializeField]
    private LocalMinimap _localMinimap;
    [SerializeField]
    private WorldMinimap _worldMinimap;

    public void FirstInitialize(MainCanvases canvases)
    {
        _mainCanvases = canvases;
    }

    public void OnClick_SizeToggle()
    {
        if (_localMinimap.gameObject.activeSelf == true)
        {
            _localMinimap.Hide();
            _worldMinimap.Show();
        }
        else
        {
            _localMinimap.Show();
            _worldMinimap.Hide();
        }
    }
}
