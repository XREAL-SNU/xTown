using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.savePoint = this.transform.position + new Vector3(0, 10, 0); // savepoint에 위치 저장
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
