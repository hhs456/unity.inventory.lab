using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace ToolKid.InventorySystem {
    [CustomEditor(typeof(ItemViewer))]
    [CanEditMultipleObjects]
    public class ItemViewerEditor : Editor {

        static ItemViewer script;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            script = (ItemViewer)target;
            serializedObject.Update();
            script.dataBaseAddress = AssetDatabase.GetAssetPath(script.dataBase);
            if (GUILayout.Button("Save Data")) {
                SaveData();
            }
            if (GUILayout.Button("Load Data")) {
                LoadData();
            }
            serializedObject.ApplyModifiedProperties();
        }

        public void SaveData() {            
            string js = JsonUtility.ToJson(script.Items, true);
            Debug.Log(js, this);
            File.WriteAllText(script.dataBaseAddress, js);
        }

        public void LoadData() {
            JsonUtility.FromJson<Item[]>(script.dataBase.text);
        }
    }
}
