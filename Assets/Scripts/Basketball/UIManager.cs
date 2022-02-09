using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _playButtonGO;
    [SerializeField]
    private TMP_Text _roundText;
    [SerializeField]
    private TMP_Text _instructionText;

    private Button _playButton;

    // Start is called before the first frame update
    void Start()
    {
        _playButton = _playButtonGO.GetComponent<Button>();
        _playButton.onClick.AddListener(OnClick_Play);

        _roundText.text = "";
        _instructionText.text = "";

        GameManager.OnGameStateChanged += EnablePlayButton;
        GameManager.OnGameStateChanged += UpdateInstructionText;
        GameManager.OnGameStateChanged += UpdateRoundText;
    }

    private void OnClick_Play()
    {
        GameManager.SetGameState(GameManager.GameState.RoundWaiting);
        _playButtonGO.SetActive(false);
        GameManager.OnGameStateChanged();
    }

    private void EnablePlayButton()
    {
        if (GameManager.CurrentGameState == GameManager.GameState.GameReady)
        {
            _playButtonGO.SetActive(true);
        }
    }

    private void UpdateInstructionText()
    {
        switch (GameManager.CurrentGameState)
        {
            case GameManager.GameState.GameReady:
                {
                    _instructionText.text = "Press play button to start game";
                    break;
                }
            case GameManager.GameState.RoundWaiting:
                {
                    _instructionText.text = "Get Ready for Round " + GameManager.round.ToString();
                    break;
                }
            case GameManager.GameState.RoundOngoing:
                {
                    _instructionText.text = "";
                    break;
                }
            case GameManager.GameState.RoundFinished:
                {
                    _instructionText.text = "Round " + GameManager.round.ToString() + " Finish!";
                    break;
                }
            case GameManager.GameState.GameFinished:
                {
                    _instructionText.text = "Score : " + GameManager.totalScore.ToString();
                    break;
                }
        }
    }

    private void UpdateRoundText()
    {
        switch (GameManager.CurrentGameState)
        {
            case GameManager.GameState.GameReady:
                {
                    _instructionText.text = "";
                    break;
                }
            case GameManager.GameState.RoundWaiting:
                {
                    _instructionText.text = "Round " + GameManager.round.ToString();
                    break;
                }
        }
    }
}
