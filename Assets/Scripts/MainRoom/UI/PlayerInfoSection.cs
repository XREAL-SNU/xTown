using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInfoSection : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _nicknameText;
    [SerializeField]
    private Text _teamText;
    [SerializeField]
    private Text _fowText;
    [SerializeField]
    private Text _introText;
    [SerializeField]
    private Text _contactText;

    public void Initialize(string[] playerInfoData)
    {
        _nicknameText.text = playerInfoData[0];
        _teamText.text = playerInfoData[1];
        _fowText.text = playerInfoData[2];
        _introText.text = playerInfoData[3];
        _contactText.text = playerInfoData[4];
    }
}
