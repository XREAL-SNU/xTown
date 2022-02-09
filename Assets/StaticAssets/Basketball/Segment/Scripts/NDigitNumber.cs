using System.Linq;
using UnityEngine;
using static System.Linq.Enumerable;

namespace XReal.XTown.Basketball
{
    public class NDigitNumber : MonoBehaviour
    {
        private SevenSegmentDisplay[] displays;

        private void Awake()
        {
            displays = GetComponentsInChildren<SevenSegmentDisplay>();
            SetNumber(0);
        }

        public void SetNumber(int digits)
        {
            foreach (var i in Range(0, displays.Length))
            {
                var display = displays[i];
                if (i > 0 && digits == 0)
                    display.TurnOff();  // Suppress leading zeroes
                else
                {
                    var rightmostDigit = digits % 10;
                    display.SetDigit(rightmostDigit);
                    digits /= 10;
                }
            }
        }
    }
}
