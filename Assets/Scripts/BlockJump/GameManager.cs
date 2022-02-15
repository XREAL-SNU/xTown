using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    static public Vector3 savePoint; //추락시 돌아오는 지점
    public Text pointText;

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.position = savePoint; //추락 시 savePoint로 이동
            other.GetComponent<Rigidbody>().velocity = Vector3.zero; //이동한 위치에서 정지
        }
    }

    private void EndGame()
    {
        pointText.text = pointText.ToString();
    }
}
