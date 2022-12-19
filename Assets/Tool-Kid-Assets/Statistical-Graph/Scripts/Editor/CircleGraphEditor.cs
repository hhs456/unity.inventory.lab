using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ToolKid {

    [CustomEditor(typeof(CircleGraph))]
    public class CircleGraphEditor : Editor {
                
        bool foldout = true;

        void OnEnable() {

        }

        public override void OnInspectorGUI() {

            serializedObject.Update();
            
            foldout = this.ScriptHint<CircleGraph>(foldout);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("style"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("distanceFromCenter"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sliceValues"), new GUIContent("�ƾڮw Data"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}