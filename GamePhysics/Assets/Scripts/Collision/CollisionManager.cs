using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public List<CollisionHull2D> objs = new List<CollisionHull2D>();

    private void Update()
    {
        foreach(CollisionHull2D i in objs)
        {
            foreach(CollisionHull2D j in objs)
            {
                if (i == j) continue;

                if (CollisionHull2D.TestCollision(i, j))
                {
                    i.GetComponent<MeshRenderer>().material.color = Color.red;
                    j.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else // gross cuz it does it every frame even if it doesn't have to, but will work for now.
                {
                    i.GetComponent<MeshRenderer>().material.color = Color.green;
                    j.GetComponent<MeshRenderer>().material.color = Color.green;
                }
            }
        }
    }
}
