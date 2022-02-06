using System.Collections;
using UnityEngine;

public class NumberGenerator : MonoBehaviour {
    public MultiDigitNumber multiDigitNumber;

    void Start() {
        StartCoroutine(nameof(Generate));
    }

    private IEnumerator Generate() {
        while (true) {
            multiDigitNumber.SetNumber(Random.Range(0, 99999));
            yield return new WaitForSeconds(1);
        }
    }
}
