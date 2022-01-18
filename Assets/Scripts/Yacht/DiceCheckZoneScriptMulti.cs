using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XReal.XTown.Yacht
{
    public class DiceCheckZoneScriptMulti : DiceCheckZoneScript
    {
        private DiceScriptMulti diceScriptMulti;


        protected override void OnTriggerStay(Collider col)
        {
            if (!NetworkManager.Instance.networked)
            {
                base.OnTriggerStay(col);
                return;
            }
            if (NetworkManager.Instance.MeDone) return;
            if (col.gameObject.tag == "Side")
            {
                diceScriptMulti = col.transform.parent.gameObject.GetComponent<DiceScriptMulti>();
                diceVelocity = diceScriptMulti.diceVelocity;

                if (diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f)
                {
                    switch (col.gameObject.name)
                    {
                        case "Side1":
                            diceScriptMulti.diceInfo.diceNumber = 6;
                            break;
                        case "Side2":
                            diceScriptMulti.diceInfo.diceNumber = 5;
                            break;
                        case "Side3":
                            diceScriptMulti.diceInfo.diceNumber = 4;
                            break;
                        case "Side4":
                            diceScriptMulti.diceInfo.diceNumber = 3;
                            break;
                        case "Side5":
                            diceScriptMulti.diceInfo.diceNumber = 2;
                            break;
                        case "Side6":
                            diceScriptMulti.diceInfo.diceNumber = 1;
                            break;
                    }

                    diceScriptMulti.diceInfo.rolling = false;
                    diceScriptMulti.rb.isKinematic = true;
                }
            }

        }
    }
}
