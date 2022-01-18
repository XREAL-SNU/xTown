using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneBack : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeSceneBtn()
    {
            SceneManager.LoadScene("Lobby");
    }


}
