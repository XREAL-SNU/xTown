using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAppearanceNew
{
    public enum AppearancePropertyTypes
    {
        BaseColor,
        Metallic,
        Emission,
        Texture,
        Transparency
    }


    // private fields
    public Dictionary<string, GameObject> CustomParts = new Dictionary<string, GameObject>();
    ObjectPartsInfo _descriptor;
    public ObjectPartsInfo Descriptor
    {
        get => _descriptor;
    }






    // static default descriptor
    static ObjectPartsInfo _XRealSpaceSuitAppreanceDescriptor;
    public static ObjectPartsInfo XRealSpaceSuitAppearanceDescriptor
    {
        get
        {
            if(_XRealSpaceSuitAppreanceDescriptor is null)
            {
                ObjectPartsInfo objectPartsInfo = new ObjectPartsInfo();
                objectPartsInfo.ObjectType = "Avatar";
                objectPartsInfo.ObjectName = "SpaceSuitAvatar";

                ObjectPart partBackpack = new ObjectPart();
                partBackpack.PartName = "Backpack";
                partBackpack.PartPath = "Space_Suit/Tpose_/Man_Suit/Backpack";
                partBackpack.Properties = new ObjectPartProperty[2];
                partBackpack.Properties[0] = new ObjectPartProperty("Metallic", AppearancePropertyTypes.Metallic);
                partBackpack.Properties[1] = new ObjectPartProperty("Color", AppearancePropertyTypes.BaseColor);


                ObjectPart partBody = new ObjectPart();
                partBody.PartName = "Body";
                partBody.PartPath = "Space_Suit/Tpose_/Man_Suit/Body";
                partBody.Properties = new ObjectPartProperty[2];
                partBody.Properties[0] = new ObjectPartProperty("Metallic", AppearancePropertyTypes.Metallic);
                partBody.Properties[1] = new ObjectPartProperty("Color", AppearancePropertyTypes.BaseColor);

                ObjectPart partBoots = new ObjectPart();
                partBoots.PartName = "Boot";
                partBoots.PartPath = "Space_Suit/Tpose_/Man_Suit/Boot";
                partBoots.Properties = new ObjectPartProperty[2];
                partBoots.Properties[0] = new ObjectPartProperty("Metallic", AppearancePropertyTypes.Metallic);
                partBoots.Properties[1] = new ObjectPartProperty("Color", AppearancePropertyTypes.BaseColor);

                ObjectPart partGloves = new ObjectPart();
                partGloves.PartName = "Glove";
                partGloves.PartPath = "Space_Suit/Tpose_/Man_Suit/Glove";
                partGloves.Properties = new ObjectPartProperty[2];
                partGloves.Properties[0] = new ObjectPartProperty("Metallic", AppearancePropertyTypes.Metallic);
                partGloves.Properties[1] = new ObjectPartProperty("Color", AppearancePropertyTypes.BaseColor);

                ObjectPart partHelmet = new ObjectPart();
                partHelmet.PartName = "Helmet";
                partHelmet.PartPath = "Space_Suit/Tpose_/Man_Suit/Helmet";
                partHelmet.Properties = new ObjectPartProperty[2];
                partHelmet.Properties[0] = new ObjectPartProperty("Metallic", AppearancePropertyTypes.Metallic);
                partHelmet.Properties[1] = new ObjectPartProperty("Color", AppearancePropertyTypes.BaseColor);


                objectPartsInfo.Parts = new ObjectPart[] { partBackpack, partBody, partBoots, partGloves, partHelmet};
                _XRealSpaceSuitAppreanceDescriptor = objectPartsInfo;

                string json = JsonUtility.ToJson(objectPartsInfo);
                Debug.Log(json);
            }

            return _XRealSpaceSuitAppreanceDescriptor;
        }
    }

    // ctors
    public AvatarAppearanceNew(ObjectPartsInfo info, GameObject target)
    {
        Debug.Log("AvatarApperanceNew/ ctor binding info to target");
        _descriptor = info;
        foreach(ObjectPart part in info.Parts)
        {
            GameObject go = target.transform.Find(part.PartPath).gameObject;
            if(go is null)
            {
                Debug.LogError($"AvatarAppearanceNew/ part with path {part.PartPath} cannot be found");
                continue;
            }

            // create copy of preset materials.
            AddCustomPart(go, part.PartName);

            ApplyProperties(go, part.Properties);
        }
    }

    public AvatarAppearanceNew() { }
    public static int MaterialsCount = 0;

    public void AddCustomPart(GameObject go, string PartName)
    {
        CustomParts.Add(PartName, go);
        Material mat = go.GetComponent<Renderer>().material;
        mat = new Material(mat);

        MaterialsCount++;
        Debug.Log($"custom part {PartName}, material count: {MaterialsCount}");
    }


    // setters
    public void SetProperty(GameObject go, string propertyName, AppearancePropertyTypes type, string palette, int pick)
    {
        if(go is null)
        {
            Debug.LogError("AvatarAppearanceNew/ SetOrAddBaseColor: gameobject is null");
            return;
        }

        // set descriptor and apply property
        foreach(ObjectPart part in Descriptor.Parts)
        {
            if (part.PartName.Equals(go.name))
            {
                ObjectPartProperty prop = part.SetProperty(propertyName, type, palette, pick);
                ApplyProperty(go, prop);
            }
        }
    }

    // getter
    public GameObject GetCustomPartGo(string partName)
    {
        GameObject value;
        CustomParts.TryGetValue(partName, out value);
        if(value is null)
        {
            Debug.LogError($"AvatarAppearanceNew/ Cannot fetch part name: {partName}");
        }
        return value;
    }

    public ObjectPart GetCustomPart(string partName)
    {
        foreach(ObjectPart part in _descriptor.Parts)
        {
            if (part.PartName.Equals(partName))
            {
                return part;
            }
        }

        Debug.LogError($"AvatarAppearanceNew/ cannot fetch part {partName}");
        return null;
    }
    // applier (provide the avatar to apply)
    public void Apply(GameObject target)
    {
        Debug.Log("AvatarAppearanceNew/ Applying Apperance");
        foreach (ObjectPart part in Descriptor.Parts)
        {
            GameObject go = target.transform.Find(part.PartPath).gameObject;
            if (go is null)
            {
                Debug.LogError($"AvatarAppearanceNew/ part with path {part.PartPath} cannot be found");
                continue;
            }
            ApplyProperties(go, part.Properties);
        }
    }

    public void ApplyProperty(GameObject go, ObjectPartProperty prop)
    {
        AppearancePropertyTypes type = (AppearancePropertyTypes)Enum.Parse(typeof(AppearancePropertyTypes), prop.PropertyType, true);
        string paletteName = prop.PaletteName;
        int pick = prop.Pick;
        switch (type)
        {
            case AppearancePropertyTypes.BaseColor:
                ApplyBaseColor(go, ColorPalette.GetXrealPalette(paletteName)[pick]);
                break;
            case AppearancePropertyTypes.Metallic:
                ApplyMetallic(go, LinearPalette.GetXrealPalette(paletteName)[pick]);
                break;
        }
    }


    void ApplyProperties(GameObject go, ObjectPartProperty[] properties)
    {
        for (int i = 0; i < properties.Length; i++)
        {
            AppearancePropertyTypes type = (AppearancePropertyTypes)Enum.Parse(typeof(AppearancePropertyTypes), properties[i].PropertyType, true);
            string paletteName = properties[i].PaletteName;
            int pick = properties[i].Pick;
            switch (type)
            {
                case AppearancePropertyTypes.BaseColor:
                    ApplyBaseColor(go, ColorPalette.GetXrealPalette(paletteName)[pick]);
                    break;
                case AppearancePropertyTypes.Metallic:
                    ApplyMetallic(go, LinearPalette.GetXrealPalette(paletteName)[pick]);
                    break;
            }
        }
    }
    public void ApplyBaseColor(GameObject obj, XTownColor color)
    {
        Material mat = obj.GetComponent<Renderer>().material;
        mat.SetColor("_Color", color.ToColor());
        Debug.Log("AvatarApperanceNew/ Applying BaseColor " + color.ToColor());

    }

    public void ApplyMetallic(GameObject obj, float value)
    {
        Material mat = obj.GetComponent<Renderer>().material;
        mat.SetFloat("_Metallic", value);
        Debug.Log("AvatarApperanceNew/ Applying Metallic " + value);

    }




}

