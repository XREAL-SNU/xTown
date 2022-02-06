using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SY_CoinScript : MonoBehaviour
{    

    // Update is called once per frame
    void Update()
    {
        //Coin rotation
        transform.Rotate(new Vector3(200f, 0f, 0f) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);

    }

}
