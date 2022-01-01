using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] CharacterPrefabs;
    public Transform SpawnPoint;
    public CinemachineFreeLook FreeLookCam;
    private Transform FollowTarget;
    private GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        GameObject Prefab = CharacterPrefabs[selectedCharacter];
        GameObject Clone = Instantiate(Prefab, SpawnPoint.position, Quaternion.identity);
        Player = null;
    }

    void Update()
    {
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player");
            if (Player != null)
            {
                //FollowTarget = Player.transform;
                FollowTarget = GameObject.Find("FollowTarget").transform;
                FreeLookCam.Follow = Player.transform;
                FreeLookCam.LookAt = FollowTarget;
            }
        }
    }
}
