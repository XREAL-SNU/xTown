using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace XReal.XTown.Yacht
{
    public class DiceManagerMulti : DiceManager
    {


        // Start is called before the first frame update
        protected override void Start()
        {

            if (!NetworkManager.Instance.networked)
            {
                base.Start();
                return;
            }

            
            int diceIndex = 0;
            foreach (var dice in dices)
            {
                dice.diceIndex = diceIndex;
                diceIndex += 1;
                dice.InitDice();
            }
        }

        public static void BeginSyncDices()
        {
            foreach (DiceScriptMulti dice in dices)
            {
                dice.BeginSyncDice();
            }
        }
    }
}
