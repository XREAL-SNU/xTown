using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Penthickness : MonoBehaviour
{

    public int thickness = 3;
    public void thicknesschoose3()
    {
        thickness=3;
        Debug.Log("most thick");
        WhiteboardMarker WBM_R = GameObject.Find("Red Marker").GetComponent<WhiteboardMarker>();
        WBM_R._penSize = 50;
        WhiteboardMarker WBM_B = GameObject.Find("Blue Marker").GetComponent<WhiteboardMarker>();
        WBM_B._penSize = 50;
        WhiteboardMarker WBM_BL = GameObject.Find("Black Marker").GetComponent<WhiteboardMarker>();
        WBM_BL._penSize = 50;
        WhiteboardMarker WBM_Y = GameObject.Find("Yellow Marker").GetComponent<WhiteboardMarker>();
        WBM_Y._penSize = 50;
        WhiteboardMarker WBM_G = GameObject.Find("Green Marker").GetComponent<WhiteboardMarker>();
        WBM_G._penSize = 50;
    }

    public void thicknesschoose2()
    {
        thickness=2;
        Debug.Log("middle thick");
        WhiteboardMarker WBM_R = GameObject.Find("Red Marker").GetComponent<WhiteboardMarker>();
        WBM_R._penSize = 30;
        WhiteboardMarker WBM_B = GameObject.Find("Blue Marker").GetComponent<WhiteboardMarker>();
        WBM_B._penSize = 30;
        WhiteboardMarker WBM_BL = GameObject.Find("Black Marker").GetComponent<WhiteboardMarker>();
        WBM_BL._penSize = 30;
        WhiteboardMarker WBM_Y = GameObject.Find("Yellow Marker").GetComponent<WhiteboardMarker>();
        WBM_Y._penSize = 30;
        WhiteboardMarker WBM_G = GameObject.Find("Green Marker").GetComponent<WhiteboardMarker>();
        WBM_G._penSize = 30;
    }

    public void thicknesschoose1()
    {
        thickness=1;
        Debug.Log("least thick");
        WhiteboardMarker WBM_R = GameObject.Find("Red Marker").GetComponent<WhiteboardMarker>();
        WBM_R._penSize = 10;
        WhiteboardMarker WBM_B = GameObject.Find("Blue Marker").GetComponent<WhiteboardMarker>();
        WBM_B._penSize = 10;
        WhiteboardMarker WBM_BL = GameObject.Find("Black Marker").GetComponent<WhiteboardMarker>();
        WBM_BL._penSize = 10;
        WhiteboardMarker WBM_Y = GameObject.Find("Yellow Marker").GetComponent<WhiteboardMarker>();
        WBM_Y._penSize = 10;
        WhiteboardMarker WBM_G = GameObject.Find("Green Marker").GetComponent<WhiteboardMarker>();
        WBM_G._penSize = 10;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
