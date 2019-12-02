using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceRegistry : ManagerBase<ForceRegistry>
{
    private List<ForceEntry> registry = null;

    private void Awake()
    {
        registry = new List<ForceEntry>();
    }

    public void Add(RigidBaby rigidbaby, IForceBaby force)
    {
        registry.Add(new ForceEntry(rigidbaby, force));
    }

    public void UpdateForces()
    {
        foreach (ForceEntry entry in registry)
        {
            entry.Force.UpdateForce(entry.Rigidbaby, Time.fixedDeltaTime);

            //foreach (IForceBaby persistentForce in entry.Rigidbaby.PersistentForces)
            //{
            //    entry.Force.UpdateForce(entry.Rigidbaby, Time.fixedDeltaTime);
            //}
        }

        registry.Clear();
    }
}

public class ForceEntry
{
    public RigidBaby Rigidbaby { get; private set; } = null;
    public IForceBaby Force { get; private set; } = null;

    public ForceEntry(RigidBaby rigidbaby, IForceBaby force)
    {
        Rigidbaby = rigidbaby;
        Force = force;
    }
}