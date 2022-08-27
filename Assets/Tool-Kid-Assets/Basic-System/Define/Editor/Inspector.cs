using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public static class Inspector {
    
    public static Rect DropArea = new Rect();

    public static void ScriptHint(Editor editor ,MonoBehaviour script, bool inheritBase) {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Editor", MonoScript.FromScriptableObject(editor), script.GetType(), true);
        if(!inheritBase)
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(script), script.GetType(), true);
        EditorGUI.EndDisabledGroup();
    }
    public static void ScriptHint(Editor editor, ScriptableObject script, bool inheritBase) {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Editor", MonoScript.FromScriptableObject(editor), script.GetType(), true);
        if (!inheritBase)
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(script), script.GetType(), true);
        EditorGUI.EndDisabledGroup();
    }

    public static void DoLayoutList(ref bool foldout, ref ReorderableList list, Rect drapArea, string label, string tooltip) {
        if (GUI.Button(drapArea, "")) {
            foldout = !foldout;
        }
        DropAreaGUI(ref list, drapArea, Event.current);
        if (EditorGUILayout.Foldout(foldout, new GUIContent(label, tooltip))) {            
            list.DoLayoutList();
        }
    }

    public static void DoList(ref bool foldout, ref ReorderableList reorderableList, Rect drapArea, string label, string tooltip) {
        if (GUI.Button(drapArea, "")) {
            foldout = !foldout;
        }
        EditorGUI.indentLevel++;
        DropAreaGUI(ref reorderableList, drapArea, Event.current);
        if (EditorGUI.Foldout(drapArea, foldout, new GUIContent(label, tooltip))) {
            drapArea.y += EditorGUIUtility.singleLineHeight;
            reorderableList.DoList(drapArea);
        }
        EditorGUI.indentLevel--;
    }

    public static void DrawPropertyDataList(this ReorderableList list, SerializedProperty props, string label, string tooltip) {                  
        list.drawElementCallback = (rect, index, isActive, isFocused) => {
            Rect arect = rect;            
            arect.y = rect.y + 2.45f;
            arect.height = list.elementHeight;
            EditorGUI.LabelField(arect, new GUIContent(label + " " + index, tooltip));
            arect.x = rect.x + 48;
            arect.width = rect.width - rect.width / 4;
            EditorGUI.PropertyField(arect, props.GetArrayElementAtIndex(index), new GUIContent(""));            
        };        
    }

    public static void DrawClassDataList(this ReorderableList reorderableList, SerializedProperty props, string label, string tooltip, bool hasDrawer) {
        reorderableList.drawElementCallback = (rect, index, isActive, isFocused) => {
            Rect arect = rect;
            arect.y = rect.y + 2.45f;
            arect.height = reorderableList.elementHeight;
            arect.x = rect.x + 12;
            arect.width = rect.width - rect.width / 10;
            EditorGUI.PropertyField(arect, props.GetArrayElementAtIndex(index), new GUIContent("", tooltip));
        };
        reorderableList.elementHeightCallback = (index) => EditorGUI.GetPropertyHeight(props.GetArrayElementAtIndex(index));
        reorderableList.onAddCallback = (list) => {
            props.arraySize++;
            props.GetArrayElementAtIndex(props.arraySize - 1).FindPropertyRelative("enable").boolValue = false;
            props.GetArrayElementAtIndex(props.arraySize - 1).FindPropertyRelative("index").intValue = props.arraySize - 1;
        };
        reorderableList.onRemoveCallback = (list) => {
            int index = list.index;
            list.serializedProperty.DeleteArrayElementAtIndex(index);
            int i_size = list.count;
            for (int i = index; i < i_size; i++) {
                list.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("index").intValue = i;
                list.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("enable").boolValue = false;
            }
        };
    }

    public static void DropAreaGUI(ref SerializedProperty list, Rect drop_area, params string[] propertiesName) {
        Event operating = Event.current;
        switch (operating.type) {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!drop_area.Contains(operating.mousePosition))
                    return;
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (operating.type == EventType.DragPerform) {
                    DragAndDrop.AcceptDrag();
                    foreach (UnityEngine.Object dragged_object in DragAndDrop.objectReferences) {                        
                        list.arraySize++;
                        list.GetArrayElementAtIndex(list.arraySize - 1).FindPropertyRelative(propertiesName[0]).objectReferenceValue = dragged_object;
                    }
                }
                break;
        }
    }

    public static void DropAreaGUI(ref ReorderableList list, Rect drop_area, Event operating) {
        switch (operating.type) {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!drop_area.Contains(operating.mousePosition))
                    return;
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (operating.type == EventType.DragPerform) {
                    DragAndDrop.AcceptDrag();
                    foreach (UnityEngine.Object dragged_object in DragAndDrop.objectReferences) {
                        Debug.Log(list.list);
                        list.serializedProperty.arraySize++;
                        list.serializedProperty.GetArrayElementAtIndex(list.count - 1).objectReferenceValue = dragged_object;
                    }
                }
                break;
        }
    }
    
    public static float GUIList(ref ReorderableList list, SerializedProperty property , Rect position, GUIContent label) {
        float h_comp = 0;
        bool foldout = property.isExpanded;
        DoList(ref foldout, ref list, position, label.text, label.tooltip);
        property.isExpanded = foldout;
        
        if (list.serializedProperty.isExpanded) {
            h_comp = list.GetHeight();            
        }
        return h_comp;
    }
}
[System.Serializable]
public class ReorderableListSet {
    public readonly List<ReorderableList> setList = new List<ReorderableList>();
    public ReorderableList[] getList;
    public readonly List<SerializedProperty> setProperty = new List<SerializedProperty>();
    public SerializedProperty[] getProperty;

    public void SetList(SerializedProperty property, int index, string name, string label, string tooltip) {
        setProperty.Add(property.FindPropertyRelative(name));
        setList.Add(new ReorderableList(setProperty[index].serializedObject, setProperty[index], true, false, true, true));
        setList[index].DrawPropertyDataList(setProperty[index], label, tooltip);
        getList = setList.ToArray();
        getProperty = setProperty.ToArray();
    }
}
