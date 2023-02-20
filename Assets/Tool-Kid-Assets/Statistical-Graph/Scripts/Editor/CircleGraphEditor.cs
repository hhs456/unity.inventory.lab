using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ToolKid {

    [CustomEditor(typeof(CircleGraph))]
    public class CircleGraphEditor : BaseEditor {
        public override void CustomGUI() {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("style"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("distanceFromCenter"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sliceValues"), new GUIContent("�ƾڮw Data"));
        }
    }
}