using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[System.Serializable]
public class TextureArray2
{
    public Texture[] Textures;
}

public class CustomizingPanelScript2 : MonoBehaviour
{
    [SerializeField]
    private string _customPartId;
    [SerializeField]
    private PlayerAvatar _avatar;
    [SerializeField]
    private Material _avatarMaterial;
    [SerializeField]
    private List<CustomizingButtonScript> _customizingColorButtons;
    [SerializeField]
    private List<CustomizingButtonScript> _customizingTextureButtons;
    [SerializeField]
    private TextureArray2[] _textures;

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
        /* OLD
        if (_avatarAppearance is null)
        { // only once! create the local avatar appearance and store reference to it.
            AvatarAppearance.LocalAvatarAppearance = new AvatarAppearance();
            _avatarAppearance = AvatarAppearance.LocalAvatarAppearance;
        } 
        */
    }

    private void Start()
    {
        //ClickButton(_selected);
    }

    private void Update()
    {
        /* OLD
        _avatarAppearance[_customPartId].SetMaterialBaseColor(Fcp.color);
        _avatarAppearance.ApplyAppearance(_avatar);
        */
        //_avatarMaterial.color= Fcp.color;
    }
    /*
    public void ClickButton(int id)
    {
        for(int i = 0; i < _customizingTextureButtons.Count; i++)
        {
            _selected = id;
            if (i == id)
            {
                _avatarMaterial.SetTexture("_MainTex", _textures[i].Textures[0]);
                _avatarMaterial.SetTexture("_MetallicGlossMap", _textures[i].Textures[1]);
                _avatarMaterial.SetTexture("_BumpMap", _textures[i].Textures[2]);
                _avatarMaterial.SetTexture("_OcclusionMap", _textures[i].Textures[3]);
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
        _avatarMaterial.color = _normal;
    }

    public void ResetSelected()
    {
        _selected = 0;
    }*/
}
