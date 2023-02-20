using UnityEditor;
using UnityEngine;

namespace ToolKid {
    
    public abstract class BaseEditor : Editor {

        bool foldout = true;

        public override void OnInspectorGUI() {

            serializedObject.Update();
            EditorGUILayout.Space(0);
            foldout = this.ScriptHint(foldout);
            CustomGUI();
            serializedObject.ApplyModifiedProperties();
        }

        public virtual void CustomGUI() {

        }
    }
}