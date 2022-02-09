using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTransparency : MonoBehaviour
{
    /* 플레이어가 블록에 접촉 시, 블록이 투명해지게 합니다. */
    /* 플레이어가 접촉하지 않으면 다시 불투명하게 돌아옵니다. */

    Material Mat;
    public float fadeSpeed; //블록 투명화 속도
    private bool isPlayerOnBlock = false; //플레이어의 블록 접촉 여부

    void Start()
    {
        Mat = GetComponent<Renderer>().material;
        if (fadeSpeed == 0)
        {
            fadeSpeed = 0.01f; //투명화 속도 기본값
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        isPlayerOnBlock = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isPlayerOnBlock = false; //플레이어가 블록에 접촉 중이 아닌 경우,
        StopAllCoroutines(); //투명화를 중단하고,
        StartCoroutine(FadeInObject()); //다시 불투명화
    }

    void FixedUpdate()
    {
        if (isPlayerOnBlock == true)
        {
            //플레이어가 블록 접촉 시, 블록 투명화
            StartCoroutine(FadeOutObject());
        }
    }

    IEnumerator FadeOutObject()
    {
        /* 블록 투명화하는 함수 */
        while (Mat.color.a > 0.01f) //블록이 불투명한 경우
        {
            //블록 알파값 감소
            Color ObjectColor = Mat.color;
            float fadeAmount = ObjectColor.a - (fadeSpeed * Time.deltaTime);

            //감소된 알파값으로 블록 색 재설정
            ObjectColor = new Color(ObjectColor.r, ObjectColor.g, ObjectColor.b, fadeAmount);
            Mat.color = ObjectColor;

            yield return null;
        }
        if (Mat.color.a <= 0.01f) //블록이 투명해진 경우
        {
            //블록 콜라이더를 비활성화하여 플레이어가 추락하게 함
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    IEnumerator FadeInObject()
    {
        /* 블록이 다시 불투명해지게 하는 함수 */
        gameObject.GetComponent<BoxCollider>().enabled = true;
        while (Mat.color.a <= 0.5f) //초기 불투명도로 돌아오기 전까지
        {
            // 블록 알파값 증가
            Color objectColor = Mat.color;
            float fadeAmount = objectColor.a + (Time.deltaTime);
            //증가된 알파값으로 블록 색 재설정
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            Mat.color = objectColor;

            yield return null;
        }
    }
}
