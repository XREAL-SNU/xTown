using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarViewMenu : MonoBehaviour
{
    public GameObject[] CharacterPrefabs;
    public Transform SpawnPoint;
    private GameObject Player;
    private RoomsCanvases _roomCanvases;
    private GameObject _prefab;
    private float _speed = 10f;
    private bool _dragMode = false;
    public Ray ray;
    public RaycastHit hit;
    
    public void Awake()
    {
        SpawnPoint = GameObject.Find("SpawnPoint").transform;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    }
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
    }

    public void ConvertMode()
    {
        _dragMode = !_dragMode;
    }

    public void ViewAvatar()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        _prefab = CharacterPrefabs[selectedCharacter];
        Player = Instantiate(_prefab, SpawnPoint.position, Quaternion.identity,_roomCanvases.transform);
        Player.transform.Rotate(0,180,0);
    }

    public void Update()
    {
        /*
        if(Physics.Raycast(ray.origin,ray.direction*10, out hit))
        {
            Debug.Log("raycast"+hit.transform.gameObject);
        }*/
        if(_dragMode && Input.GetMouseButton(0))
        {
            //if(Physics.Raycast(ray, out hit))
            //{
            //    Debug.Log(hit.transform.gameObject);
            //    if(hit.transform.gameObject.tag == "UI")
            //    {
                    Player.transform.Rotate(0f, -Input.GetAxis("Mouse X") * _speed, 0f, Space.World);
                    Player.transform.Rotate(-Input.GetAxis("Mouse Y") * _speed, 0f, 0f);
            //    }
            //}
        }
    }

    public void OnNextButton()
    {
        Player.transform.Rotate(0,-90,0);
    }
    public void OnPrevButton()
    {
        Player.transform.Rotate(0,90,0);
    }
}
