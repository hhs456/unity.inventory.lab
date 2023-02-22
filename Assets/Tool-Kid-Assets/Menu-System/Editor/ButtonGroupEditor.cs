using System.Collections;
using System.Collections.Generic;
using ToolKid.EditorExtension;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
/// <summary>
/// ���и}�������s�边
/// </summary>
[CustomEditor(typeof(ButtonGroup))]
public class ButtonGroupEditor : Editor {
    ButtonGroup script;

    static bool[] foldout = new bool[3];

    ReorderableList[] list = new ReorderableList[3];
    SerializedProperty[] prop = new SerializedProperty[3];
    
    static Rect[] drapArea = new Rect[3];

    float w = 275;

    void OnEnable() {
        int foId = 0;
        prop[foId] = serializedObject.FindProperty("buttonFields");
        list[foId] = new ReorderableList(serializedObject, prop[foId], true, false, true, true);
        list[foId].DrawPropertyDataList(prop[foId], "���", "");
        foId++;
        prop[foId] = serializedObject.FindProperty("buttonStyles");
        list[foId] = new ReorderableList(serializedObject, prop[foId], true, false, false, false);
        list[foId].DrawPropertyDataList(prop[foId], "����", "");
        foId++;
        prop[foId] = serializedObject.FindProperty("pageSettings");
        list[foId] = new ReorderableList(serializedObject, prop[foId], true, false, true, true);
        list[foId].DrawClassDataList(prop[foId], "����", "", true);
    }

    public override void OnInspectorGUI() {

        serializedObject.Update();
        script = (ButtonGroup)target;
        InspectorUtility.ScriptHint(this, script, false);

        EditorGUILayout.BeginHorizontal();
        ToggleDrawer();
        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel++;
        ReorderListDrawer();
        EditorGUI.indentLevel--;
        serializedObject.ApplyModifiedProperties();
    }

    void ToggleDrawer() {        
        string label = "�}�s����";
        string tooltip = "�ثe���A '" + script.isFullScreen + "'" + '\n' + "���Φ��]�m��A�s�����i�H�M�ιq���� ESC �Τ�� Reback �\��i���^�W�@��";
        script.isFullScreen = GUILayout.Toggle(script.isFullScreen, new GUIContent(label, tooltip), EditorStyles.miniButton);
    }

    void ReorderListDrawer() {
        float h = EditorGUIUtility.singleLineHeight;
        w = EditorGUIUtility.currentViewWidth * 0.88f;

        int foId = 0;
        float fst_h = 64f;
        float leftBound = 18f;
        drapArea[foId] = new Rect(leftBound, fst_h, w, h);
        drapArea[foId].width = w;
        string label = "�������";
        string tooltip = "";
        list[foId].DoLayoutList(label, tooltip);
        prop[foId + 1].arraySize = prop[foId].arraySize;
        float sec_h = foldout[foId] ? fst_h + list[foId].GetHeight() : fst_h;
        foId++;
        drapArea[foId] = new Rect(leftBound, sec_h + h, w, h);
        label = "���䭷��";
        tooltip = "";
        list[foId].DoLayoutList(label, tooltip);        
   
        float trd_h = foldout[foId] ? sec_h + list[foId].GetHeight() : sec_h;
        foId++;
        drapArea[foId] = new Rect(leftBound, trd_h + 2 * h, w, h);
        
        label = "�����]�w";
        tooltip = "";
        list[foId].DoLayoutList(label, tooltip);        
    }
    
}
