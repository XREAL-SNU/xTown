using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    private GameObject PortalExit;
    private Rigidbody PlayerRb;

    private bool isEnter = false;

    // Start is called before the first frame update
    void Start()
    {
        PortalExit = GameObject.Find("Descending/BlockStaticSP");
    }
    void OnTriggerEnter(Collider other)
    {
        other.transform.position = PortalExit.transform.position; //PortalExit 위치로 이동
        other.GetComponent<Rigidbody>().velocity = Vector3.zero; //이동한 위치에서 정지
    }

}
