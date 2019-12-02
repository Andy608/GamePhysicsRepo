using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RigidBabyIntegrator))]
public class ForceDriver : MonoBehaviour
{
    private RigidBabyIntegrator integrator = null;

    #region Sandbox Variables

    [SerializeField] private IForceBaby[] worldForces = null;

    #endregion

    private void Start()
    {
        integrator = GetComponent<RigidBabyIntegrator>();
        InitSandbox();
    }

    private void FixedUpdate()
    {
        UpdateSandbox();
    }

    private void InitSandbox()
    {
        //worldGravity.Init(new Vector3(0.0f, -9.8f, 0.0f));
        worldForces = GetComponents<IForceBaby>();

        foreach (RigidBaby rigidbaby in integrator.RigidBabies)
        {
            foreach (IForceBaby force in worldForces)
            {
                rigidbaby.InitPersistentForce(force);
            }
        }
    }

    private void UpdateSandbox()
    {
    }
}
