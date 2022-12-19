using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKid.GUIToolkit {
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelAttributeDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var attribute = this.attribute as LabelAttribute;
            label.text = attribute.text;
            string[] pos = property.propertyPath.Split('[', ']');
            string[] path = pos[0].Split('.');            
            string i = "";
            if (path[path.Length - 1] == "data") {
                i = " " + pos[1];                
            }
            else if (property.hasChildren) {
                Debug.LogWarning("It is disallow to apply 'LabelAttribute' on 'class'.");
            }
            EditorGUI.PropertyField(position, property, new GUIContent(label.text + i, label.tooltip));
        }
    }
}
