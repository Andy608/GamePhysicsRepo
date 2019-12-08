using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBTestWorld : ManagerBase<RBTestWorld>
{
    //public OctreeOld WorldOctree { get { return worldOctree; } }
    [SerializeField] private Octree worldOctree = null;

    private void Awake()
    {
        if (worldOctree == null)
        {
            worldOctree = new Octree();
        }

        //worldOctree.CreateOctree();
    }
}
