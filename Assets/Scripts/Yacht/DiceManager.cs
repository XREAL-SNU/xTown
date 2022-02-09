using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace XReal.XTown.Yacht
{
    public class DiceManager : MonoBehaviour
    {
        public static DiceScript[] dices;
        public UnityEvent onRollingFinish;

        // Start is called before the first frame update
        void Awake()
        {
            dices = transform.GetComponentsInChildren<DiceScript>();
            int diceIndex = 0;
            foreach (var dice in dices)
            {
                dice.diceIndex = diceIndex;
                diceIndex += 1;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnInitialze()
        {
            List<DiceInfo> diceInfoList = DiceScript.diceInfoList;

            foreach (DiceInfo diceInfo in diceInfoList)
            {
                if (GameManager.currentGameState == GameState.ready)
                {
                    diceInfo.diceNumber = 0;
                    diceInfo.rolling = false;
                    diceInfo.keeping = false;
                    diceInfo.sortedIndex = 0;
                }
                else if (GameManager.currentGameState == GameState.shaking)
                {
                    if (diceInfo.keeping == false)
                    {
                        diceInfo.diceNumber = 0;
                    }
                }
            }
        }

        public void OnReadyStart()
        {
            foreach (var dice in dices)
            {
                if (dice.diceInfo.keeping == false)
                {
                    dice.Ready();
                }
            }
        }


        public void OnRollingStart()
        {
            foreach (var dice in dices)
            {
                if (dice.diceInfo.keeping == false)
                {
                    dice.Roll();
                }
            }
        }


        public void OnRollingFinish()
        {
            var sortedList = DiceScript.diceInfoList.OrderBy(x => x.diceNumber).ToList();

            int i = 0;
            foreach (DiceInfo sortedDiceInfo in sortedList)
            {
                DiceInfo diceInfo = DiceScript.diceInfoList.Where(x => x.diceIndex == sortedDiceInfo.diceIndex).First();
                diceInfo.sortedIndex = i;
                i += 1;
            }

            // keeping이 false인 것들에 대해서만 loop through
            var sortedUnkeptList = sortedList.Where(x => x.keeping == false).ToList();
            StartCoroutine(DiceRollFinish(sortedUnkeptList));
        }

        public void OnFinish()
        {
            var sortedList = DiceScript.diceInfoList.OrderBy(x => x.diceNumber).ToList();

            int i = 0;
            foreach (DiceInfo sortedDiceInfo in sortedList)
            {
                DiceInfo diceInfo = DiceScript.diceInfoList.Where(x => x.diceIndex == sortedDiceInfo.diceIndex).First();
                diceInfo.sortedIndex = i;
                i += 1;
            }

            var sortedUnkeptList = sortedList.Where(x => x.keeping == false).ToList();
            StartCoroutine(TurnFinish(sortedUnkeptList));
        }

        IEnumerator DiceRollFinish(List<DiceInfo> sortedUnkeptList)
        {
            foreach (DiceInfo diceInfo in sortedUnkeptList)
            {
                int i = diceInfo.diceIndex;
                dices[i].OnRollingFinish();
                yield return new WaitForSecondsRealtime(0.05f);
            }
        }

        IEnumerator TurnFinish(List<DiceInfo> sortedUnkeptList)
        {
            foreach (DiceInfo diceInfo in sortedUnkeptList)
            {
                PickedSlotController.instance.PutIntoEmptySlot(diceInfo.diceIndex);
                yield return new WaitForSecondsRealtime(0.05f);
            }
        }
    }
}
