using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMover : MonoBehaviour

{
    private void Awake() {
        Debug.Log("Load Scene");
        SceneManager.LoadScene("_Avatar_Master", LoadSceneMode.Additive);  
    }
}