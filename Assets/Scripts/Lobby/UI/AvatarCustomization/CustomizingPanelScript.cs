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
    private string _partID;

    public static string PartID;

    [SerializeField]
    private Material _avatarMaterial;


    private List<CustomizingButtonScript> _customizingColorButtons;
    public List<CustomizingButtonScript> _customizingTextureButtons;

    [SerializeField]
    private TextureArray[] _textures;

    //public FlexibleColorPicker Fcp;

    private Color _normal = new Color(255 / 255, 255 / 255, 255 / 255, 255 / 255);
    public int _selectedTex = 0;
    public int _selectedCol = 0;

    private void Start()
    {
        PartID = _partID;
        //ClickButton(_selected);
    }

    private void Update()
    { 
        /*
        if(_customizingTextureButtons is null)
        {
            _customizingTextureButtons = new List<CustomizingButtonScript>();
            _subscribe();
        }*/
        //_avatarMaterial.color= Fcp.color;
    }

    /*public void ClickButton(int id)
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
                //_customizingTextureButtons[i].Select();
            }
            else
            {
                //_customizingTextureButtons[i].Deselect();
            }
        }
    }*/

    public void ColorReset()
    {
        //Fcp.color = Color.red;
        //Fcp.color = _normal;
        //Fcp.mode = FlexibleColorPicker.MainPickingMode.SV;
        _avatarMaterial.color = _normal;
    }

    /*
    public void ResetSelected()
    {
        _selected = 0;
    }*/
    /*
    private void _subscribe()
    {
        Debug.Log("in");
        GameObject texbtnGroup = this.transform.GetChild(1).gameObject;
        Debug.Log("in");
        if (texbtnGroup.transform.childCount != 0)
        {
            Debug.Log("inin");
            for (int i = 0; i < texbtnGroup.transform.childCount; i++)
            {
                GameObject tempbtn = texbtnGroup.transform.GetChild(i).gameObject;
                _customizingTextureButtons.Add(tempbtn.GetComponent<CustomizingButtonScript>());
                Debug.Log(i+" in");
            }
        }
    }*/
    
}
