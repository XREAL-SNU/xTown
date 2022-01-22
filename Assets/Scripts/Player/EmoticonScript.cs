using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoticonScript : MonoBehaviour
{
    public GameObject PlayerGo;
    private Transform _player;
    private float _rotSpeed = 100f;

    [Header("display offset")]
    [Tooltip("offset of emoticon from the character")]
    public Vector3 EmoticonOffset;

    // Start is called before the first frame update
    private void OnEnable() {
        // 플레이어위치를 추적하기 위해 플레이어를 찾아줍니다
        if (_player is null)
        {
            _player = PlayerGo.transform.Find("Space_Suit");
        }
        transform.localScale = new Vector3(0, 0, 0);
    }
    void Update()
    {
        if(transform.localScale.x<=0.4f) {
            transform.localScale += new Vector3(0.04f,0.04f,0.04f);
        }
        transform.position = _player.transform.position + EmoticonOffset;
        transform.Rotate(new Vector3(0, _rotSpeed * Time.deltaTime, 0));
    }
}
