using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IForceBaby : MonoBehaviour
{
    [SerializeField] private bool isPersistent = false;
    public bool IsPersistent { get; private set; } = false;

    protected virtual void Awake()
    {
        IsPersistent = isPersistent;
    }

    public abstract void UpdateForce(RigidBaby rigidbaby, float deltaTime);

    //Use this instead of NormalBaby if you need to access the force instead of adding it to the rigidbaby.
    protected static Vector3 CalculateNormal(Vector3 gravitationalForce, Vector3 surfaceNormal)
    {
        return Vector3.Project(-gravitationalForce, surfaceNormal);
    }
}
