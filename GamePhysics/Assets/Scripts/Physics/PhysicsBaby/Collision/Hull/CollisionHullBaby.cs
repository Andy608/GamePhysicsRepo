using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//I LEFT THIS ERROR IN HERE ON PURPOSE SO I KNOW WHERE TO CONTINUE LATER ON
[RequireComponent(typeof(Rigidbady))]
public class CollisionHullBaby : MonoBehaviour
{
    protected RigidBaby rigidbaby;

    protected CollisionHullBaby()
    {

    }

    private void Awake()
    {
        rigidbaby = GetComponent<RigidBaby>();
    }

    private void OnEnable()
    {
        CollisionTesterBaby.Instance?.RegisterHull(this);
    }
}
