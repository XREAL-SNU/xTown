using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] Characters;
    public int selectedCharacter = 0;

    public void NextCharacter()
    {
        Characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % Characters.Length;
        Characters[selectedCharacter].SetActive(true);
        Debug.Log("Click Next");
    }

    public void PreviousCharacter()
    {
        Characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += Characters.Length;
        }
        Characters[selectedCharacter].SetActive(true);
        Debug.Log("Click Previous");
    }

    public void Enter()
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        SceneManager.LoadScene("MainRoom", LoadSceneMode.Single);
    }
}
