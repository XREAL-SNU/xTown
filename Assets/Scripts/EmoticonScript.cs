using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoticonScript : MonoBehaviour
{
    private GameObject Player;
    private RectTransform _rectTransform;
    private float _rotSpeed=100f;
    // Start is called before the first frame update
    private void OnEnable() {
        // 플레이어위치를 추적하기 위해 플레이어를 찾아줍니다
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player");
        }
        _rectTransform = GetComponent<RectTransform>();
        transform.localScale = new Vector3(0,0,0);
    }
    void Update()
    {
        if(transform.localScale.x<=0.4f){
            transform.localScale += new Vector3(0.04f,0.04f,0.04f);
        }
        transform.position = Player.transform.position + new Vector3(0,1.5f,0);
        transform.Rotate(new Vector3(0,_rotSpeed * Time.deltaTime,0));
    }
}
