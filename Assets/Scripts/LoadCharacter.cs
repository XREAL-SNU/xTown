using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] CharacterPrefabs;
    public Transform SpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        GameObject Prefab = CharacterPrefabs[selectedCharacter];
        GameObject Clone = Instantiate(Prefab, SpawnPoint.position, Quaternion.identity);
    }
}
