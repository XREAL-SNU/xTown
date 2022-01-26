using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarViewMenu : MonoBehaviour
{
    public GameObject[] CharacterPrefabs;
    private GameObject Player;
    private RoomsCanvases _roomCanvases;
    private GameObject _prefab;
    private float _speed = 10f;
    private bool _dragMode = false;
    private Vector3 _spawnPoint = new Vector3(-40,-15,90);

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
        Player = Instantiate(_prefab, _spawnPoint, Quaternion.identity,_roomCanvases.transform);
        Player.transform.Rotate(0,180,0);
    }
    public void DestroyAvatar()
    {
        Destroy(Player);
    }

    public void Update()
    {
        if(_dragMode && Input.GetMouseButton(0))
        {
            Player.transform.Rotate(0f, -Input.GetAxis("Mouse X") * _speed, 0f, Space.World);
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
