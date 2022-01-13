//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/BookControl.inputactions
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

public partial class @BookControl : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @BookControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""BookControl"",
    ""maps"": [
        {
            ""name"": ""Book"",
            ""id"": ""0be4a00a-9fb1-4b02-881a-22db017dfdb8"",
            ""actions"": [
                {
                    ""name"": ""Exit"",
                    ""type"": ""Button"",
                    ""id"": ""bfedf89e-0886-4edc-84bd-e5eecf6c1117"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b205a4d8-1ff9-4e9e-8155-d102a4a734ef"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Exit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Book
        m_Book = asset.FindActionMap("Book", throwIfNotFound: true);
        m_Book_Exit = m_Book.FindAction("Exit", throwIfNotFound: true);
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

    // Book
    private readonly InputActionMap m_Book;
    private IBookActions m_BookActionsCallbackInterface;
    private readonly InputAction m_Book_Exit;
    public struct BookActions
    {
        private @BookControl m_Wrapper;
        public BookActions(@BookControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Exit => m_Wrapper.m_Book_Exit;
        public InputActionMap Get() { return m_Wrapper.m_Book; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BookActions set) { return set.Get(); }
        public void SetCallbacks(IBookActions instance)
        {
            if (m_Wrapper.m_BookActionsCallbackInterface != null)
            {
                @Exit.started -= m_Wrapper.m_BookActionsCallbackInterface.OnExit;
                @Exit.performed -= m_Wrapper.m_BookActionsCallbackInterface.OnExit;
                @Exit.canceled -= m_Wrapper.m_BookActionsCallbackInterface.OnExit;
            }
            m_Wrapper.m_BookActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Exit.started += instance.OnExit;
                @Exit.performed += instance.OnExit;
                @Exit.canceled += instance.OnExit;
            }
        }
    }
    public BookActions @Book => new BookActions(this);
    public interface IBookActions
    {
        void OnExit(InputAction.CallbackContext context);
    }
}
