using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace ToolKid.InventorySystem {
    [CustomEditor(typeof(InventoryBase))]
    [CanEditMultipleObjects]
    public class InventoryBaseEditor : Editor {

        InventoryBase script;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            script = (InventoryBase)target;
            serializedObject.Update();
            if (GUILayout.Button("Enable")) {
                script.Enable();
            }
            if (GUILayout.Button("Disable")) {
                script.Disable();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
