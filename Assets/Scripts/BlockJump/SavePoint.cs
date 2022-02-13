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
        GameManager.savePoint = other.transform.position; // savepoint에 현재 플레이어 위치 저장
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