[Serializable]
public class ObjectPartsInfo
{
    // Avatar, Axe, Desk... any object
    public string ObjectType;
    // SpaceSuit, Alienware, ...
    public string ObjectName;
    public ObjectPart[] Parts;
}


[Serializable]
public class ObjectPart
{
    public string PartName;
    public string PartPath;
    public ObjectPartProperty[] Properties;

    public ObjectPart(string name, string path)
    {
        PartName = name;
        PartPath = path;
    }

    public ObjectPart() { }

    // getters and setters

    public ObjectPartProperty this[string name]
    {
        get
        {
            ObjectPartProperty prop = null;
            for (int i = 0; i < Properties.Length; ++i)
            {
                if (Properties[i].PropertyName.Equals(name))
                {
                    prop = Properties[i];
                }
            }
            if (prop is null)
            {
                Debug.LogError($"AvatarAppearanceNew/ property with name {name} does not exist");
            }
            return prop;
        }

    }
    public ObjectPartProperty SetProperty(string name, AvatarAppearanceNew.AppearancePropertyTypes type, string paletteName, int pick)
    {
        ObjectPartProperty prop = null;
        for(int i = 0; i < Properties.Length; i++)
        {
            if (Properties[i].PropertyName.Equals(name))
            {
                prop = Properties[i];
                prop.PaletteName = paletteName;
                prop.Pick = pick;
                Debug.Log($"Edited property {prop.PropertyName} to {paletteName}.{pick}");
            }
        }
        if (prop is null)
        {
            Debug.LogError($"property with name {name} does not exist");
        }
        return prop;
    }
}

