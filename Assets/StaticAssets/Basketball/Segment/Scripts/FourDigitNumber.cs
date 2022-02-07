using System.Linq;
using UnityEngine;
using static System.Linq.Enumerable;

public class FourDigitNumber : MonoBehaviour {
    private int maxDigits = 4;
    private SevenSegmentDisplay[] displays;

    private void Awake() {
        displays = GetComponentsInChildren<SevenSegmentDisplay>();
        GameManager.OnGoal += UpdateScoreDisplay;
        SetNumber(0);
    }

    private void SetNumber(int digits) {
        foreach (var i in Range(0, displays.Length)) {
            var display = displays[i];
            if (i > 0 && digits == 0)
                display.TurnOff();  // Suppress leading zeroes
            else {
                var rightmostDigit = digits % 10;
                display.SetDigit(rightmostDigit);
                digits /= 10;
            }
        }
    }

    private void UpdateScoreDisplay()
    {
        SetNumber(GameManager.score);
    }
}
