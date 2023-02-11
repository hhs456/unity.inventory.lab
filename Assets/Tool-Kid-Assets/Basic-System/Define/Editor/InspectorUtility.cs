using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public static class InspectorUtility {

    public static readonly float space = 2f;
    public static readonly float width = EditorGUIUtility.currentViewWidth;
    public static Rect DropArea = new Rect();

    public static bool ScriptHint(this Editor editor, bool foldout) {
        float w = GUILayoutUtility.GetLastRect().width;
        if (foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, "µ{¦¡½X CSharp")) {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("½s¿è GUI Editor", MonoScript.FromScriptableObject(editor), typeof(Editor), true);
            EditorGUILayout.ObjectField("±±¨î Behaviour", MonoScript.FromMonoBehaviour(editor.target as MonoBehaviour), typeof(Editor), true);
            EditorGUI.EndDisabledGroup();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        return foldout;
    }

    public static void ScriptHint(Editor editor, MonoBehaviour script, bool inheritBase) {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Editor", MonoScript.FromScriptableObject(editor), script.GetType(), true);
        if (!inheritBase) {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(script), script.GetType(), true);
        }
        EditorGUI.EndDisabledGroup();
    }
    public static void ScriptHint(Editor editor, ScriptableObject script, bool inheritBase) {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Editor", MonoScript.FromScriptableObject(editor), script.GetType(), true);
        if (!inheritBase)
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(script), script.GetType(), true);
        EditorGUI.EndDisabledGroup();
    }

    public static void DoLayoutList(this ReorderableList list, string label, string tooltip) {
        EditorGUILayout.Space(0f);
        Rect rect = GUILayoutUtility.GetLastRect();
        float size_w = EditorGUIUtility.fieldWidth; // default 48f
        rect.width = rect.width - size_w + 8f;
        rect.height = EditorGUIUtility.singleLineHeight;
        DropAreaGUI(ref list, rect, Event.current);
        list.serializedProperty.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(rect, list.serializedProperty.isExpanded, new GUIContent(label, tooltip));
        EditorGUILayout.EndFoldoutHeaderGroup();
        rect.x += rect.width - 8f;
        rect.width = size_w;
        list.serializedProperty.arraySize = EditorGUI.IntField(rect, list.serializedProperty.arraySize);
        rect.y += EditorGUIUtility.singleLineHeight + 2f;                
        EditorGUILayout.Space(rect.y);
        if (list.serializedProperty.isExpanded) {            
            list.DoLayoutList();
            EditorGUILayout.Space(2f);
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
            arect.y = rect.y + 1f;
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
            arect.height = EditorGUI.GetPropertyHeight(reorderableList.serializedProperty.GetArrayElementAtIndex(index));
            arect.x = rect.x + 12;
            arect.width = rect.width - rect.width / 10;
            EditorGUI.PropertyField(arect, reorderableList.serializedProperty.GetArrayElementAtIndex(index), true);
        };
        reorderableList.elementHeightCallback = (index) => EditorGUI.GetPropertyHeight(props.GetArrayElementAtIndex(index));                
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

    public static void OnDrop<T>(this Rect drop_area, Event operating, Predicate<T> match, Action<T> action) {
        switch (operating.type) {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (drop_area.Contains(operating.mousePosition)) {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    if (operating.type == EventType.DragPerform) {
                        DragAndDrop.AcceptDrag();
                        T[] ts = DragAndDrop.objectReferences as T[];
                        Array.ForEach(ts, t => {
                            if (match(t)) {
                                action.Invoke(t);
                            }
                        });
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
                        int final = list.count - 1;
                        list.serializedProperty.InsertArrayElementAtIndex(final);                        
                        list.serializedProperty.GetArrayElementAtIndex(final + 1).objectReferenceValue = dragged_object;
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
