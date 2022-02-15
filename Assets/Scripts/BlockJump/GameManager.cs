using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    static public Vector3 savePoint; //추락시 돌아오는 지점
    private CharacterController CharController;
    private GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");    
        CharController = Player.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CharController.enabled = false;
            CharController.transform.position = savePoint; //추락 시 savePoint로 이동
            CharController.enabled = true;
        }
    }


}
