using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private CollisionHull2D[] objs;
    private List<CollisionHull2D> unCheckedObjs = new List<CollisionHull2D>();

    private void Start()
    {
        objs = GetComponentsInChildren<CollisionHull2D>();
    }

    private void Update()
    {
        unCheckedObjs.AddRange(objs);

        foreach (CollisionHull2D otherObj in objs)
        {
            for (int i = 0; i < unCheckedObjs.Count; ++i)
            {
                CollisionHull2D currentObj = unCheckedObjs[i];

                if (currentObj.GetInstanceID() == otherObj.GetInstanceID()) continue;

                bool isColliding = CollisionHull2D.TestCollision(currentObj, otherObj);
                currentObj.SetColliding(isColliding, otherObj);
                otherObj.SetColliding(isColliding, currentObj);

                if (isColliding)
                {
                    InitCollision(currentObj, otherObj);
                }
            }

            unCheckedObjs.RemoveAt(0);
        }

        unCheckedObjs.Clear();
    }

    private void InitCollision(CollisionHull2D a, CollisionHull2D b)
    {
        Particle2D aParticle = a.GetComponent<Particle2D>();
        Particle2D bParticle = b.GetComponent<Particle2D>();

        Vector2 normal = (aParticle.Position - bParticle.Position).normalized;

        CollisionHull2D.Collision.Contact[] contacts = new CollisionHull2D.Collision.Contact[4];

        //TODO: FIND 4 CONTACT THINGS

        float closingVel = -Vector2.Dot(
            (aParticle.Velocity - bParticle.Velocity), 
            (aParticle.Position - bParticle.Position).normalized
        );


        CollisionHull2D.Collision c = new CollisionHull2D.Collision(a, b, contacts, closingVel);


    }
}
