//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/RadioControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @RadioControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @RadioControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""RadioControls"",
    ""maps"": [
        {
            ""name"": ""Radio"",
            ""id"": ""77640c71-fe09-472d-b8d6-c1117e0b89f8"",
            ""actions"": [
                {
                    ""name"": ""Aiguille"",
                    ""type"": ""Button"",
                    ""id"": ""2504f9f4-82a0-487b-817e-54418ee487b5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""a2eb0fb5-391a-49d9-b376-7aea632b6ab6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""c3fd8099-f7ef-409d-b9ab-1b6f722544b4"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aiguille"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d131921d-366e-4f19-9da8-0f368600f57d"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aiguille"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""ec5e41f3-a9e5-40a5-9d04-8e3e5316033f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aiguille"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""52203bfd-9316-4063-a3d4-a38d3558cbf4"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Radio
        m_Radio = asset.FindActionMap("Radio", throwIfNotFound: true);
        m_Radio_Aiguille = m_Radio.FindAction("Aiguille", throwIfNotFound: true);
        m_Radio_Escape = m_Radio.FindAction("Escape", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Radio
    private readonly InputActionMap m_Radio;
    private IRadioActions m_RadioActionsCallbackInterface;
    private readonly InputAction m_Radio_Aiguille;
    private readonly InputAction m_Radio_Escape;
    public struct RadioActions
    {
        private @RadioControls m_Wrapper;
        public RadioActions(@RadioControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Aiguille => m_Wrapper.m_Radio_Aiguille;
        public InputAction @Escape => m_Wrapper.m_Radio_Escape;
        public InputActionMap Get() { return m_Wrapper.m_Radio; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(RadioActions set) { return set.Get(); }
        public void SetCallbacks(IRadioActions instance)
        {
            if (m_Wrapper.m_RadioActionsCallbackInterface != null)
            {
                @Aiguille.started -= m_Wrapper.m_RadioActionsCallbackInterface.OnAiguille;
                @Aiguille.performed -= m_Wrapper.m_RadioActionsCallbackInterface.OnAiguille;
                @Aiguille.canceled -= m_Wrapper.m_RadioActionsCallbackInterface.OnAiguille;
                @Escape.started -= m_Wrapper.m_RadioActionsCallbackInterface.OnEscape;
                @Escape.performed -= m_Wrapper.m_RadioActionsCallbackInterface.OnEscape;
                @Escape.canceled -= m_Wrapper.m_RadioActionsCallbackInterface.OnEscape;
            }
            m_Wrapper.m_RadioActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Aiguille.started += instance.OnAiguille;
                @Aiguille.performed += instance.OnAiguille;
                @Aiguille.canceled += instance.OnAiguille;
                @Escape.started += instance.OnEscape;
                @Escape.performed += instance.OnEscape;
                @Escape.canceled += instance.OnEscape;
            }
        }
    }
    public RadioActions @Radio => new RadioActions(this);
    public interface IRadioActions
    {
        void OnAiguille(InputAction.CallbackContext context);
        void OnEscape(InputAction.CallbackContext context);
    }
}
