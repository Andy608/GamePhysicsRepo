using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : ManagerBase<CollisionManager>
{
    private List<CollisionHull2D> particleColliders = new List<CollisionHull2D>();

    private List<ParticleContact> contacts = new List<ParticleContact>();

    private ContactResolver contactResolver;
    [SerializeField] private int maxCollisionIterations = 2;

    public void RegisterHull(CollisionHull2D hull)
    {
        particleColliders.Add(hull);
    }

    public void UnRegisterHull(CollisionHull2D hull)
    {
        particleColliders.Remove(hull);
    }

    private void Start()
    {
        contactResolver = new ContactResolver(maxCollisionIterations);
    }

    private void FixedUpdate()
    {
        int count = 0;

        for (int i = 0; i < particleColliders.Count; i++)
        {
            CollisionHull2D currentObj = particleColliders[i];

            for (int j = i + 1; j < particleColliders.Count; j++)
            {
                CollisionHull2D otherObj = particleColliders[j];

                if ((currentObj.tag == "Coral" && otherObj.tag != "Player") ||
                    (currentObj.tag != "Player" && otherObj.tag == "Coral") ||
                    (currentObj.tag == "Bubble" && otherObj.tag == "Bubble") ||
                    (currentObj.tag == "Player" && otherObj.tag == "Bubble") ||
                    (currentObj.tag == "Bubble" && otherObj.tag == "Player"))
                {
                    continue;
                }

                CollisionHull2D.TestCollision(currentObj, otherObj, ref contacts);
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
