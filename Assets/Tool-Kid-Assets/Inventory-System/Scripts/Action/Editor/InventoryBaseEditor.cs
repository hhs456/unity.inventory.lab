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
                script.Enable(true);
            }
            if (GUILayout.Button("Disable")) {
                script.Enable(false);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
