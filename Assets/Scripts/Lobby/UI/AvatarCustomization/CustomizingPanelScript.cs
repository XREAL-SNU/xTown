using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TextureArray
{
    public Texture[] Textures;
}

public class CustomizingPanelScript : MonoBehaviour
{
    [SerializeField]
    private string _customPartId;

    [SerializeField]
    private List<CustomizingButtonScript> _customizingColorButtons;
    [SerializeField]
    private List<CustomizingButtonScript> _customizingTextureButtons;

    [SerializeField]
    private TextureArray[] _textures;

    public FlexibleColorPicker Fcp;

    private Color _normal = new Color(255 / 255, 255 / 255, 255 / 255, 255 / 255);
    private int _selected = 0;

    // that static is needed! this script attached to many panels
    // but there's only one _avatarAppearance!
    private static AvatarAppearance _avatarAppearance;
    static GameObject _previewAvatarObject;
    private void Awake()
    {
        if (_previewAvatarObject is null)
        {
            _previewAvatarObject = GameObject.FindWithTag("Player");
            PlayerManager.Players.LocalPlayerGo = _previewAvatarObject;
        }
    }

    private void Start()
    {
        ClickButton(_selected);
    }



    public void ClickButton(int id)
    {
        for(int i = 0; i < _customizingTextureButtons.Count; i++)
        {
            _selected = id;
            if (i == id)
            {

                _customizingTextureButtons[i].Select();
            }
            else
            {
                _customizingTextureButtons[i].Deselect();
            }
        }
    }

    public void ColorReset()
    {
        Fcp.color = Color.red;
        Fcp.color = _normal;
        Fcp.mode = FlexibleColorPicker.MainPickingMode.SV;
        //_avatarMaterial.color = _normal;
    }

    public void ResetSelected()
    {
        _selected = 0;
    }
}
