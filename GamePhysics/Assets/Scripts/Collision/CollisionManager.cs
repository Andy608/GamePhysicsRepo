using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : ManagerBase<CollisionManager>
{
    private List<CollisionHull2D> objs = new List<CollisionHull2D>();
    private List<CollisionHull2D> unCheckedObjs = new List<CollisionHull2D>();

    public void RegisterObject(CollisionHull2D hull)
    {
        objs.Add(hull);
    }

    public void UnRegisterObject(CollisionHull2D hull)
    {
        objs.Remove(hull);
    }

    private void Update()
    {
        unCheckedObjs.AddRange(objs);

		//List<CollisionHull2D.Collision> collisions = new List<CollisionHull2D.Collision>();

		int count = 0;

		for (int i = 0; i < unCheckedObjs.Count; i++)
		{
			CollisionHull2D currentObj = unCheckedObjs[i];

			for (int j = i + 1; j < unCheckedObjs.Count; j++)
			{
				CollisionHull2D otherObj = unCheckedObjs[j];

				CollisionHull2D.Collision col = null;
				bool isColliding = CollisionHull2D.TestCollision(currentObj, otherObj, ref col);
                currentObj.SetColliding(isColliding, otherObj);

                //if (currentObj.IsColliding())
                if (isColliding)
				{
                    //collisions.Add(col);
                    col.Resolve();
                }
                count++;
			}
		}

		Debug.Log("TESTED: " + count + " POSSIBLE COLLISIONS.");
        Debug.Log("Registered Obj Count: " + objs.Count);


		//foreach (CollisionHull2D otherObj in objs)
        //{
        //    for (int i = 0; i < unCheckedObjs.Count; ++i)
        //    {
        //        CollisionHull2D currentObj = unCheckedObjs[i];
		//
        //        if (currentObj.GetInstanceID() == otherObj.GetInstanceID()) continue;
		//
        //        CollisionHull2D.Collision col = null;
        //        bool isColliding = CollisionHull2D.TestCollision(currentObj, otherObj, ref col);
        //        currentObj.SetColliding(isColliding, otherObj, col);
		//
		//		if (currentObj.IsColliding())
		//		{
		//			collisions.Add(col);
		//		}
		//
		//		//otherObj.SetColliding(isColliding, currentObj, col);
        //    }
		//
        //    //unCheckedObjs.RemoveAt(0);
        //}

		//if (collisions.Count > 0)
		//{
		//	collisions[0].Resolve();
		//}

		//foreach (CollisionHull2D.Collision cols in collisions)
		//{
			//cols.Resolve();
		//}

		//collisions.Clear();
		unCheckedObjs.Clear();
    }
}
