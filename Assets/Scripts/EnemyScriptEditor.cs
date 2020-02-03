using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(EnemyScript))]
public class EnemyScriptEditor : Editor {
    public override void OnInspectorGUI() {
        //DrawDefaultInspector();
        EnemyScript enemy = (EnemyScript)target;
        if(GUILayout.Button("Generate Path"))
        {
            enemy.StorePath();
        }    
        base.OnInspectorGUI();
        
    }
}