using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    public Texture2D texture;
    public Vector2 textureSize = new Vector2(2048, 2048);
    //public  var r= GetComponent<Renderer> ();

    void Start()
    {
        var r= GetComponent<Renderer> ();
        texture= new Texture2D((int)textureSize.x, (int)textureSize.y);
        r.material.mainTexture = texture;
    }
    void Update()
    {
      /*
      if (Input.GetKeyDown (KeyCode.Space))
      {
        print("test");
        DestroyImmediate(texture);
        var r= GetComponent<Renderer> ();
        texture= new Texture2D((int)textureSize.x, (int)textureSize.y);
        r.material.mainTexture = texture;
      }
      */
    }

}
