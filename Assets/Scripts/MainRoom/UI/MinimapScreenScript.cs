using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class MinimapScreenScript : MonoBehaviour
{
    public GameObject WorldField;   //우선 현재는 월드 필드를 임의의 큐브로 잡고, 그 Collider의 width와 height를 받아 reactive하게 작동되도록 하려고 함. 나중에 월드 전체 Field를 이 자리에 넣으면 됨.
    //public GameObject WorldImage; 나중에는 월드 전체 이미지를 캡쳐하여 이미지 위에 플레이어 dot만 좌표를 받아 움직이도록 설정하는게 효율적일 것으로 보임. 이건 미니맵 스크린의 image를 바꾸면 될 듯.
    private GameObject Player;      //플레이어 오브젝트(multi로 간다면 여러개를 추가해야할 듯)
    public GameObject PlayerDot;    //플레이어의 위치를 표시할 점
    
    public static string PlaceName; //현재 위치 표현하는 text

    public TextMeshProUGUI _userLocationLocal;
    public TextMeshProUGUI _userLocationWorld;
    private Vector3 _playerPos;
    private Vector3 _dotPos;
    private float _worldWidth;
    private float _worldHeight;
    RectTransform rectTran;
    // Start is called before the first frame update
    void Start()
    {
        //월드 필드 값 받기 (lossyScale: 절대적인 scale(ReadOnly) <-> localScale)
        _worldWidth = WorldField.transform.lossyScale.x;
        _worldHeight = WorldField.transform.lossyScale.z;
        
        Player = GameObject.FindWithTag("Player");
        Debug.Log(WorldField.name);
    }

    // Update is called once per frame
    void Update()
    {
        
        //World Minimap
        if(gameObject.name == "WorldMinimapScreen")
        {
            //플레이어 좌표 불러오기
            _playerPos=Player.transform.position;   //매 프레임마다 받아오면 computing이 너무 많아져서 coroutine을 걸어야할듯.(0.1~0.2초에 한번씩 불러오도록)
            
            //플레이어 움직임에 따라 미니맵의 점이 움직이도록 설정. why 420?? -> 월드 미니맵 스크린의 scale
            PlayerDot.GetComponent<RectTransform>().anchoredPosition = new Vector3(4.2f/_worldWidth*_playerPos.x,4.2f/_worldHeight*_playerPos.z,0);
            
        }
        //Local Minimap
        else
        {
            
        }

        PlaceName = WorldField.name;

        //지금 있는 곳의 이름 받아오기
        _userLocationLocal.text = PlaceName;
        _userLocationWorld.text = PlaceName;

        //Debug.Log(WorldField.name);
    }
}
