using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureElement
{
    //private
    private string _basePath;
    private string[] _component;
    

    //private(property value)
    private List<List<Texture>> _myTexture;
    private string[] _mySetTexture;
    private Dictionary<string, int> _texNum;

    //property
    public List<List<Texture>> TextureP { get { return _myTexture; } }
    public string[] SetTextureP { get { return _mySetTexture; } }
    public Dictionary<string,int> TextureNum { get { return _texNum; } }


    public TextureElement()
    {
        _basePath = "AvatarTextures/";
        _component = new string[4] { "_Base_Color.png", "_MetallicRoughness.png", "_Noraml_DirectX.png", "_Mixed_AO.png" };
        _mySetTexture = new string[4] { "_MainTex", "_MetallicGlossMap", "_BumpMap", "_OcclusionMap" };
        _texNum = new Dictionary<string, int>() { { "BackPack", 6 }, { "Body", 1 }, { "Boot", 1 }, { "Glove", 1 }, { "Helmet", 1 } };
    }

    public TextureElement(string partID): this()
    {
        if (_myTexture == null && _texNum.ContainsKey(partID))
        {
            for (int i = 1; i <= _texNum[partID]; i++)
            {
                string path1 = _basePath + partID + "/" + i + "/" + partID;
                _myTexture.Add(new List<Texture>());
                for (int j = 0; j < 4; j++)
                {
                    string path2 = path1 + _component[j];
                    _myTexture[j].Add(Resources.Load(path2) as Texture);
                }
            }
        }
    }



}
