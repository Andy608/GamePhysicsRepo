using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBabyIntegrator : ManagerBase<RigidBabyIntegrator>
{
    private List<RigidBaby> rigidBabies = new List<RigidBaby>();

    public void RegisterRigidBaby(RigidBaby rigidBaby)
    {
        rigidBabies.Add(rigidBaby);
    }

    public void UnRegisterRigidBaby(RigidBaby rigidBaby)
    {
        rigidBabies.Remove(rigidBaby);
    }

    private void FixedUpdate()
    {
        foreach (RigidBaby r in rigidBabies)
        {
            r.Integrate();
        }
    }
}
