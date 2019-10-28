using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Particle2D))]
public class ShipController : MonoBehaviour
{
    private ShipControls controls;
    private Transform shipTransform;
    private Particle2D shipParticle;
    private Animator propellerAnimator;

    [SerializeField] private TorpedoSpawner torpedoSpawner = null;
    [SerializeField] private SpriteRenderer hull = null;
    private Vector2 halfHullSize;

    [SerializeField] private float movementSpeed = 100.0f;
    //[SerializeField] private float maxRotationOffset = 80.0f;
    private float rotationSpeed = 360.0f;


    private Vector2 RotationalForce;
    private Vector2 RotationalPoint = Vector2.right * 5.0f;

    private Coroutine MoveShipCoroutine;
    private Dictionary<EnumDirection, Vector2> totalDirectionalForces = new Dictionary<EnumDirection, Vector2>();
    private Dictionary<EnumRotDirection, Vector2> totalRotDirectionalForces = new Dictionary<EnumRotDirection, Vector2>();

    private bool isSpriteFlipped = false;

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

        controls.Movement.RotUpPressed.performed += context => OnRotPressedUp(EnumRotDirection.CCW);
        controls.Movement.RotDownPressed.performed += context => OnRotPressedDown(EnumRotDirection.CW);

        controls.Movement.UpReleased.performed += context => OnReleased(EnumDirection.UP);
        controls.Movement.DownReleased.performed += context => OnReleased(EnumDirection.DOWN);
        controls.Movement.LeftReleased.performed += context => OnReleased(EnumDirection.LEFT);
        controls.Movement.RightReleased.performed += context => OnReleased(EnumDirection.RIGHT);

        controls.Movement.RotUpReleased.performed += context => OnRotReleased(EnumRotDirection.CCW);
        controls.Movement.RotDownReleased.performed += context => OnRotReleased(EnumRotDirection.CW);

        controls.Movement.FireTorpedo.performed += context => OnTorpedoFired();

        shipParticle = GetComponent<Particle2D>();

        //shipParticle.RotUpperBound = maxRotationOffset;
        //shipParticle.RotLowerBound = -maxRotationOffset;
        //shipParticle.IsRotBounded = true;

        RotationalForce = Vector2.right * rotationSpeed;

        halfHullSize = new Vector2(hull.sprite.texture.width / hull.sprite.pixelsPerUnit * 0.5f, hull.sprite.texture.height / hull.sprite.pixelsPerUnit * 0.5f);
    }

    private void OnPressedUp()
    {
        totalDirectionalForces[EnumDirection.UP] = Vector2.up * movementSpeed;
        //totalRotDirectionalForces[EnumRotDirection.UP] = RotationalForce;
    }

    private void OnPressedDown()
    {
        totalDirectionalForces[EnumDirection.DOWN] = Vector2.down * movementSpeed;
        //totalRotDirectionalForces[EnumDirection.UP] = -RotationalForce;
    }

    private void OnPressedLeft()
    {
        totalDirectionalForces[EnumDirection.LEFT] = Vector2.left * movementSpeed;
        FlipShip(true);
    }

    private void OnPressedRight()
    {
        totalDirectionalForces[EnumDirection.RIGHT] = Vector2.right * movementSpeed;
        FlipShip(false);
    }

    private void OnReleased(EnumDirection dir)
    {
        totalDirectionalForces[dir] = Vector2.zero;
    }

    private void OnRotPressedUp(EnumRotDirection dir)
    {
        totalRotDirectionalForces[dir] = RotationalForce;
    }

    private void OnRotPressedDown(EnumRotDirection dir)
    {
        totalRotDirectionalForces[dir] = -RotationalForce;
    }

    private void OnRotReleased(EnumRotDirection dir)
    {
        totalRotDirectionalForces[dir] = Vector2.zero;
    }

    private void OnTorpedoFired()
    {
        EventAnnouncer.OnButtonPressed?.Invoke(EnumSound.BUBBLE_FIRE);
        torpedoSpawner.FireTorpedo(isSpriteFlipped);
    }

    private void FixedUpdate()
    {
        if (GameScene.Instance.IsRunning)
        {
            foreach (Vector2 force in totalDirectionalForces.Values)
            {
                shipParticle.AddForce(force);
            }

            foreach (Vector2 rotForce in totalRotDirectionalForces.Values)
            {
                shipParticle.ApplyTorque(RotationalPoint, rotForce);
            }
        }
	}

    private void FlipShip(bool flip)
    {
        isSpriteFlipped = flip;
        Vector3 flippedScale = shipTransform.localScale;
        flippedScale.x = (flip ? -1.0f : 1.0f);
        shipTransform.localScale = flippedScale;
    }

    private enum EnumRotDirection
    {
        CW,
        CCW
    }

    private enum EnumDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
}
