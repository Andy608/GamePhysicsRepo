using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Put this script on the _SCENE_MANAGER in the scene if it doesn't need any special functionality.
public class GenericSceneManager : SceneBase<GenericSceneManager>
{
    private void OnEnable()
    {
        Debug.Log("Generic Scene Enabled");
    }
}
