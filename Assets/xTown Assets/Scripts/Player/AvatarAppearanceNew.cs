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
    public string InfoJson
    {
        get {
            return JsonUtility.ToJson(_descriptor);
        }
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
        // we copy info to bind into _descriptor.
        _descriptor = new ObjectPartsInfo();
        _descriptor.CopyFrom(info);
         
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
        // sets the material to a copy of the material
        mat = new Material(mat);

        MaterialsCount++;
        //Debug.Log($"custom part {PartName}, material count: {MaterialsCount}");
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
        for (int i = 0; i < properties.Length; ++i)
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
        //Debug.Log("AvatarApperanceNew/ Applying BaseColor " + color.ToColor());

    }

    public void ApplyMetallic(GameObject obj, float value)
    {
        Material mat = obj.GetComponent<Renderer>().material;
        mat.SetFloat("_Metallic", value);
        //Debug.Log("AvatarApperanceNew/ Applying Metallic " + value);

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

    // ctor
    public ObjectPartsInfo(string type, string name)
    {
        ObjectType = type;
        ObjectName = name;
    }
    public ObjectPartsInfo() { }

    // getter
    public ObjectPart this[string name]
    {
        get
        {
            ObjectPart part = null;
            for (int i = 0; i < Parts.Length; ++i)
            {
                if (Parts[i].PartName.Equals(name))
                {
                    part = Parts[i];
                }
            }
            if (part is null)
            {
                Debug.LogError($"AvatarAppearanceNew/ property with name {name} does not exist");
            }
            return part;
        }
    }


    // recursive copy "constructor" we allocate everything here, assuming previous structure had nothing in it.
    public void CopyFrom(ObjectPartsInfo source)
    {
        // string are copied by reference so we deep copy them.
        this.ObjectType = String.Copy(source.ObjectType);
        this.ObjectName = String.Copy(source.ObjectName);

        if(Parts is null)
        {
            // copy only when not initalized
            Parts = new ObjectPart[source.Parts.Length];
            int i = 0;
            foreach (ObjectPart part in source.Parts)
            {
                ObjectPart m_part = new ObjectPart();
                Parts[i] = m_part;
                m_part.CopyFrom(part);
                ++i;
            }
        }
        else
        {
            Debug.LogError("PlayerAvatar/ Copy failed: Trying to copy into a non null struct");
        }
        
    }
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

    // recursive copy constructor
    public void CopyFrom(ObjectPart part)
    {
        this.PartName = String.Copy(part.PartName);
        this.PartPath = String.Copy(part.PartPath);

        if(Properties is null)
        {
            Properties = new ObjectPartProperty[part.Properties.Length];
            int i = 0;
            foreach (ObjectPartProperty prop in part.Properties)
            {
                // asign them new
                ObjectPartProperty m_prop = new ObjectPartProperty();
                Properties[i] = m_prop;
                m_prop.CopyFrom(prop);
                ++i;
            }
        }
        else
        {
            Debug.LogError("PlayerAvatar/ Copy failed: copying into not null struct");
        }

    }

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
        for(int i = 0; i < Properties.Length; ++i)
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
    public XTownColor TextColor;

    public ObjectPartProperty(string name, AvatarAppearanceNew.AppearancePropertyTypes type)
    {
        PropertyName = name;
        PropertyType = type.ToString();

        switch (type)
        {
            case AvatarAppearanceNew.AppearancePropertyTypes.BaseColor:
                PaletteName = ColorPalette.DefaultColorPalette.PaletteName;
                Pick = 0;
                TextColor = XTownColor.XTownRed;
                break;
            case AvatarAppearanceNew.AppearancePropertyTypes.Metallic:
            case AvatarAppearanceNew.AppearancePropertyTypes.Emission:
            case AvatarAppearanceNew.AppearancePropertyTypes.Transparency:
                PaletteName = LinearPalette.DefaultLinearPalette.PaletteName;
                Pick = 0;
                TextColor = XTownColor.XTownBlue;
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

        switch (type)
        {
            case AvatarAppearanceNew.AppearancePropertyTypes.BaseColor:
                TextColor = XTownColor.XTownRed;
                break;
            case AvatarAppearanceNew.AppearancePropertyTypes.Metallic:
            case AvatarAppearanceNew.AppearancePropertyTypes.Emission:
            case AvatarAppearanceNew.AppearancePropertyTypes.Transparency:
                TextColor = XTownColor.XTownRed;
                break;
        }
    }
    public ObjectPartProperty() { }

    // copy constructor
    public void CopyFrom(ObjectPartProperty source)
    {
        this.PropertyName = source.PropertyName;
        this.PropertyType = source.PropertyType;

        // copy modifiable strings by value.
        this.PaletteName = String.Copy(source.PaletteName);
        this.Pick = source.Pick;
        this.TextColor = source.TextColor;
    }

    // setter
    public ObjectPartProperty SetProperty(string paletteName, int pick)
    {
        // maybe we should check existence of palette
        // and range validness of pick?
        Debug.Log($"Edited property {PropertyName} to {paletteName}.{pick}");

        PaletteName = paletteName;
        Pick = pick;
        return this;
    }
}
