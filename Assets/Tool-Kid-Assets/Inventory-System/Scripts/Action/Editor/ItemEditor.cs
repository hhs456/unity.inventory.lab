using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace ToolKid.InventorySystem {
    [CustomEditor(typeof(Item))]
    [CanEditMultipleObjects]
    public class ItemEditor : Editor {

        Item script;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            script = (Item)target;
            serializedObject.Update();
            if (GUILayout.Button("Collect")) {
                script.Collect();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
