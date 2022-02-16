// GENERATED AUTOMATICALLY FROM 'Assets/Resources/FirstPersonControl.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @FirstPersonControl : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @FirstPersonControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""FirstPersonControl"",
    ""maps"": [
        {
            ""name"": ""FPS"",
            ""id"": ""ce5099a3-6928-4ba9-a244-e96fda9e80b2"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7298b3d6-9017-4699-95a5-f5916a739914"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a0be393c-35b0-4e35-b580-395cd0e70653"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // FPS
        m_FPS = asset.FindActionMap("FPS", throwIfNotFound: true);
        m_FPS_Look = m_FPS.FindAction("Look", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // FPS
    private readonly InputActionMap m_FPS;
    private IFPSActions m_FPSActionsCallbackInterface;
    private readonly InputAction m_FPS_Look;
    public struct FPSActions
    {
        private @FirstPersonControl m_Wrapper;
        public FPSActions(@FirstPersonControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_FPS_Look;
        public InputActionMap Get() { return m_Wrapper.m_FPS; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FPSActions set) { return set.Get(); }
        public void SetCallbacks(IFPSActions instance)
        {
            if (m_Wrapper.m_FPSActionsCallbackInterface != null)
            {
                @Look.started -= m_Wrapper.m_FPSActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_FPSActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_FPSActionsCallbackInterface.OnLook;
            }
            m_Wrapper.m_FPSActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
            }
        }
    }
    public FPSActions @FPS => new FPSActions(this);
    public interface IFPSActions
    {
        void OnLook(InputAction.CallbackContext context);
    }
}
