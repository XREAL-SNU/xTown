using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoticonShow : MonoBehaviour
{
    private GameObject Player;
    private RectTransform rectTransform;
    // Start is called before the first frame update
    private void OnEnable() {
        // 플레이어위치를 추적하기 위해 플레이어를 찾아줍니다
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player");
        }
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        // Vector3 playerPosition = Camera.main.WorldToScreenPoint( Player.transform.position );
        // GetComponent<RectTransform>().anchoredPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, playerPosition);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(Player.transform.position.x,Player.transform.position.y+1.3f,Player.transform.position.z));
        rectTransform.anchoredPosition = new Vector3(screenPos.x,screenPos.y,Player.transform.position.z);
        // transform.position = Camera.main.WorldToScreenPoint(Player.gameObject.transform.position + new Vector3(0, 0.8f, 0));
        Debug.Log(rectTransform.anchoredPosition);
    }
    private void OnDisable() {
        
    }
}
