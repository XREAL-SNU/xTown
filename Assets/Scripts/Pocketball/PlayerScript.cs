using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JK
{
    public class PlayerScript : MonoBehaviour
    {
        public static Vector3 playerPosition = Vector3.zero;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            playerPosition = transform.position;
        }
    }
}