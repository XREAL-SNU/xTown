using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XReal.XTown.Yacht
{
    public class PickedSlotController : MonoBehaviour
    {
        public static PickedSlotController instance;
        public static PickedSlot[] pickedSlots;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (instance != this)
                {
                    Destroy(this.gameObject);
                }
            }

            pickedSlots = transform.GetComponentsInChildren<PickedSlot>();
            int slotIndex = 0;
            foreach (PickedSlot pickedSlot in pickedSlots)
            {
                pickedSlot.slotIndex = slotIndex;
                slotIndex += 1;
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnInitialize()
        {
            foreach (PickedSlot pickedSlot in pickedSlots)
            {
                pickedSlot.occupied = false;
            }
        }


        public void PutIntoEmptySlot(int diceIndex)
        {
            // find empty pickedslots
            int i = FindEmptySlot();

            if (i != -1)
            {
                pickedSlots[i].PutDiceInSlot(diceIndex);
            }
            else
            {
                return;
            }
        }

        public void GetOutOccupiedSlot(int diceIndex)
        {
            foreach (PickedSlot pickedSlot in pickedSlots)
            {
                if (pickedSlot.dice != null)
                {
                    if (pickedSlot.dice.diceIndex == diceIndex)
                    {
                        pickedSlot.TakeOutDice();
                    }
                }
            }

        }

        public int FindEmptySlot()
        {
            foreach (PickedSlot pickedSlot in pickedSlots)
            {
                //check if it is occupied or empty
                if (pickedSlot.occupied == false)
                {
                    return pickedSlot.slotIndex;
                }
            }

            // if there is no empty slot
            return -1;
        }
    }
}

