using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ColorPalette
{

    public string PaletteName;
    public XTownColor[] ColorsSet;

    public XTownColor this[int index]
    {
        get
        {
            if(index < ColorsSet.Length)
                return ColorsSet[index];
            else
            {
                Debug.LogError("Palette/ requested index outside range");
                return null;
            }
        }
    }

    public static XTownColor[] DefaultColorsSet = new XTownColor[] {
        XTownColor.XTownWhite, XTownColor.XTownGrey, XTownColor.XTownRed, XTownColor.XTownMagenta, XTownColor.XTownYellow, XTownColor.XTownGreen, XTownColor.XTownCyan, XTownColor.XTownBlue
    };

    public static XTownColor[] DarkColorsSet = new XTownColor[] {
        new XTownColor("WineRed", 115/255, 32/255, 25/255, 1.0f), 
        new XTownColor("DeepBlue", 29/255, 47/255, 115/255, 1.0f)
    };

    public static ColorPalette DefaultColorPalette = new ColorPalette("DefaultColorPalette");
    public static ColorPalette DefaultColorPaletteDark = new ColorPalette("DefaultColorPaletteDark", DarkColorsSet);


    static List<ColorPalette> XRealPalettes = new List<ColorPalette> { DefaultColorPalette, DefaultColorPaletteDark };
    public static ColorPalette GetXrealPalette(string name)
    {
        foreach(ColorPalette palette in XRealPalettes)
        {
            if (palette.PaletteName == name) return palette;
        }

        Debug.LogError("Palette/ No palette named: " + name);
        return null;
    }
    public ColorPalette(string name)
    {
        PaletteName = name;
        ColorsSet = DefaultColorsSet;
    }
    public ColorPalette(string name, XTownColor[] colors)
    {
        PaletteName = name;
        ColorsSet = colors;
    }
    public ColorPalette() {
        ColorsSet = DefaultColorsSet;
    }
}

[Serializable]
public class LinearPalette
{
    public string PaletteName;
    public float[] ValuesSet;

    public static LinearPalette DefaultLinearPalette = new LinearPalette("DefaultLinearPalette", new float[] { 0.0f, 0.3f, 0.7f, 1.0f });

    static List<LinearPalette> XRealPalettes = new List<LinearPalette> { DefaultLinearPalette };


    public static LinearPalette GetXrealPalette(string name)
    {
        foreach (LinearPalette palette in XRealPalettes)
        {
            if (palette.PaletteName == name) return palette;
        }

        Debug.LogError("Palette/ No palette named: " + name);
        return null;
    }

    public LinearPalette(string name, float[] valuesSet)
    {
        PaletteName = name;
        ValuesSet = valuesSet;
    }

    public LinearPalette() { }

    public float this[int index]
    {
        get
        {
            if (index < ValuesSet.Length)
                return ValuesSet[index];
            else
            {
                Debug.LogError("Palette/ requested index outside range");
                return 0;
            }
        }
    }
}




[Serializable]
public class XTownColor
{
    public static XTownColor XTownWhite = new XTownColor("XTownWhite", Color.white);
    public static XTownColor XTownRed = new XTownColor("XTownRed", new Color(0.7f, 0.2f, 0.3f, 1.0f));
    public static XTownColor XTownBlue = new XTownColor("XTownBlue", new Color(0.2f, 0.3f, 0.7f, 1.0f));
    public static XTownColor XTownGreen = new XTownColor("XTownGreen", new Color(0.2f, 0.7f, 0.3f, 1.0f));
    public static XTownColor XTownYellow = new XTownColor("XTownYellow", Color.yellow);
    public static XTownColor XTownGrey = new XTownColor("XTownGrey", Color.grey);
    public static XTownColor XTownMagenta = new XTownColor("XTownMagenta", Color.magenta);
    public static XTownColor XTownBlack = new XTownColor("XTownBlack", Color.black);
    public static XTownColor XTownCyan = new XTownColor("XTownCyan", Color.cyan);

    public static XTownColor ButtonOutlineDefault = new XTownColor("ButtonOutlineDefault", new Color(0, 0, 0, 0));
    public static XTownColor ButtonOutlineEnter = new XTownColor("ButtonOutlineEnter", new Color32(40, 56, 195, 255));
    public static XTownColor ButtonOutlineClick = new XTownColor("ButtonOutlineClick", new Color32(67, 86, 255, 255));

    public XTownColor(string name, float r, float g, float b, float a)
    {
        colorName = name;
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public XTownColor(float com)
    {
        this.r = com;
        this.g = com;
        this.b = com;
        this.a = 1f;
    }

    public XTownColor(string name, Color col)
    {
        colorName = name;
        r = col.r;
        g = col.g;
        b = col.b;
        a = col.a;
    }

    public XTownColor() { }

    // Deserializer
    public Color ToColor()
    {
        return new Color(r, g, b, a);
    }
    public Color ToColor(float a)
    {
        this.a = a;
        return new Color(r, g, b, a);
    }

    public string colorName;
    public float r;
    public float g;
    public float b;
    public float a;
}