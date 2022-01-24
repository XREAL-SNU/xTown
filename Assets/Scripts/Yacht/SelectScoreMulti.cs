using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace XReal.XTown.Yacht
{
    public class SelectScoreMulti : SelectScore
    {
        public Text categoryText;
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (NetworkManager.Instance.MeDone) return;
            if (GameManager.currentGameState == GameState.selecting)
            {
                GameObject go = eventData.pointerCurrentRaycast.gameObject;
                int done = StrategyScript.strategies[categoryText.text]["done"];
                if (done != 1)
                {
                    StrategyScript.strategies[categoryText.text]["done"] = 1;
                    NetworkManager.Instance.SendStrategySelected(
                        StrategyScript.strategies[categoryText.text]["order"],
                        StrategyScript.strategies[categoryText.text]["score"]);
                    GameManagerMulti.MyTotalPoints = StrategyScript.strategies["Total"]["score"];
                    GameManager.SetGameState(GameState.initializing);
                    GameManagerMulti.TurnFinish();
                }
            }
        }
    }
}

