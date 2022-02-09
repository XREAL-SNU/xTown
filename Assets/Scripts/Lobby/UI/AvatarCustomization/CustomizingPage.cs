using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XReal.XTown.UI;

public class CustomizingPage : UIBase
{
    enum GameObjects
    {
        ContentPanel,
        ResetButton,
        RandomButton
    }

    private void Start()
    {
        Init();
    }

    private string _partName;
    private int _partsIndex = 0;

    private VerticalLayoutGroup _verticalLayout;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        GetUIComponent<GameObject>((int)GameObjects.ResetButton).gameObject.BindEvent(ResetCustomizing);
        GetUIComponent<GameObject>((int)GameObjects.RandomButton).gameObject.BindEvent(RandomCustomizing);

        GameObject contentPanel = GetUIComponent<GameObject>((int)GameObjects.ContentPanel);
        _verticalLayout = contentPanel.GetComponent<VerticalLayoutGroup>();
        Debug.Log(AvatarAppearanceNew.Descriptor.Parts[0].Properties[0].PropertyName);
        if (AvatarAppearanceNew.CustomParts.ContainsKey(_partName))
        {
            foreach(ObjectPart parts in AvatarAppearanceNew.Descriptor.Parts)
            {
                _partsIndex++;
                if (parts.PartName == _partName)
                {
                    for (int i = 0; i < parts.Properties.Length; i++)
                    {
                        GameObject buttons = UIManager.UI.MakeSubItem<CustomizingButtonGroup>(contentPanel.transform).gameObject;
                        CustomizingButtonGroup button = buttons.GetOrAddComponent<CustomizingButtonGroup>();
                        button.SetInfo(_partName, parts.Properties[i].PropertyName, _partsIndex);
                    }
                    break;
                }
            }
        }
    }

    public void SetInfo(string name)
    {
        _partName = name;
        this.name = name;
    }



    public void ResetCustomizing(PointerEventData data)
    {

    }

    public void RandomCustomizing(PointerEventData data)
    {

    }
}
