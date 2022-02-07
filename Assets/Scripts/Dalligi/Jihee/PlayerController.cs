using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public float speed;
    public float jumpforce;

    private int Coincount = 0;
    private int Scorecount = 0;
    public Text CoinCountText;
    public Text ScoreCountText;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        CoinSetCount();
        Score();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("up"))
        {
            Debug.Log("up");
            _rigidbody.AddForce(speed, 0f, 0f);
        }
        if (Input.GetKey("down"))
        {
            Debug.Log("down");
            _rigidbody.AddForce(-speed, 0f, 0f);
        }
        if (Input.GetKey("right"))
        {
            Debug.Log("right");
            _rigidbody.AddForce(0f, 0f, -speed);
        }
        if (Input.GetKey("left"))
        {
            Debug.Log("left");
            _rigidbody.AddForce(0f, 0f, speed);
        }
        if (Input.GetKey("space"))
        {
            Debug.Log("jump");
            _rigidbody.AddForce(0f, jumpforce, 0f);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Coin")){
            other.gameObject.SetActive(false);
            Coincount = Coincount + 1;
            CoinSetCount();
        }

        if (other.gameObject.CompareTag("BlockMove")){
            //other.gameObject.SetActive(true);
            Scorecount = Scorecount + 20;
            Score();
        }

        if (other.gameObject.CompareTag("BlockResize")){
            //other.gameObject.SetActive(true);
            Scorecount = Scorecount + 20;
            Score();
        }
        if (other.gameObject.CompareTag("BlockStatic")){
           // other.gameObject.SetActive(true);
            Scorecount = Scorecount + 40;
            Score();
        }
        if (other.gameObject.CompareTag("BlockTrans")){
            //other.gameObject.SetActive(true);
            Scorecount = Scorecount + 10;
            Score();
        }
    }
    private void CoinSetCount()
    {
        CoinCountText.text = "CoinCount :"+ Coincount.ToString();
    }

    
    private void Score()
    {
        ScoreCountText.text = "Score :"+ Scorecount.ToString();
    }
}
