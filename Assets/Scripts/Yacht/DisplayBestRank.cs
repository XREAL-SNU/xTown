using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XReal.XTown.Yacht
{
    public class DisplayBestRank : MonoBehaviour
    {
        Text text;

        // Start is called before the first frame update
        void Start()
        {
            text = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowBestRank()
        {
            text.text = StrategyScript.bestRank;

            StartCoroutine(HideBestRank());
        }

        IEnumerator HideBestRank()
        {
            yield return new WaitForSeconds(2);
            text.text = "";
        }
    }
}