[Serializable]
public class ObjectPartProperty
{
    public string PropertyName;
    // predefined set, ex: BaseColor, Metallic,.. , from enum.
    // it is NOT type names(Color, float, Image..)
    public string PropertyType;
    public string PaletteName;
    public int Pick;

    public ObjectPartProperty(string name, AvatarAppearanceNew.AppearancePropertyTypes type)
    {
        PropertyName = name;
        PropertyType = type.ToString();

        switch (type)
        {
            case AvatarAppearanceNew.AppearancePropertyTypes.BaseColor:
                PaletteName = ColorPalette.DefaultColorPalette.PaletteName;
                Pick = 0;
                break;
            case AvatarAppearanceNew.AppearancePropertyTypes.Metallic:
            case AvatarAppearanceNew.AppearancePropertyTypes.Emission:
            case AvatarAppearanceNew.AppearancePropertyTypes.Transparency:
                PaletteName = LinearPalette.DefaultLinearPalette.PaletteName;
                Pick = 0;
                break;
        }
    }

    public ObjectPartProperty(string name, AvatarAppearanceNew.AppearancePropertyTypes type,
        string paletteName, int defaultPick)
    {
        PropertyName = name;
        PropertyType = type.ToString();
        PaletteName = paletteName;
        Pick = defaultPick;
    }
    public ObjectPartProperty() { }

    // setter
    public ObjectPartProperty SetProperty(string paletteName, int pick)
    {
        // maybe we should check existence of palette
        // and range validness of pick?
        PaletteName = paletteName;
        Pick = pick;
        return this;
    }
}
