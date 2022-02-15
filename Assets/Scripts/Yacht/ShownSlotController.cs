using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XReal.XTown.Yacht
{
    // 미완성된 기능입니다. 무시하셔도 됩니다.
    public class ShownSlotController : MonoBehaviour
    {
        public static ShownSlotController instance;
        public static ShownSlot[] shownSlots;

        void Awake()
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

            shownSlots = transform.GetComponentsInChildren<ShownSlot>();
            int slotIndex = 0;
            foreach (ShownSlot shownSlot in shownSlots)
            {
                shownSlot.slotIndex = slotIndex;
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
    }
}

