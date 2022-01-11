using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace XReal.XTown.Yacht
{
    public class PickedSlot : MonoBehaviour
    {
        public int slotIndex = 0;
        public bool occupied = false;
        public DiceScript dice = null;



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PutDiceInSlot(int diceIndex)
        {
            occupied = true;
            dice = DiceManager.dices[diceIndex];
            dice.OnPicked(transform);
        }

        public void TakeOutDice()
        {
            occupied = false;
            dice.OnTakeOut();
            dice = null;
        }
    }
}

