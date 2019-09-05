using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(Particle2D))]
public class Particle2DEditor : Editor
{
    //UnityEvent uEvent;

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        Particle2D particle = (Particle2D)target;

        GUILayout.BeginHorizontal();
        GUILayout.Label("Action");
        GUILayout.EndHorizontal();

        //EditorGUILayout.PropertyField(serializedObject.FindProperty("myDelegate"), true);
        //serializedObject.ApplyModifiedProperties();
    }
}
