using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XReal.XTown.Yacht
{
    public class StrategyScript : MonoBehaviour
    {
        public static StrategyScript instance;
        public static Dictionary<string, Dictionary<string, int>> strategies = new Dictionary<string, Dictionary<string, int>>();
        public static string[] strategiesOrder = { "1s", "2s", "3s", "4s", "5s", "6s", "Bonus", "Choice", "4-of-a-kind", "Full House", "S. Straight", "L. Straight", "Yacht", "Total" };
        public static string bestRank;

        private int[] diceNumberArray = new int[5];
        private int[] uniqueNumberArray;

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

            foreach (string strategyName in strategiesOrder)
            {
                Dictionary<string, int> strategy_info = new Dictionary<string, int>();
                strategy_info.Add("score", 0);
                strategy_info.Add("done", 0);
                strategies.Add(strategyName, strategy_info);
            }
            strategies["Bonus"]["done"] = 1;
            strategies["Total"]["done"] = 1;
        }
        public void Calculate()
        {
            diceNumberArray = DiceScript.diceInfoList.OrderBy(x => x.diceNumber).Select(x => x.diceNumber).ToArray();
            uniqueNumberArray = diceNumberArray.Distinct().ToArray();
            Debug.Log(uniqueNumberArray);

            string b = "";
             foreach (var a in uniqueNumberArray)
            {
                b += a.ToString();
            }
            Debug.Log(b);

            int upperScore = 0;
            for (int i = 1; i < 7; i++)
            {
                if (strategies[string.Format("{0}s", i)]["done"] == 1)
                {
                    upperScore += strategies[string.Format("{0}s", i)]["score"];
                    continue;
                }

                int score = SumOfSingle(i);
                strategies[string.Format("{0}s", i)]["score"] = score;
            }

            if (upperScore >= 63)
            {
                strategies["Bonus"]["score"] = 35;
            }

            if (strategies["Choice"]["done"] == 0)
            {
                strategies["Choice"]["score"] = diceNumberArray.Sum();
            }
            if (strategies["4-of-a-kind"]["done"] == 0)
            {
                strategies["4-of-a-kind"]["score"] = OfAKind(4);
            }
            if (strategies["Full House"]["done"] == 0)
            {
                strategies["Full House"]["score"] = FullHouse();
            }
            if (strategies["S. Straight"]["done"] == 0)
            {
                strategies["S. Straight"]["score"] = SmallStraight();
            }
            if (strategies["L. Straight"]["done"] == 0)
            {
                strategies["L. Straight"]["score"] = LargeStraight();
            }
            if (strategies["Yacht"]["done"] == 0)
            {
                strategies["Yacht"]["score"] = OfAKind(5);
            }

            strategies["Total"]["score"] = 0;
            for (int i = 0; i < 13; i++)
            {
                if (strategies[strategiesOrder[i]]["done"] == 1)
                {
                    strategies["Total"]["score"] += strategies[strategiesOrder[i]]["score"];
                }
            }

            bestRank = FindBestRank();
        }

        private string FindBestRank()
        {
            string bestRank = "";
            for (int i = 8; i < 13; i++)
            {
                if (strategies[strategiesOrder[i]]["done"] == 0)
                {
                    if (strategies[strategiesOrder[i]]["score"] > 0)
                    {
                        bestRank = strategiesOrder[i];
                    }
                }
            }

            return bestRank;
        }

        public int HighestRepeated(int minRepeats)
        {
            var groups = diceNumberArray.GroupBy(x => x);
            var largest = groups.OrderByDescending(x => x.Count()).First();

            if (largest.Count() >= minRepeats)
            {
                return largest.Key;
            }
            else
            {
                return 0;
            }

        }

        public int OfAKind(int n)
        {
            int hr = HighestRepeated(n);

            if (hr == 0)
            {
                return 0;
            }

            if (n == 5)
            {
                return 50;
            }

            return diceNumberArray.Sum();
        }

        public int SumOfSingle(int number)
        {
            return diceNumberArray.Where(x => x == number).Sum();
        }

        public int FullHouse()
        {
            int hr = HighestRepeated(5);
            if (hr > 0)
            {
                return 5 * hr;
            }

            hr = HighestRepeated(3);
            if (hr > 0)
            {
                var rests = diceNumberArray.Where(x => x != hr).ToArray();
                if (rests.Length == 2 && rests.Distinct().Count() == 1)
                {
                    return diceNumberArray.Sum();
                }
            }

            return 0;
        }

        public int SmallStraight()
        {
            
            if (LargeStraight() > 0)
            {
                return 15;
            }

            int[] ss1 = new int[] { 1, 2, 3, 4 };
            int[] ss2 = new int[] { 2, 3, 4, 5 };
            int[] ss3 = new int[] { 3, 4, 5, 6 };

            bool isSubset = (!ss1.Except(uniqueNumberArray).Any() || !ss2.Except(uniqueNumberArray).Any() || !ss3.Except(uniqueNumberArray).Any());
            if (isSubset)
            {
                return 15;
            }

            return 0;
        }

        public int LargeStraight()
        {
            int[] ls1 = new int[] { 1, 2, 3, 4, 5 };
            int[] ls2 = new int[] { 2, 3, 4, 5, 6 };
            if (uniqueNumberArray.SequenceEqual(ls1) || uniqueNumberArray.SequenceEqual(ls2))
            {
                return 30;
            }

            return 0;
        }
    }
}

