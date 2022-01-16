using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarViewImage : MonoBehaviour
{
    private float _speed = 3f;
    public void Update()
    {
       if(Input.GetMouseButton(0))
        {
           transform.Rotate(0f, -Input.GetAxis("Mouse X") * _speed, 0f, Space.World);
            transform.Rotate(-Input.GetAxis("Mouse Y") * _speed, 0f, 0f);
        }
    }
}
