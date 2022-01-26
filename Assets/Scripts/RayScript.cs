using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayScript : MonoBehaviour
{
    RaycastHit hit; //레이저에 닿는 물체
    public GameObject whiteBall;

    // Start is called before the first frame update
    void Start()
    {
       whiteBall = GameObject.Find("Ball_0");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(whiteBall.transform); //큐대가 흰 공을 바라보게 한다

        Vector3 raycastDir = whiteBall.transform.position - transform.position; //레이저 방향 설정
        Debug.DrawRay(this.transform.position, raycastDir * 15f, Color.blue, 0.3f);
        if (Physics.Raycast(this.transform.position, raycastDir, out hit, 15f)) //레이저에 닿는 물체가 있다면
        {
            //var localHit = transform.InverseTransformPoint(hit.point);            
            //Debug.Log(localHit);
            hit.transform.GetComponent<MeshRenderer>().material.color = Color.red; //물체의 색 변경
        }
        else
        {
            
        }
    }
}
