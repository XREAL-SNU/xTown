using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XReal.XTown.UI;

public class CustomizingTabGroup : UIScene
{
    public string SelectedTab;

    enum GameObjects
    {
        TabPanel,
        PagePanel
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        GameObject tabPanel = GetUIComponent<GameObject>((int)GameObjects.TabPanel);
        GameObject pagePanel = GetUIComponent<GameObject>((int)GameObjects.PagePanel);

        foreach (string partName in AvatarAppearanceNew.CustomParts.Keys)
        {
            Debug.Log(partName);
            GameObject tabs = UIManager.UI.MakeSubItem<CustomizingTab>(tabPanel.transform).gameObject;
            CustomizingTab tab = tabs.GetOrAddComponent<CustomizingTab>();
            tab.SetInfo(partName);
            GameObject pages = UIManager.UI.MakeSubItem<CustomizingPage>(pagePanel.transform).gameObject;
            CustomizingPage page = pages.GetOrAddComponent<CustomizingPage>();
            page.SetInfo(partName);
        }

    }

    public void OpenPage()
    {
        GameObject pagePanel = GetUIComponent<GameObject>((int)GameObjects.PagePanel);
        foreach(Transform page in pagePanel.GetComponentInChildren<Transform>())
        {
            if (page.name == SelectedTab) page.gameObject.SetActive(true);
            else page.gameObject.SetActive(false);
        }
    }
}
