using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockResize : MonoBehaviour
{
    /* 플레이어가 블록에 접촉 시, 블록이 절반 크기로 줄어들게 합니다 */

    public float resizeDuration = 2f; //블록 크기 축소 소요 시간
    private float t; //축소 시간 제어를 위한 보조 변수

    private bool isPlayerOnBlock = false; //플레이어의 블록 접촉 여부
    private Vector3 originalScale; //블록의 초기 크기
    private Vector3 targetScale; //블록의 축소된 크기


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

    void Update()
    {
        if (isPlayerOnBlock) //플레이어가 블록에 접촉한 경우
        {
            ShrinkObject(); //블록 크기 축소
            if(transform.localScale == targetScale)
            {
                //블록이 절반 크기가 되면 축소 중단
                isPlayerOnBlock = false;
            }
        }
    }

    void ShrinkObject()
    {
        /* 블록 크기를 축소시키는 함수 */
        t += Time.deltaTime / resizeDuration;
        Vector3 newScale = Vector3.Lerp(originalScale, targetScale, t);
        transform.localScale = newScale;
    }
}

