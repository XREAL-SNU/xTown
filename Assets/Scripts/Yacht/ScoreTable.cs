using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XReal.XTown.Yacht
{
    public class ScoreTable : MonoBehaviour
    {
        protected static Transform scoreContainer;
        protected static Transform scoreTemplate;

        protected Dictionary<string, Dictionary<string, int>> strategies = StrategyScript.strategies;
        protected static List<Text> scoreTexts = new List<Text>();

        protected virtual void Awake()
        {
            scoreContainer = transform.Find("ScoreContainer");

        }
        public static void InitSingleTable()
        {
            scoreTemplate = scoreContainer.Find("ScoreTemplateMulti");
            scoreTemplate.gameObject.SetActive(false);
            scoreTemplate = scoreContainer.Find("ScoreTemplate");
            scoreTemplate.gameObject.SetActive(false);

            float templateHeight = 24f;
            for (int i = 0; i < 14; i++)
            {
                Transform scoreTransform = Instantiate(scoreTemplate, scoreContainer);
                RectTransform scoreRectTransform = scoreTransform.GetComponent<RectTransform>();
                scoreRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
                scoreTransform.gameObject.SetActive(true);

                scoreTransform.Find("ScoreBackground/CategoryText").GetComponent<Text>().text = StrategyScript.strategiesOrder[i];
                Text scoreText = scoreTransform.Find("ScoreText").GetComponent<Text>();
                scoreTexts.Add(scoreText);

            }
        }

        public virtual void UpdateScoreTable()
        {
            GameState _gameState = GameManager.currentGameState;
            Dictionary<string, int> _strategy = null;

            for (int i = 0; i < 14; i++)
            {
                _strategy = strategies[StrategyScript.strategiesOrder[i]];

                if ((_gameState != GameState.selecting) && _strategy["done"] == 0)
                {
                    scoreTexts[i].text = "";
                }
                else if (_gameState == GameState.selecting)
                {
                    scoreTexts[i].text = _strategy["score"].ToString();
                }

                if (_strategy["done"] == 1)
                {
                    scoreTexts[i].color = Color.black;
                    scoreTexts[i].text = _strategy["score"].ToString();
                }
            }
        }
    }
}

