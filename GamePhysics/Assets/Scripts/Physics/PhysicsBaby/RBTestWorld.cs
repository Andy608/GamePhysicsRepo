using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBTestWorld : ManagerBase<RBTestWorld>
{
    public Octree WorldOctree { get { return worldOctree; } }
    [SerializeField] private Octree worldOctree = null;
}
