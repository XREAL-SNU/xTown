using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XReal.XTown.Yacht
{
    public class ScoreTableMulti : ScoreTable
    {
        private static List<Text> scoreTextsOther = new List<Text>();
        private static int[] otherPoints = new int[14];
        public static void InitMultiTable()
        {
            // set all templates inactive
            scoreTemplate = scoreContainer.Find("ScoreTemplate");
            scoreTemplate.gameObject.SetActive(false);
            scoreTemplate = scoreContainer.Find("ScoreTemplateMulti");
            scoreTemplate.gameObject.SetActive(false);


            float templateHeight = 24f;
            for (int i = 0; i < 14; i++)
            {
                Transform scoreTransform = Instantiate(scoreTemplate, scoreContainer);
                RectTransform scoreRectTransform = scoreTransform.GetComponent<RectTransform>();
                scoreRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
                scoreTransform.gameObject.SetActive(true);

                scoreTransform.Find("CategoryText").GetComponent<Text>().text = StrategyScript.strategiesOrder[i];
                Text scoreText = scoreTransform.Find("ScoreText").GetComponent<Text>();
                scoreTexts.Add(scoreText);
                Text scoreText2 = scoreTransform.Find("ScoreTextOther").GetComponent<Text>();
                scoreTextsOther.Add(scoreText2);

            }
        }
        public override void UpdateScoreTable()
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
        public static int UpdateOtherScoreTable(int move, int points)
        {
            int bonusSum = 0;
            otherPoints[move] = points;
            otherPoints[13] = 0;
            for (int i = 0; i < 6; i++)
            {
                bonusSum += otherPoints[i];
                otherPoints[13] += otherPoints[i];
                scoreTextsOther[i].color = Color.black;
                scoreTextsOther[i].text = otherPoints[i].ToString();

            }
            if(bonusSum>62)
            {
                otherPoints[6] = 35;
            }
            for(int i = 6; i<13; i++)
            {
                otherPoints[13] += otherPoints[i];
                scoreTextsOther[i].color = Color.black;
                scoreTextsOther[i].text = otherPoints[i].ToString();
            }
            scoreTextsOther[13].color = Color.black;
            scoreTextsOther[13].text = otherPoints[13].ToString();
            return otherPoints[13];
        }     

    }
}

