using UnityEngine;
using UnityEngine.SceneManagement;

public class HyperspaceSceneSelect : MonoBehaviour
{
    public void LoadSmokeDemo01()
    {
        SceneManager.LoadScene("hyperspace01");
    }
    public void LoadSmokeDemo02()
    {
        SceneManager.LoadScene("hyperspace02");
    }
    public void LoadSmokeDemo03()
    {
        SceneManager.LoadScene("hyperspace03");
    }
}