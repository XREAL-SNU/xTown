using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteElement
{
    private Color[] _colorElement = new Color[] { Color.white, Color.blue, Color.green, Color.red };
    private Material[] _materialElementBackPack;
    private Material[] _materialElementBody;
    private Material[] _materialElementBoot;
    private Material[] _materialElementGlove;
    private Material[] _materialElementHelmet;

    public Color[] ColorP { get { return _colorElement; } }

    public Material[] BackPackMaterialP { get { return _materialElementBackPack; } }


}
