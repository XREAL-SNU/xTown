using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class markerdrag : MonoBehaviour
{
    // Start is called before the first frame update

    public int offset = 0;
    public bool dragging;

    void Start()
    {

    }

    void OnMouseDrag()
    {
      /*
        if(Input.GetMouseButtonDown(1))
        {
            offset = -1;
            Debug.Log("right");
        }

        else
        {
            offset = 0;
            Debug.Log("left");
        }
   */
        //offset = 1;
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y,2 + offset);
        Debug.Log("offset="+offset);
        //Vector3 mousePosition = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.z);
        this.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    void OnMouseUpAsButton()
    {
        offset = -1;
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y,2 + offset);
        Debug.Log("offset="+offset);
        //Vector3 mousePosition = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.z);
        this.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    // Update is called once per frame

    void Update()
    {
      //if(Input.GetMouseButtonDown(1))
      //if(Input.GetKeyDown(KeyCode.UpArrow))


      if(Input.GetMouseButtonDown(0))
      {
          offset = 1;
          Debug.Log("left down=" + offset);

      }

      if(Input.GetMouseButtonDown(1))
      {
          offset = -1;
          Debug.Log("right down=" + offset);
      }

      // else
      //if(Input.GetKeyDown(KeyCode.DownArrow))
    /*
      if (Input.GetMouseButtonDown(1))
      {
          offset = -1;
          Debug.Log("right down=" + offset);
          dragging = true;
      }

      if (Input.GetMouseButtonDown(1))
      {
          offset = -1;
          Debug.Log("right up=" + offset);
          dragging = false;
      }

      if(dragging)
      {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y,2 + offset);
        Debug.Log("offset="+offset);
        //Vector3 mousePosition = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.z);
        this.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
      }
     */

    }

}
