using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBabyIntegrator : ManagerBase<RigidBabyIntegrator>
{
    public HashSet<RigidBaby> RigidBabies { get; private set; } = new HashSet<RigidBaby>();

    public void RegisterRigidBaby(RigidBaby rigidBaby)
    {
        RigidBabies.Add(rigidBaby);
    }

    public void UnRegisterRigidBaby(RigidBaby rigidBaby)
    {
        RigidBabies.Remove(rigidBaby);
    }

    private void FixedUpdate()
    {
        ForceRegistry.Instance.UpdateForces();

        foreach (RigidBaby r in RigidBabies)
        {
            r.Integrate();
        }
    }
}
