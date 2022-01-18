using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingPanelScript : MonoBehaviour
{
    [SerializeField]
    private GameObject avatarPart;
    [SerializeField]
    private List<CustomizingButtonScript> customizingButtons;
    [SerializeField]
    private List<Material> avatarMaterial;

    public FlexibleColorPicker fcp;

    private int selected = 0;

    private void Start()
    {
        ClickButton(selected);
    }

    private void Update()
    {
        avatarMaterial[selected].color= fcp.color;
    }

    public void ClickButton(int id)
    {
        for(int i = 0; i < customizingButtons.Count; i++)
        {
            selected = id;
            if (i == id)
            {
                avatarPart.gameObject.GetComponent<SkinnedMeshRenderer>().material = avatarMaterial[i];
                customizingButtons[i].Select();
            }
            else
            {
                customizingButtons[i].Deselect();
            }
        }
    }
}
