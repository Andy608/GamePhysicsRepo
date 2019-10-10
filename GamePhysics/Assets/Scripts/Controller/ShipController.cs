using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    private ShipControls controls;
    private Transform shipTransform;
    private Particle2D shipParticle;
    private Animator propellerAnimator;

    [SerializeField] private float movementSpeed = 100.0f;

    private Coroutine MoveShipCoroutine;
    private Dictionary<EnumDirection, Vector2> totalDirectionalForces = new Dictionary<EnumDirection, Vector2>();


    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Awake()
    {
        shipTransform = transform;

        controls = new ShipControls();
        controls.Movement.UpPressed.performed += context => OnPressedUp();
        controls.Movement.DownPressed.performed += context => OnPressedDown();
        controls.Movement.LeftPressed.performed += context => OnPressedLeft();
        controls.Movement.RightPressed.performed += context => OnPressedRight();

        controls.Movement.UpReleased.performed += context => OnReleased(EnumDirection.UP);
        controls.Movement.DownReleased.performed += context => OnReleased(EnumDirection.DOWN);
        controls.Movement.LeftReleased.performed += context => OnReleased(EnumDirection.LEFT);
        controls.Movement.RightReleased.performed += context => OnReleased(EnumDirection.RIGHT);

        shipParticle = GetComponent<Particle2D>();
    }

    private void OnPressedUp()
    {
        totalDirectionalForces[EnumDirection.UP] = Vector2.up * movementSpeed;
    }

    private void OnPressedDown()
    {
        totalDirectionalForces[EnumDirection.DOWN] = Vector2.down * movementSpeed;
    }

    private void OnPressedLeft()
    {
        totalDirectionalForces[EnumDirection.LEFT] = Vector2.left * movementSpeed;
    }

    private void OnPressedRight()
    {
        totalDirectionalForces[EnumDirection.RIGHT] = Vector2.right * movementSpeed;
    }

    private void OnReleased(EnumDirection dir)
    {
        totalDirectionalForces[dir] = Vector2.zero;
    }

    private void Update()
    {
        foreach (Vector2 force in totalDirectionalForces.Values)
        {
            shipParticle.AddForce(force);
        }
    }

    private enum EnumDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
}
