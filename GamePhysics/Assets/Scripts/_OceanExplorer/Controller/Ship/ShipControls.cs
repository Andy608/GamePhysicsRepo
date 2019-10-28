// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/_OceanExplorer/Controller/Ship/ShipControls.inputactions'

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class ShipControls : IInputActionCollection
{
    private InputActionAsset asset;
    public ShipControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ShipControls"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""8b8589fd-336f-4627-8da5-6123bdf769e2"",
            ""actions"": [
                {
                    ""name"": ""UpPressed"",
                    ""type"": ""Value"",
                    ""id"": ""b195a8e9-c4b5-48b9-a89c-55f5bad210e0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""UpReleased"",
                    ""type"": ""Value"",
                    ""id"": ""db7629c6-07e3-467c-b6b7-f67245c73a66"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""LeftPressed"",
                    ""type"": ""Value"",
                    ""id"": ""4628703e-cdd6-4ab6-9a9e-75365bc7814a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""LeftReleased"",
                    ""type"": ""Value"",
                    ""id"": ""cdc80e04-7465-4e5b-a189-8686c6a0f6bc"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""DownReleased"",
                    ""type"": ""Value"",
                    ""id"": ""9b7a036b-221d-4c34-a86e-57a8defb67f9"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""DownPressed"",
                    ""type"": ""Value"",
                    ""id"": ""e90aeda3-6135-4647-9979-dc04f5ccebd2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""RightPressed"",
                    ""type"": ""Value"",
                    ""id"": ""e0e5a043-9fbe-4c5b-a11f-8c14265e04a7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""RightReleased"",
                    ""type"": ""Value"",
                    ""id"": ""c41ea025-fce6-4e29-b872-2ea7c94e5dca"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""RotUpPressed"",
                    ""type"": ""Button"",
                    ""id"": ""7d5a69af-b662-413d-b3fd-adbf8a16ceff"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""RotUpReleased"",
                    ""type"": ""Button"",
                    ""id"": ""429cae70-bd85-46fd-884f-6f2733db8549"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""RotDownPressed"",
                    ""type"": ""Button"",
                    ""id"": ""c883aaec-f4d0-40e4-81ab-fa271e574000"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""RotDownReleased"",
                    ""type"": ""Button"",
                    ""id"": ""c43292dc-237b-4e3a-b66f-a81057bbb964"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""FireTorpedo"",
                    ""type"": ""Button"",
                    ""id"": ""333272b7-bde7-433e-b084-009441788bb7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8212cc53-b316-4c98-82ab-d9c65f3d956b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UpPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a436ac2-eb69-418e-9444-8dabf4590338"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UpPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93233c66-fd52-49b5-a2c3-c99a55d47b96"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""LeftReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""637cdbcd-9bfe-4752-bc2c-47d556b95001"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""LeftReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0fa1ae6c-68ec-476e-a93b-81ac96364eb4"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""LeftPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ca980dea-7acb-4b01-b763-69337d8b8aae"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""LeftPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d454d8fe-e929-483e-ac78-c3fe5ab53296"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""RightPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""190dad95-7aba-4d3a-b345-23d43e40fcba"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""RightPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7416c10c-4666-42a1-897f-49c6c48b0da1"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""UpReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b29472c4-372c-42c0-945a-40483a0dbab2"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""UpReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d1f0b24a-056d-462d-a1e3-749ed0768da1"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""DownReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20f583e4-3a74-4253-8804-410e84cc2c0c"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""DownReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d115d5b-5805-4064-abed-90a37a072942"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""RightReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""57e4f7f3-e5a4-49b2-b0ab-f9c08efaa7d0"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""RightReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""faf317b8-10be-47d3-bd21-e1e0532907a2"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""DownPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd003ec9-c103-410d-a080-cbf8c46b6719"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controls"",
                    ""action"": ""DownPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0fb9845c-aa5f-4d62-8474-5697842226ca"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotUpPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5e1e23ca-2ec6-4392-a2f6-99db66874afa"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotUpReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""da2f0fef-faa8-409e-a5ec-8d81093ef465"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotDownPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7f5be17a-ad99-4415-9607-1f74c8732749"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotDownReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04701142-1917-47a9-8403-d7f57e6c3213"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FireTorpedo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Controls"",
            ""bindingGroup"": ""Controls"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_UpPressed = m_Movement.FindAction("UpPressed", throwIfNotFound: true);
        m_Movement_UpReleased = m_Movement.FindAction("UpReleased", throwIfNotFound: true);
        m_Movement_LeftPressed = m_Movement.FindAction("LeftPressed", throwIfNotFound: true);
        m_Movement_LeftReleased = m_Movement.FindAction("LeftReleased", throwIfNotFound: true);
        m_Movement_DownReleased = m_Movement.FindAction("DownReleased", throwIfNotFound: true);
        m_Movement_DownPressed = m_Movement.FindAction("DownPressed", throwIfNotFound: true);
        m_Movement_RightPressed = m_Movement.FindAction("RightPressed", throwIfNotFound: true);
        m_Movement_RightReleased = m_Movement.FindAction("RightReleased", throwIfNotFound: true);
        m_Movement_RotUpPressed = m_Movement.FindAction("RotUpPressed", throwIfNotFound: true);
        m_Movement_RotUpReleased = m_Movement.FindAction("RotUpReleased", throwIfNotFound: true);
        m_Movement_RotDownPressed = m_Movement.FindAction("RotDownPressed", throwIfNotFound: true);
        m_Movement_RotDownReleased = m_Movement.FindAction("RotDownReleased", throwIfNotFound: true);
        m_Movement_FireTorpedo = m_Movement.FindAction("FireTorpedo", throwIfNotFound: true);
    }

    ~ShipControls()
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

    // Movement
    private readonly InputActionMap m_Movement;
    private IMovementActions m_MovementActionsCallbackInterface;
    private readonly InputAction m_Movement_UpPressed;
    private readonly InputAction m_Movement_UpReleased;
    private readonly InputAction m_Movement_LeftPressed;
    private readonly InputAction m_Movement_LeftReleased;
    private readonly InputAction m_Movement_DownReleased;
    private readonly InputAction m_Movement_DownPressed;
    private readonly InputAction m_Movement_RightPressed;
    private readonly InputAction m_Movement_RightReleased;
    private readonly InputAction m_Movement_RotUpPressed;
    private readonly InputAction m_Movement_RotUpReleased;
    private readonly InputAction m_Movement_RotDownPressed;
    private readonly InputAction m_Movement_RotDownReleased;
    private readonly InputAction m_Movement_FireTorpedo;
    public struct MovementActions
    {
        private ShipControls m_Wrapper;
        public MovementActions(ShipControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @UpPressed => m_Wrapper.m_Movement_UpPressed;
        public InputAction @UpReleased => m_Wrapper.m_Movement_UpReleased;
        public InputAction @LeftPressed => m_Wrapper.m_Movement_LeftPressed;
        public InputAction @LeftReleased => m_Wrapper.m_Movement_LeftReleased;
        public InputAction @DownReleased => m_Wrapper.m_Movement_DownReleased;
        public InputAction @DownPressed => m_Wrapper.m_Movement_DownPressed;
        public InputAction @RightPressed => m_Wrapper.m_Movement_RightPressed;
        public InputAction @RightReleased => m_Wrapper.m_Movement_RightReleased;
        public InputAction @RotUpPressed => m_Wrapper.m_Movement_RotUpPressed;
        public InputAction @RotUpReleased => m_Wrapper.m_Movement_RotUpReleased;
        public InputAction @RotDownPressed => m_Wrapper.m_Movement_RotDownPressed;
        public InputAction @RotDownReleased => m_Wrapper.m_Movement_RotDownReleased;
        public InputAction @FireTorpedo => m_Wrapper.m_Movement_FireTorpedo;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                UpPressed.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnUpPressed;
                UpPressed.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnUpPressed;
                UpPressed.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnUpPressed;
                UpReleased.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnUpReleased;
                UpReleased.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnUpReleased;
                UpReleased.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnUpReleased;
                LeftPressed.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnLeftPressed;
                LeftPressed.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnLeftPressed;
                LeftPressed.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnLeftPressed;
                LeftReleased.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnLeftReleased;
                LeftReleased.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnLeftReleased;
                LeftReleased.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnLeftReleased;
                DownReleased.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnDownReleased;
                DownReleased.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnDownReleased;
                DownReleased.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnDownReleased;
                DownPressed.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnDownPressed;
                DownPressed.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnDownPressed;
                DownPressed.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnDownPressed;
                RightPressed.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnRightPressed;
                RightPressed.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnRightPressed;
                RightPressed.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnRightPressed;
                RightReleased.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnRightReleased;
                RightReleased.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnRightReleased;
                RightReleased.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnRightReleased;
                RotUpPressed.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnRotUpPressed;
                RotUpPressed.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnRotUpPressed;
                RotUpPressed.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnRotUpPressed;
                RotUpReleased.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnRotUpReleased;
                RotUpReleased.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnRotUpReleased;
                RotUpReleased.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnRotUpReleased;
                RotDownPressed.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnRotDownPressed;
                RotDownPressed.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnRotDownPressed;
                RotDownPressed.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnRotDownPressed;
                RotDownReleased.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnRotDownReleased;
                RotDownReleased.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnRotDownReleased;
                RotDownReleased.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnRotDownReleased;
                FireTorpedo.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnFireTorpedo;
                FireTorpedo.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnFireTorpedo;
                FireTorpedo.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnFireTorpedo;
            }
            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                UpPressed.started += instance.OnUpPressed;
                UpPressed.performed += instance.OnUpPressed;
                UpPressed.canceled += instance.OnUpPressed;
                UpReleased.started += instance.OnUpReleased;
                UpReleased.performed += instance.OnUpReleased;
                UpReleased.canceled += instance.OnUpReleased;
                LeftPressed.started += instance.OnLeftPressed;
                LeftPressed.performed += instance.OnLeftPressed;
                LeftPressed.canceled += instance.OnLeftPressed;
                LeftReleased.started += instance.OnLeftReleased;
                LeftReleased.performed += instance.OnLeftReleased;
                LeftReleased.canceled += instance.OnLeftReleased;
                DownReleased.started += instance.OnDownReleased;
                DownReleased.performed += instance.OnDownReleased;
                DownReleased.canceled += instance.OnDownReleased;
                DownPressed.started += instance.OnDownPressed;
                DownPressed.performed += instance.OnDownPressed;
                DownPressed.canceled += instance.OnDownPressed;
                RightPressed.started += instance.OnRightPressed;
                RightPressed.performed += instance.OnRightPressed;
                RightPressed.canceled += instance.OnRightPressed;
                RightReleased.started += instance.OnRightReleased;
                RightReleased.performed += instance.OnRightReleased;
                RightReleased.canceled += instance.OnRightReleased;
                RotUpPressed.started += instance.OnRotUpPressed;
                RotUpPressed.performed += instance.OnRotUpPressed;
                RotUpPressed.canceled += instance.OnRotUpPressed;
                RotUpReleased.started += instance.OnRotUpReleased;
                RotUpReleased.performed += instance.OnRotUpReleased;
                RotUpReleased.canceled += instance.OnRotUpReleased;
                RotDownPressed.started += instance.OnRotDownPressed;
                RotDownPressed.performed += instance.OnRotDownPressed;
                RotDownPressed.canceled += instance.OnRotDownPressed;
                RotDownReleased.started += instance.OnRotDownReleased;
                RotDownReleased.performed += instance.OnRotDownReleased;
                RotDownReleased.canceled += instance.OnRotDownReleased;
                FireTorpedo.started += instance.OnFireTorpedo;
                FireTorpedo.performed += instance.OnFireTorpedo;
                FireTorpedo.canceled += instance.OnFireTorpedo;
            }
        }
    }
    public MovementActions @Movement => new MovementActions(this);
    private int m_ControlsSchemeIndex = -1;
    public InputControlScheme ControlsScheme
    {
        get
        {
            if (m_ControlsSchemeIndex == -1) m_ControlsSchemeIndex = asset.FindControlSchemeIndex("Controls");
            return asset.controlSchemes[m_ControlsSchemeIndex];
        }
    }
    public interface IMovementActions
    {
        void OnUpPressed(InputAction.CallbackContext context);
        void OnUpReleased(InputAction.CallbackContext context);
        void OnLeftPressed(InputAction.CallbackContext context);
        void OnLeftReleased(InputAction.CallbackContext context);
        void OnDownReleased(InputAction.CallbackContext context);
        void OnDownPressed(InputAction.CallbackContext context);
        void OnRightPressed(InputAction.CallbackContext context);
        void OnRightReleased(InputAction.CallbackContext context);
        void OnRotUpPressed(InputAction.CallbackContext context);
        void OnRotUpReleased(InputAction.CallbackContext context);
        void OnRotDownPressed(InputAction.CallbackContext context);
        void OnRotDownReleased(InputAction.CallbackContext context);
        void OnFireTorpedo(InputAction.CallbackContext context);
    }
}
