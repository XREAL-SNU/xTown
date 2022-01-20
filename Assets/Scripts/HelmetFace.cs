using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetFace : MonoBehaviour
{
    [Tooltip("Helmet Face GameObject")]
    public GameObject HelmetFaceObject;

    [Tooltip("Add Face Textures Here")]
    [SerializeField] List<Material> _helmetTextures;

    Renderer _helmetRenderer;

    // Start is called before the first frame update
    void Start()                { _helmetRenderer = HelmetFaceObject.GetComponent<Renderer>(); }

    // Call this Function from Manager (AvatarM, EmojiM ... )
    void ChangeFace(int index)  { _helmetRenderer.material = _helmetTextures[index]; }    
}
