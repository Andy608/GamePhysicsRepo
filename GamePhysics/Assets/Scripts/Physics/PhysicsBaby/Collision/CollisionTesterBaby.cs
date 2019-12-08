using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTesterBaby : ManagerBase<CollisionTesterBaby>
{
    private List<CollisionHullBaby> rigidBabyColliders = new List<CollisionHullBaby>();
    private List<RigidBabyContact> contacts = new List<RigidBabyContact>();
    private ContactResolverBaby contactResolver;
    [SerializeField] private int maxCollisionIterations = 2;

    public void RegisterHull(CollisionHullBaby hull)
    {
        rigidBabyColliders.Add(hull);
    }

    public void UnRegisterHull(CollisionHullBaby hull)
    {
        rigidBabyColliders.Remove(hull);
    }

    private void Start()
    {
        contactResolver = new ContactResolverBaby(maxCollisionIterations);
    }

    private void FixedUpdate()
    {
        int count = 0;

        for (int i = 0; i < rigidBabyColliders.Count; i++)
        {
            //rigidBabyColliders[i].RefreshOctantOwners();

            CollisionHullBaby currentObj = rigidBabyColliders[i];

            for (int j = i + 1; j < rigidBabyColliders.Count; j++)
            {
                CollisionHullBaby otherObj = rigidBabyColliders[j];
                CollisionHullBaby.TestCollision(currentObj, otherObj, ref contacts);
                count++;
            }
        }

        if (contacts.Count > 0)
        {
            contactResolver.ResolveContacts(ref contacts, Time.fixedDeltaTime);
            contacts.Clear();
        }
    }
}
