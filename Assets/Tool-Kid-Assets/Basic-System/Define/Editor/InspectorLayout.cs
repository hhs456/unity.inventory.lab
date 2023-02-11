using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public static class InspectorLayout {
    
    public static void PropertyField(Rect Begin, SerializedProperty property, GUIContent content, out Rect End) {
        float h = EditorGUI.GetPropertyHeight(property);
        Begin.y += InspectorUtility.space;
        Begin.height = h;
        EditorGUI.PropertyField(Begin, property, content);
        End = new Rect(Begin.x, Begin.y + h, 0f, h);
    }
    public static void ObjectField(Rect Begin, SerializedProperty property, GUIContent content, out Rect End) {
        float h = EditorGUI.GetPropertyHeight(property);
        Begin.y += InspectorUtility.space;
        Begin.height = h;
        EditorGUI.ObjectField(Begin, property, content);
        End = new Rect(Begin.x, Begin.y + h, 0f, h);
    }
}
