using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class markerdrag : MonoBehaviour
{
    // Start is called before the first frame update

    public int offset = 0;

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

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y,2 + offset);
        Debug.Log("offset="+offset);
        //Vector3 mousePosition = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.z);
        this.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }
    // Update is called once per frame
    void Update()
    {
      //if(Input.GetMouseButtonDown(1))
      if(Input.GetKeyDown(KeyCode.UpArrow))
      {
          offset = -1;
          Debug.Log("right=" + offset);
      }

      // else
        if(Input.GetKeyDown(KeyCode.DownArrow))
      {
          offset = 0;
          Debug.Log("left=" + offset);
      }

    }

}
