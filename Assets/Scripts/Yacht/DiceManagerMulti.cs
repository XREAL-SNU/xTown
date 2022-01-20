using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace XReal.XTown.Yacht
{
    public class DiceManagerMulti : DiceManager
    {
        // Dice prefab to instantiate over the network. should be inside Resources folder.
        public DiceScriptMulti DicePrefab;


        public void SpawnDices()
        {
            Debug.Log("DiceManager/SpawnDices");
            DiceScriptMulti dice;
            for (int diceIndex = 0; diceIndex < 5; diceIndex++)
            {
                // dice info list will be initialized inside Instantiated dice's Start method.
                dice = PhotonNetwork.Instantiate(DicePrefab.name, this.transform.position, Quaternion.identity).GetComponent<DiceScriptMulti>();
                /*
                dice.diceIndex = diceIndex;
                dices[diceIndex] = dice;
                */
            }
        }

        public static void RequestDiceOwnership()
        {
            Debug.Log("Requesting ownership of all dices count" + dices.Count);
            
            foreach(DiceScriptMulti dice in dices)
            {
                dice.RequestOwnership();
            }
        }

    }
}
