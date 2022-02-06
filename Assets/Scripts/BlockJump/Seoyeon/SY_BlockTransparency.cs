using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SY_BlockTransparency : MonoBehaviour
{
    public float fadeSpeed;
    private bool isPlayerOnBlock = false;

    Material Mat;

    // Start is called before the first frame update
    void Start()
    {
        Mat = GetComponent<Renderer>().material;
        if(fadeSpeed == 0)
        {
            fadeSpeed = 0.01f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        isPlayerOnBlock = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isPlayerOnBlock = false;
        StopAllCoroutines();
        StartCoroutine(FadeInObject());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isPlayerOnBlock == true)
        {
            StartCoroutine(FadeOutObject());
        }
    }

    IEnumerator FadeOutObject()
    {
        while (Mat.color.a > 0.01f)
        {
            Color objectColor = Mat.color;
            float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            Mat.color = objectColor;
            yield return null;
        }
        if (Mat.color.a <= 0.01f)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    IEnumerator FadeInObject()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
        while (Mat.color.a <= 0.5f)
        {
            Color objectColor = Mat.color;
            float fadeAmount = objectColor.a + (Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            Mat.color = objectColor;
            yield return null;
        }
    }
}
