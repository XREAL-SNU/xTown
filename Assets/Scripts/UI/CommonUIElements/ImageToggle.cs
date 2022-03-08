using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XReal.XTown.UI;

public class ImageToggle : UIBase
{
    bool _isOn = false;
    public virtual bool IsOn
    {
        get => _isOn;
        protected set
        {
            _isOn = value;

            if(OnImage) OnImage.enabled = value;
            if(OffImage) OffImage.enabled = !value;

            if(OnValueChanged != null)
            {
                OnValueChanged.Invoke(value);
            }
        }
    }


    RawImage OffImage;
    RawImage OnImage;

    public enum Images
    {
        OffImage, OnImage
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<RawImage>(typeof(Images));
        OffImage = GetUIComponent<RawImage>((int)Images.OffImage);
        OnImage = GetUIComponent<RawImage>((int)Images.OnImage);

        OffImage.gameObject.BindEvent((PointerEventData data) => IsOn = true);
        OnImage.gameObject.BindEvent((PointerEventData data) => IsOn = false);

        // set initial state of image
        OnImage.enabled = IsOn;
        OffImage.enabled = !IsOn;
    }

    // toggle methods
    public void Toggle(PointerEventData data)
    {
        IsOn = !IsOn;
    }

    public void SetToggleValue(bool value)
    {
        IsOn = value;
    }

    // callbacks
    Action<bool> OnValueChanged = null;
    public void AddListener(Action<bool> action)
    {
        OnValueChanged -= action;
        OnValueChanged += action;
    }

    public void RemoveListener(Action<bool> action)
    {
        OnValueChanged -= action;
    }
}
