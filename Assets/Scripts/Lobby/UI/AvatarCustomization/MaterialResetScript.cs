using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialResetScript : MonoBehaviour
{
    [SerializeField]
    private List<Material> _avatarMaterial;

    private Color _normal = new Color(255 / 255, 255 / 255, 255 / 255, 255 / 255);

    private void Start()
    {
        MaterialReset();
    }


    public void MaterialReset()
    {
        for (int i = 0; i < _avatarMaterial.Count; i++)
        {
            _avatarMaterial[i].color = _normal;
        }
    }
}
