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


    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        GetUIComponent<GameObject>((int)GameObjects.ResetButton).gameObject.BindEvent(OnButtonEnter, UIEvents.UIEvent.Enter);
        GetUIComponent<GameObject>((int)GameObjects.RandomButton).gameObject.BindEvent(OnButtonEnter, UIEvents.UIEvent.Enter);
        GetUIComponent<GameObject>((int)GameObjects.ResetButton).gameObject.BindEvent(OnButtonExit, UIEvents.UIEvent.Exit);
        GetUIComponent<GameObject>((int)GameObjects.RandomButton).gameObject.BindEvent(OnButtonExit, UIEvents.UIEvent.Exit);

        GameObject contentPanel = GetUIComponent<GameObject>((int)GameObjects.ContentPanel);
        AvatarAppearanceNew appearance = PlayerManager.Players.LocalAvatarAppearance;
        Debug.Log(appearance.Descriptor.Parts[0].Properties[0].PropertyName);

        if (appearance.CustomParts.ContainsKey(_partName))
        {
            foreach(ObjectPart parts in appearance.Descriptor.Parts)
            {
                if (parts.PartName == _partName)
                {
                    for (int i = 0; i < parts.Properties.Length; ++i)
                    {
                        GameObject buttons = UIManager.UI.MakeSubItem<CustomizingButtonGroup>(contentPanel.transform).gameObject;
                        CustomizingButtonGroup button = buttons.GetOrAddComponent<CustomizingButtonGroup>();
                        button.SetInfo(_partName, parts.Properties[i].PropertyName, _partsIndex, parts.Properties[i].TextColor);
                    }
                    break;
                }
                _partsIndex++;
            }
        }
    }

    public void SetInfo(string name)
    {
        _partName = name;
        this.name = name;
    }

    public void OnButtonEnter(PointerEventData data)
    {
        GameObject btn = data.pointerEnter.transform.parent.gameObject;
        btn.GetComponent<Image>().color = XTownColor.XTownGrey.ToColor();
    }

    public void OnButtonExit(PointerEventData data)
    {
        GameObject btn = data.pointerEnter.transform.parent.gameObject;
        btn.GetComponent<Image>().color = XTownColor.XTownWhite.ToColor();
    }
}
