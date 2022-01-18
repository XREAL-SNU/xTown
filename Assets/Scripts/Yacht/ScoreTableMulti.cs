using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XReal.XTown.Yacht
{
    public class ScoreTableMulti : ScoreTable
    {

        private static List<Text> scoreTextsOther = new List<Text>();


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

                scoreTransform.Find("ScoreBackground/CategoryText").GetComponent<Text>().text = StrategyScript.strategiesOrder[i];
                Text scoreText = scoreTransform.Find("ScoreText").GetComponent<Text>();
                scoreTexts.Add(scoreText);
                scoreText = scoreTransform.Find("ScoreTextOther").GetComponent<Text>();
                scoreTextsOther.Add(scoreText);

            }
        }
       

    }
}

