using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BamsongiGenerator : MonoBehaviour
{
    public GameObject bamsongiPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bamsongi= Instantiate(bamsongiPrefab) as GameObject;

            Ray ray= Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldDir=ray.direction;
            bamsongi.GetComponent<BamsongiController>().Shoot(worldDir.normalized*2000);
        }
    }
}
