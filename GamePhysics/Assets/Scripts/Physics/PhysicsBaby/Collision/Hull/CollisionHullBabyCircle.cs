using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHullBabyCircle : CollisionHullBaby
{
    protected CollisionHullBabyCircle() : base(CollisionHullBabyType.Circle) {}

    public float radius = 5.0f;
    public float restitution = 0.0f;

    public override bool TestCollisionVsCircle(CollisionHullBabyCircle other, ref List<RigidBabyContact> c)
    {
        //collides if distance between centers is <= sum of radii
        //or if you like opptimizations
        //if (distance between center)^2 <= (sum of radii)^2
        // 1. get two centers
        Vector3 circleCenter1 = transform.position;
        Vector3 circleCenter2 = other.transform.position;
        // 2. take diference
        Vector3 distance = circleCenter2 - circleCenter1;
        // 3. distance squared = dot(diff, diff)
        float distSqr = Vector3.Dot(distance, distance);
        // 4. add the radii
        float radiiSum = other.radius + radius;
        // 5. square sum
        radiiSum *= radiiSum;
        // 6. do test. johnny test. it passes if distSq <= sumSq
        if (distSqr < radiiSum)
        {
            RigidBaby aParticle = GetComponent<RigidBaby>();
            RigidBaby bParticle = other.GetComponent<RigidBaby>();

            Vector2 normal = (aParticle.GetPosition() - bParticle.GetPosition()).normalized;
            float penetration = other.radius + radius - distance.magnitude;

            RigidBabyContact contact = new RigidBabyContact(
                aParticle, bParticle, restitution, normal, penetration);
            c.Add(contact);

            return true;
        }

        return false;
    }

    public override bool TestCollisionVsAABB(CollisionHullBabyAABB other, ref List<RigidBabyContact> c)
    {
        return false;
    }

    public override bool TestCollisionVsObject(CollisionHullBabyOBB other, ref List<RigidBabyContact> c)
    {
        return false;
    }
}
