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
        if (a.tag == "Player" && b.tag == "Fishy")
        {
            //Debug.Log("Player got hit!");
            GameScene.Instance.DamagePlayer(b.GetComponent<Fishy>().GetDamageValue());
            return;
        }
        else if (a.tag == "Fishy" && b.tag == "Player")
        {
            //Debug.Log("Player got hit!");
            GameScene.Instance.DamagePlayer(a.GetComponent<Fishy>().GetDamageValue());
            return;
        }
        else if (a.tag == "Bubble" && b.tag == "Fishy")
        {
            //Debug.Log("Fishy got hit by a torpedo!");
            b.GetComponent<FishDieWithPhysics>().Activate();
			ScoreManager.Instance.AddScore(b.GetComponent<Fishy>().pointsAwarded);
			Destroy(a.gameObject);
        }
        else if (a.tag == "Fishy" && b.tag == "Bubble")
        {
            //Debug.Log("Fishy got hit by a torpedo!");
            a.GetComponent<FishDieWithPhysics>().Activate();
			ScoreManager.Instance.AddScore(a.GetComponent<Fishy>().pointsAwarded);
			Destroy(b.gameObject);
        }
    }
}
