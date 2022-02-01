using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAppearance 
{
    public static AvatarAppearance LocalAvatarAppearance;


    Dictionary<string, CustomizableElement> customizableElements = new Dictionary<string, CustomizableElement>();
    public AvatarAppearance()
    {
        string suffix = "_Material_Preset1";
        customizableElements.Add("Helmet", new CustomizableElement(Resources.Load<Material>("AvatarMaterials/Helmet" + suffix)));
        customizableElements.Add("Body", new CustomizableElement(Resources.Load<Material>("AvatarMaterials/Body" + suffix)));
        customizableElements.Add("Backpack", new CustomizableElement(Resources.Load<Material>("AvatarMaterials/Backpack" + suffix)));
        
    }


    public CustomizableElement this[string partId]
    {
        get { return customizableElements[partId]; }
        set
        {
            if (customizableElements.ContainsKey(partId))
            {
                customizableElements[partId] = value;
            }
            else
            {
                Debug.LogError("AvatarAppearance/ Cannot SET CustomizableElement");
            }
        }
    }

    public void ApplyAppearance(PlayerAvatar avatar)
    {   
        if (avatar is null) Debug.LogError("AvatarAppearance/ null avatar on apply");
        this["Helmet"].Apply(avatar.transform.Find("Space_Suit/Tpose_/Man_Suit/Helmet").gameObject);
        this["Body"].Apply(avatar.transform.Find("Space_Suit/Tpose_/Man_Suit/Body").gameObject);
        this["Backpack"].Apply(avatar.transform.Find("Space_Suit/Tpose_/Man_Suit/Backpack").gameObject);
    }

    public void SyncAppearance(PlayerAvatar avatar)
    {
        // sync data.
        foreach(var part in customizableElements.Values)
        {
            part.Sync(avatar);
        }

        // apply data.
        ApplyAppearance(avatar);
    }
}
