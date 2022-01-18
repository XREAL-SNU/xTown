using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace XReal.XTown.Yacht
{
    public class SelectScoreMulti : SelectScore
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (NetworkManager.Instance.MeDone) return;
            if (GameManager.currentGameState == GameState.selecting)
            {
                GameObject go = eventData.pointerCurrentRaycast.gameObject;
                Text categoryText = go.transform.Find("CategoryText").GetComponent<Text>();


                int done = StrategyScript.strategies[categoryText.text]["done"];

                if (done != 1)
                {
                    StrategyScript.strategies[categoryText.text]["done"] = 1;
                    /* SOOOO HARD TO FIND!!!
                    GameManager.SetGameState(GameState.initializing);
                    */
                    GameManagerMulti.TurnFinish();
                }
            }
        }
    }
}

