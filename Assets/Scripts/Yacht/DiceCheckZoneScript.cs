using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XReal.XTown.Yacht
{
    public class DiceCheckZoneScript : MonoBehaviour
    {
        Vector3 diceVelocity;
        DiceScript diceScript;


        void OnTriggerStay(Collider col)
        {
            /* multiplayer testing */
            //check for my turn
            if (GameManager.multiplayMode && (!GameManager.IsMyTurn || GameManager.currentTurn <= 0)) return;

            if (col.gameObject.tag == "Side")
            {
                //diceScript = col.transform.parent.gameObject.GetComponent<DiceScript>();
                diceScript = col.transform.GetComponentInParent<DiceScript>();
                diceVelocity = diceScript.diceVelocity;

                if (diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f)
                {
                    switch (col.gameObject.name)
                    {
                        case "Side1":
                            diceScript.diceInfo.diceNumber = 6;
                            break;
                        case "Side2":
                            diceScript.diceInfo.diceNumber = 5;
                            break;
                        case "Side3":
                            diceScript.diceInfo.diceNumber = 4;
                            break;
                        case "Side4":
                            diceScript.diceInfo.diceNumber = 3;
                            break;
                        case "Side5":
                            diceScript.diceInfo.diceNumber = 2;
                            break;
                        case "Side6":
                            diceScript.diceInfo.diceNumber = 1;
                            break;
                    }
                    Debug.Log("Dice" + diceScript.diceInfo.diceIndex + "stopped with #" + diceScript.diceInfo.diceNumber);
                    diceScript.diceInfo.rolling = false;
                    diceScript.rb.isKinematic = true;
                }
            }

        }
    }
}
