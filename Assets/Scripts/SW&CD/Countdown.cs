using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {
    public float timeStart = 60;
    public Text textBox;

	// Use this for initialization
	void Start () {
        textBox.text = timeStart.ToString();
	}

	// Update is called once per frame
	void Update () {
        timeStart -= Time.deltaTime;
        textBox.text = Mathf.Round(timeStart).ToString();
	}
}
