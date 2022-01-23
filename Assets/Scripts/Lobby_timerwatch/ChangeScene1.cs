using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene1 : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeSceneBtn()
    {
            SceneManager.LoadScene("Stopwatch");
    }


}
