using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// 樂譜腳本介面編輯器
/// </summary>

[CustomPropertyDrawer(typeof(PageSet))]
public class PageSetDrawer : PropertyDrawer
{
    const float elementCount = 5.5f;

    ReorderableListSet stylesSet = new ReorderableListSet();
    ReorderableListSet objectsSet = new ReorderableListSet();

    int index = 0;

    void OnEnable(SerializedProperty property) {
        if (property.FindPropertyRelative("enable").boolValue == false) {
            property.FindPropertyRelative("enable").boolValue = true;
            if(property.FindPropertyRelative("index").intValue == 0)
                property.FindPropertyRelative("index").intValue = index;
            stylesSet.SetList(property, index, "styles", "風格", "");
            objectsSet.SetList(property, index, "objects", "物件", "");
            Debug.Log("Init " + index);
            index++;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {        
        float h = property.isExpanded ? EditorGUIUtility.singleLineHeight * elementCount + property.FindPropertyRelative("compensation").floatValue : EditorGUIUtility.singleLineHeight;
        return h;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        EditorGUI.BeginProperty(position, label, property);

        property.serializedObject.Update();

        float singleHeight = EditorGUIUtility.singleLineHeight;
        var nameRect = new Rect(position.x, position.y, position.width, singleHeight);
        if(stylesSet == null) {
            stylesSet = new ReorderableListSet();
            objectsSet = new ReorderableListSet();
        }
        
        if (property.FindPropertyRelative("index").intValue >= stylesSet.setList.Count) {
            property.FindPropertyRelative("enable").boolValue = false;
            Debug.Log("Reset Enable");
        }
        OnEnable(property);
        
        property.isExpanded = EditorGUI.Foldout(nameRect, property.isExpanded, property.FindPropertyRelative("description").stringValue);        
        if (property.isExpanded) {
            
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            {
                #region Draw Property Field
                var rect = new Rect(position.x, position.y += singleHeight, position.width, singleHeight);
                property.FindPropertyRelative("compensation").floatValue = 0;
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("description"), new GUIContent("描述"));
                rect = new Rect(position.x, position.y += singleHeight, position.width, singleHeight);
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("changeObject"), new GUIContent("切換物件"));
                #endregion

                #region Draw Reorderable List
                int listId = property.FindPropertyRelative("index").intValue;               
                if (!property.FindPropertyRelative("changeObject").boolValue) {                    
                    rect = new Rect(position.x, position.y += singleHeight, position.width, singleHeight);
                    EditorGUI.PropertyField(rect, property.FindPropertyRelative("Base"), new GUIContent("頁面基底"));
                    rect = new Rect(position.x, position.y += singleHeight * 1.25f, position.width, singleHeight);                    
                    property.FindPropertyRelative("compensation").floatValue += Inspector.GUIList(ref stylesSet.getList[listId], stylesSet.getProperty[listId], rect, new GUIContent("風格"));                    
                }
                else {                    
                    rect = new Rect(position.x, position.y += singleHeight * 1.25f + property.FindPropertyRelative("compensation").floatValue, position.width, singleHeight);
                    property.FindPropertyRelative("compensation").floatValue += Inspector.GUIList(ref objectsSet.getList[listId], objectsSet.getProperty[listId], rect, new GUIContent("物件"));
                    property.FindPropertyRelative("compensation").floatValue -= EditorGUIUtility.singleLineHeight;
                }
                #endregion

                property.serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.indentLevel = indent;
        }
        EditorGUI.EndProperty();
    }
}
