using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XReal.XTown.UI
{
    public class UIPopup : UIBase
    {
        public override void Init()
        {
            UIManager.UI.ShowCanvas(gameObject, true);
        }

        public virtual void ClosePopup()
        {
            UIManager.UI.ClosePopupUI(this);
        }
    }
}