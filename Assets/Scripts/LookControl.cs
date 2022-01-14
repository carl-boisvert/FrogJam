//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/LookControl.inputactions
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

public partial class @LookControl : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @LookControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""LookControl"",
    ""maps"": [
        {
            ""name"": ""Mouse"",
            ""id"": ""d84a979c-ac1e-4e42-9e8b-7ebc34c078a0"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""fb460df4-6037-4e91-a1ea-554287330d8f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""05fe3d9b-072b-4d57-a4f1-5805004424f3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Throw"",
                    ""type"": ""Button"",
                    ""id"": ""f140e723-bb85-47bc-a26d-45dab982f62c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MainMenu"",
                    ""type"": ""Button"",
                    ""id"": ""9eafb8cf-0676-4dca-9b5e-0e61b93c59db"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0997387d-f847-44e0-89c3-01e21ea2b689"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false,invertY=false)"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""98e8081f-6c87-4227-9ddb-b80404b3c83a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b7cd8a5-f299-4253-9c27-81ffb8d778e4"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a850e7c5-0039-46ef-91a5-9c393bb33f4e"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MainMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Mouse
        m_Mouse = asset.FindActionMap("Mouse", throwIfNotFound: true);
        m_Mouse_Look = m_Mouse.FindAction("Look", throwIfNotFound: true);
        m_Mouse_Interact = m_Mouse.FindAction("Interact", throwIfNotFound: true);
        m_Mouse_Throw = m_Mouse.FindAction("Throw", throwIfNotFound: true);
        m_Mouse_MainMenu = m_Mouse.FindAction("MainMenu", throwIfNotFound: true);
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

    // Mouse
    private readonly InputActionMap m_Mouse;
    private IMouseActions m_MouseActionsCallbackInterface;
    private readonly InputAction m_Mouse_Look;
    private readonly InputAction m_Mouse_Interact;
    private readonly InputAction m_Mouse_Throw;
    private readonly InputAction m_Mouse_MainMenu;
    public struct MouseActions
    {
        private @LookControl m_Wrapper;
        public MouseActions(@LookControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_Mouse_Look;
        public InputAction @Interact => m_Wrapper.m_Mouse_Interact;
        public InputAction @Throw => m_Wrapper.m_Mouse_Throw;
        public InputAction @MainMenu => m_Wrapper.m_Mouse_MainMenu;
        public InputActionMap Get() { return m_Wrapper.m_Mouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseActions set) { return set.Get(); }
        public void SetCallbacks(IMouseActions instance)
        {
            if (m_Wrapper.m_MouseActionsCallbackInterface != null)
            {
                @Look.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnLook;
                @Interact.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnInteract;
                @Throw.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnThrow;
                @Throw.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnThrow;
                @Throw.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnThrow;
                @MainMenu.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnMainMenu;
                @MainMenu.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnMainMenu;
                @MainMenu.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnMainMenu;
            }
            m_Wrapper.m_MouseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Throw.started += instance.OnThrow;
                @Throw.performed += instance.OnThrow;
                @Throw.canceled += instance.OnThrow;
                @MainMenu.started += instance.OnMainMenu;
                @MainMenu.performed += instance.OnMainMenu;
                @MainMenu.canceled += instance.OnMainMenu;
            }
        }
    }
    public MouseActions @Mouse => new MouseActions(this);
    public interface IMouseActions
    {
        void OnLook(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnThrow(InputAction.CallbackContext context);
        void OnMainMenu(InputAction.CallbackContext context);
    }
}
