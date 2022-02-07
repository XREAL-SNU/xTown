using System.Linq;
using UnityEngine;
using static System.Linq.Enumerable;

public class MultiDigitNumber : MonoBehaviour {
    public Transform segmentDisplayPrefab;
    private int maxDigits = 2;
    private SevenSegmentDisplay[] displays;

    private void Awake() {
        displays = Range(0, maxDigits).Select(num =>
            Instantiate(segmentDisplayPrefab, transform.position - Vector3.right * HorizontalSpacing() * num,
                Quaternion.identity, transform).GetComponent<SevenSegmentDisplay>()).ToArray();
        SetNumber(0);
    }

    private float HorizontalSpacing() {
        var mesh = segmentDisplayPrefab.GetComponent<MeshFilter>().sharedMesh;
        return mesh.bounds.size.x * 1.05f * segmentDisplayPrefab.transform.localScale.x;
    }

    public void SetNumber(int digits) {
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
}
