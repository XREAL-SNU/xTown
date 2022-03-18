using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XReal.XTown.UI
{
    public class UIScene : UIBase
    {
        //implement UIBase
        public override void Init()
        {
            // scene canvas is not sorted(sorting order default 0). (sorting = false)
            UIManager.UI.ShowCanvas(gameObject, false);
        }
    }
}

