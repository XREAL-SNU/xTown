using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XReal.XTown.Basketball
{
    public class LeftTimeDisplay : MonoBehaviour
    {
        private NDigitNumber _leftTimeDisplay;

        private void Awake()
        {
            _leftTimeDisplay = GetComponent<NDigitNumber>();
            TimerController.OnTick += OnTickHandler;
        }

        private void OnTickHandler(object sender, TimerController.OnTickEventArgs e)
        {
            _leftTimeDisplay.SetNumber(e.second);
        }
    }
}