//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public abstract class ForceGenerator : MonoBehaviour
//{
//    protected abstract void UpdateForce(Particle2D particle, float deltaTime);
//}

//public class Gravity : ForceGenerator
//{
//    [SerializeField] private Vector2 gravity;

//    protected override void UpdateForce(Particle2D particle, float deltaTime)
//    {
//        if (!particle.IsMassFinite())
//        {
//            return;
//        }

//        particle.AddForce(gravity * particle.Mass);
//    }
//}

//public class Spring : ForceGenerator
//{
//    [SerializeField] private Vector2 connectionPoint;
//    [SerializeField] private Vector2 otherConnectionPoint;

//    [SerializeField] private Particle2D particleAtEnd;
//    [SerializeField] private float springConstant;
//    [SerializeField] private float restLength;

//    protected override void UpdateForce(Particle2D particle, float deltaTime)
//    {
//        Vector2 lws = particle.Position;
//        Vector2 ows = particleAtEnd.Position;

//        Vector2 force = lws - ows;

//        float magnitude = force.magnitude();
//        magnitude = Mathf.Abs(magnitude - restLength);
//        magnitude *= springConstant;
//        force.Normalize();
//        force *= -magnitude;
//        particle.AddForce(force);
//    }
//}

//public class Buoyancy : ForceGenerator
//{
//    //The maximum submersion depth of the object before
//    //it generates it's maximum boyancy force.
//    [SerializeField] private float maxDepth;

//    //The volume of the object
//    [SerializeField] private float volume;

//    //The height of the water above y = 0.
//    [SerializeField] private float waterHeight;

//    //The density of the liquid.
//    //Pure water has a density of 1000kg per cubic meter.
//    [SerializeField] private float liquidDensity;

//    //The center of buoyancy of the particle.
//    [SerializeField] private Vector2 centerOfBuoyancy;
    
//    protected override void UpdateForce(Particle2D particle, float deltaTime)
//    {
        
//    }
//}

//public class Friction : ForceGenerator
//{
//    //The maximum submersion depth of the object before
//    //it generates it's maximum boyancy force.
//    [SerializeField] private float maxDepth;

//    protected override void UpdateForce(Particle2D particle, float deltaTime)
//    {

//    }
//}

////f_friction_s = -f_opposing if less than coeff*|f_normal|, else -coeff*|f_normal| (max amount is )
//public static Vector2 GenerateForce_FrictionStatic(Vector2 normalForce, Vector2 opposingForce, float materialCoefficientStatic)
//{
//    float frictionForceMax = materialCoefficientStatic * normalForce.magnitude;

//    float oppMag = opposingForce.magnitude;

//    if (oppMag < frictionForceMax)
//    {
//        return -opposingForce;
//    }
//    else
//    {
//        return -opposingForce * frictionForceMax / oppMag;
//    }
//}

////    // f_friction_k = -coeff*|f_normal| * unit(vel)
////    public static Vector2 GenerateForce_FrictionKinetic(Vector2 normalForce, Vector2 particleVelocity, float materialCoefficientKinetic)
////    {
////        return -materialCoefficientKinetic * normalForce.magnitude * particleVelocity.normalized;
////    }

////    public static Vector2 GenerateForce_Friction(Vector2 normalForce, Vector2 opposingForce, Vector2 particleVelocity, 
////        float materialCoefficientStatic, float materialCofficientKinetic)
////    {
////        Vector2 frictionForce = Vector2.zero;

////        if (particleVelocity.SqrMagnitude() <= 0.0f)
////        {
////            frictionForce = GenerateForce_FrictionStatic(normalForce, opposingForce, materialCoefficientStatic);
////        }
////        else
////        {
////            frictionForce = GenerateForce_FrictionKinetic(normalForce, particleVelocity, materialCofficientKinetic);
////        }

////        return frictionForce;
////    }

//public class ForceRegistry : ManagerBase<ForceRegistry>
//{
//    struct ForceRegistration
//    {
//        public Particle2D particle;
//        public ForceGenerator forceGenerator;
//    }

//    private List<ForceRegistration> registryList;

//    public void Register(Particle2D particle, ForceGenerator forceGenerator)
//    {

//    }

//    public void UnRegister(Particle2D particle, ForceGenerator forceGenerator)
//    {

//    }

//    public void Clear()
//    {

//    }

//    public void UpdateForces(float deltaTime)
//    {

//    }
//}