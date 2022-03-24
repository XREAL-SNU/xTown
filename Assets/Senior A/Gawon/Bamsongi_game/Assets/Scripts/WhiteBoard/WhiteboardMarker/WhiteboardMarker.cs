using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WhiteboardMarker : MonoBehaviour
{
    [SerializeField] private Transform _tip;
    [SerializeField] public int _penSize = 50;

    private Renderer _renderer;
    private Color[] _colors,_colors2;
    private Color _color_tmp;
    private float _tipHeight;

    private RaycastHit _touch;
    private Whiteboard _whiteboard;
    private Vector2 _touchPos, _lastTouchPos;
    private bool _touchedLastFrame;
    private Quaternion _lastTouchRot;

    void Start()
    {
        _renderer = _tip.GetComponent<Renderer>();
        _colors = Enumerable.Repeat(_renderer.material.color, _penSize*_penSize).ToArray();
        _colors2 = Enumerable.Repeat(_renderer.material.color, 2048*2048).ToArray();
        _tipHeight = _tip.localScale.y;
    }
/*
    void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y,10);
        this.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }
*/
    void Update()
    {
      _colors = Enumerable.Repeat(_renderer.material.color, _penSize*_penSize).ToArray();
        Draw();
        //OnMouseDrag();
    }
    private void Draw()
    {
      /*
        if (Input.GetKeyDown (KeyCode.Space))
        {
          //print("test1");
          if (_whiteboard == null)
          {
              _whiteboard= _touch.transform.GetComponent<Whiteboard>();
          }
           //_whiteboard.texture.SetPixels(0.0,0.0, _whiteboard.textureSize.x,_whiteboard.textureSize.y, _colors2);
           _whiteboard.texture.SetPixels(0,0, 2048,2048, _colors2);
        }
      */
        if (Physics.Raycast(_tip.position,transform.up, out _touch, _tipHeight))
        {
            if(_touch.transform.CompareTag("Whiteboard"))
            {
               if (_whiteboard == null)
               {
                   _whiteboard= _touch.transform.GetComponent<Whiteboard>();
               }
               _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

               var x=(int)(_touchPos.x * _whiteboard.textureSize.x - (_penSize/2));
               var y=(int)(_touchPos.y * _whiteboard.textureSize.y - (_penSize/2));
               if (y<0 || y> _whiteboard.textureSize.y || x<0 || x> _whiteboard.textureSize.x) return;

               if (_touchedLastFrame)
               {
                   _whiteboard.texture.SetPixels(x,y,_penSize, _penSize, _colors);

                   for (float f=0.01f; f< 1.00f; f +=0.01f)
                   {
                       var lerpX= (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                       var lerpY= (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                       _whiteboard.texture.SetPixels(lerpX,lerpY,_penSize, _penSize, _colors);
                   }

                   transform.rotation= _lastTouchRot;
                   _whiteboard.texture.Apply();
               }

               _lastTouchPos = new Vector2(x,y);
               _lastTouchRot = transform.rotation;
               _touchedLastFrame = true;
               return;


            }
        }
        _whiteboard = null;
        _touchedLastFrame = false;
    }
}
