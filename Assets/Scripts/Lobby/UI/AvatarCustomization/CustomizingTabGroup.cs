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

        Debug.Log(AvatarAppearanceNew.MaterialsCount);

        foreach (string partName in PlayerManager.Players.LocalAvatarAppearance.CustomParts.Keys)
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



    /*private ColorElement _btnColor = new ColorElement();

    public List<TabButton> Tabs;
    public TabButton SelectedTab;
    // UI sprites
    public Sprite TabDefaultImage;
    public Sprite TabHoverImage;
    public Sprite TabSelectedImage;

    public List<GameObject> Pages;
    // subscriber pattern for easy extension
    
    public void ResetTab()
    {
        SelectedTab = Tabs[0];
        OnTabSelect(Tabs[0]);
    }

    public void Subscribe(TabButton button)
    {
        if(Tabs is null)
        {
            Tabs = new List<TabButton>();
        }

        Tabs.Add(button);
        SelectedTab = Tabs[0];
        OnTabSelect(Tabs[0]);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if(SelectedTab == null || button != SelectedTab)
        {
            button.Background.sprite = TabHoverImage;
            button.Background.color = _btnColor.ButtonColorP["Enter"];
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelect(TabButton button)
    {
        SelectedTab = button;
        ResetTabs();
        button.Background.sprite = TabSelectedImage;
        button.Background.color = _btnColor.ButtonColorP["Select"];

        // select page based on order in hierarchy.
        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i < Pages.Count; i++)
        {
            if (index == i) Pages[i].SetActive(true);
            else Pages[i].SetActive(false);
        }
    }

    public void ResetTabs()
    {
        foreach(TabButton button in Tabs)
        {
            if (SelectedTab != null && button == SelectedTab) continue;
            button.Background.sprite = TabDefaultImage;
            button.Background.color = _btnColor.ButtonColorP["Base"];
        }
    }*/
}
