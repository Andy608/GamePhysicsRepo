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
                //j.SetColliding(isColliding, i);
            }

            //unCheckedObjs.RemoveAt(0);
        }

        unCheckedObjs.Clear();
    }
}
