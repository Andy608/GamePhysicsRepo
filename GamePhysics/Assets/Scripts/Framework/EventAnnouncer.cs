using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAnnouncer : ManagerBase<EventAnnouncer>
{
    #region Scene Change Events

    //Request the scene to change. Returns true if successful.
    public delegate bool RequestSceneChange(EnumScene targetScene, TransitionEffect transition = null);
    public static RequestSceneChange OnRequestSceneChange;

    //Locks the scene so it can't be changed while it is transitioning.
    public delegate void StartSceneChange(EnumScene currentScene, EnumScene targetScene);
    public static StartSceneChange OnStartSceneChange;

    //Continuously sends out this event while the scene is transitioning.
    public delegate void SceneChanging(EnumScene currentScene, EnumScene targetScene);
    public static SceneChanging OnSceneChanging;

    //Unlocks the scene so it can't be changed while it is transitioning.
    public delegate void EndSceneChange(EnumScene newScene);
    public static EndSceneChange OnEndSceneChange;

    public delegate void SceneChanged(EnumScene newScene);
    public static SceneChanged OnSceneChanged;

    #endregion

    #region Collision Events

    //Announce that a collision happened
    public delegate void CollisionOccurred(Particle2D a, Particle2D b);
    public static CollisionOccurred OnCollisionOccurred;

    #endregion
}
