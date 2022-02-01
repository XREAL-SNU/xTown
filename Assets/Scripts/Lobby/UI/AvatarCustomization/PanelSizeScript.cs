using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSizeScript : MonoBehaviour
{
    private TextureElement _txt = new TextureElement();
    private GameObject _btnPrefab;
    private GridLayoutGroup _gridLayout;

    private void Start()
    {
        _btnPrefab = Resources.Load<GameObject>("Button");
        _gridLayout = this.GetComponent<GridLayoutGroup>();
        _createBtns();
        _panelSize();
    }

    private void _createBtns()
    {
        for (int i = 0; i < _txt.TextureNum[CustomizingPanelScript.PartID]; i++)
        {
            GameObject button = Instantiate(_btnPrefab);
            button.GetComponentInChildren<Text>().text = CustomizingPanelScript.PartID + (i + 1);
            button.transform.SetParent(this.transform);
        }
    }

    private void _panelSize()
    {
        int d = (int)_gridLayout.cellSize.y + (int)_gridLayout.spacing.y;
        int a0 = 200 + (int)_gridLayout.padding.bottom;
        int panelHeight = (int)(this.transform.childCount / 4) * d + a0;
        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(684, panelHeight);
    }
}
