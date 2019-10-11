using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : ManagerBase<HitDetection>
{
    private void OnEnable()
    {
        EventAnnouncer.OnCollisionOccurred += DetectHit;
    }

    private void OnDisable()
    {
        EventAnnouncer.OnCollisionOccurred -= DetectHit;
    }

    private void DetectHit(Particle2D a, Particle2D b)
    {
        if ((a.tag == "Player" && b.tag == "Fishy") || (a.tag == "Fishy" && b.tag == "Player"))
        {
            Debug.Log("Player got hit!");
            return;
        }
        else if (a.tag == "Bubble" && b.tag == "Fishy")
        {
            Debug.Log("Fishy got hit by a torpedo!");
            b.GetComponent<FishDieWithPhysics>().Activate();
            Destroy(a.gameObject);
        }
        else if (a.tag == "Fishy" && b.tag == "Bubble")
        {
            Debug.Log("Fishy got hit by a torpedo!");
            a.GetComponent<FishDieWithPhysics>().Activate();
            Destroy(b.gameObject);
        }
    }
}
