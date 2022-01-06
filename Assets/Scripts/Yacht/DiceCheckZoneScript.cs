using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XReal.XTown.Yacht
{
    public class DiceCheckZoneScript : MonoBehaviour
    {
        Vector3 diceVelocity;
        DiceScript diceScript;

        // Update is called once per frame
        void FixedUpdate()
        {
        }

        void OnTriggerStay(Collider col)
        {
            if (col.gameObject.tag == "Side")
            {
                diceScript = col.transform.parent.gameObject.GetComponent<DiceScript>();
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

                    diceScript.diceInfo.rolling = false;
                    diceScript.rb.isKinematic = true;
                }
            }

        }
    }
}
