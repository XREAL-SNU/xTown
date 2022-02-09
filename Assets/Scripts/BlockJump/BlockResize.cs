using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockResize : MonoBehaviour
{
    public float resizeDuration = 2f;
    private Vector3 originalScale;
    private Vector3 targetScale;
    float t;
    private bool isPlayerOnBlock=false;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        targetScale = new Vector3(originalScale.x / 2f, originalScale.y, originalScale.z / 2f);
        t = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        isPlayerOnBlock = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerOnBlock)
        {
            ShrinkObject();
            if(transform.localScale == targetScale)
            {
                isPlayerOnBlock = false;
            }
        }
    }

    void ShrinkObject()
    {
        t += Time.deltaTime / resizeDuration;
        Vector3 newScale = Vector3.Lerp(originalScale, targetScale, t);
        transform.localScale = newScale;
    }

}

