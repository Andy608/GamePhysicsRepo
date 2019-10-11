using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class CollisionManager : ManagerBase<CollisionManager>
//{
//    private List<CollisionHull2D> objs = new List<CollisionHull2D>();
//    private List<CollisionHull2D> unCheckedObjs = new List<CollisionHull2D>();

//    public void RegisterObject(CollisionHull2D hull)
//    {
//        objs.Add(hull);
//    }

//    public void UnRegisterObject(CollisionHull2D hull)
//    {
//        objs.Remove(hull);
//    }

//    private void Update()
//    {
//        unCheckedObjs.AddRange(objs);

//		int count = 0;

//		for (int i = 0; i < unCheckedObjs.Count; i++)
//		{
//			CollisionHull2D currentObj = unCheckedObjs[i];

//			for (int j = i + 1; j < unCheckedObjs.Count; j++)
//			{
//				CollisionHull2D otherObj = unCheckedObjs[j];

//				CollisionHull2D.Collision col = null;
//				bool isColliding = CollisionHull2D.TestCollision(currentObj, otherObj, ref col);
//                currentObj.SetColliding(isColliding, otherObj);

//                //if (currentObj.IsColliding())
//                if (isColliding)
//				{
//                    col.Resolve();
//                }
//                count++;
//			}
//		}

//		Debug.Log("TESTED: " + count + " POSSIBLE COLLISIONS.");
//        Debug.Log("Registered Obj Count: " + objs.Count);

//		unCheckedObjs.Clear();
//    }
//}

public class CollisionManager : ManagerBase<CollisionManager>
{
    private List<Particle2D> particles = new List<Particle2D>();
    private List<CollisionHull2D> particleColliders = new List<CollisionHull2D>();

    private List<ParticleContact> contacts = new List<ParticleContact>();

    private ContactResolver contactResolver;
    [SerializeField] private int maxCollisionIterations = 5000;

    public void RegisterObject(CollisionHull2D hull)
    {
        particles.Add(hull.GetComponent<Particle2D>());
        particleColliders.Add(hull);
    }

    public void UnRegisterObject(CollisionHull2D hull)
    {
        particles.Remove(hull.GetComponent<Particle2D>());
        particleColliders.Remove(hull);
    }

    private void Start()
    {
        contactResolver = new ContactResolver(maxCollisionIterations);
    }

    private void FixedUpdate()
    {
        foreach (Particle2D p in particles)
        {
            p.Integrate(Time.fixedDeltaTime);
        }

        int count = 0;

        for (int i = 0; i < particleColliders.Count; i++)
        {
            CollisionHull2D currentObj = particleColliders[i];

            for (int j = i + 1; j < particleColliders.Count; j++)
            {
                CollisionHull2D otherObj = particleColliders[j];

                bool isColliding = CollisionHull2D.TestCollision(currentObj, otherObj, ref contacts);

                //Purely for debugging.
                currentObj.SetColliding(isColliding, otherObj);
                count++;
            }
        }

        if (contacts.Count > 0)
        {
            contactResolver.ResolveContacts(contacts.ToArray(), Time.fixedDeltaTime);
            contacts.Clear();
        }
    }
}
