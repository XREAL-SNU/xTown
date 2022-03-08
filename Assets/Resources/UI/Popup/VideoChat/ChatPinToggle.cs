using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatPinToggle : ImageToggle
{
    public override bool IsOn
    {
        get => base.IsOn;
        protected set
        {
            var context = GetComponentInParent<VideoChatScreens>();
            // cannot pin more if exceeding quota
            if (value && context.PinnedItemCount == context.MaxPinnedItemCount)
            {
                Debug.LogWarning("ChatPinToggle/ Cannot pin more!");
                return;
            }
            base.IsOn = value;
        }
    }
}
