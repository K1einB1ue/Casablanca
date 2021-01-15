using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EditorEx
{
  
    private static Rect GetNormalRect(Rect Position) {
        Rect ret = new Rect(Position)
        {
            width = 250,
            y = Position.y + EditorGUIUtility.singleLineHeight + 2,
        };
        return ret;
    }
    private static Rect GetHeightedRect(SerializedProperty property,Rect Position) {
        Rect ret = new Rect(Position)
        {
            width = 250,
            y = Position.y + EditorGUI.GetPropertyHeight(property),
        };
        return ret;
    }
    public static Rect NewProperty(Rect PositionBefore,SerializedProperty property,GUIContent content) {
        Rect ret = GetNormalRect(PositionBefore);
        EditorGUI.PropertyField(ret, property, content, true);
        return ret;
    }
    public static Rect NewProperty(Rect PositionBefore, SerializedProperty property) {
        Rect ret = GetNormalRect(PositionBefore);
        EditorGUI.PropertyField(ret, property, true);
        return ret;
    }
    public static Rect NewPropertyGroup(this SerializedProperty serializedProperty,Rect PositionBefore,string[] Fieldname) {
        Rect rect = PositionBefore;
        for (int i = 0; i < Fieldname.Length; i++) {
            SerializedProperty temp = serializedProperty.FindPropertyRelative(Fieldname[i]);
            if (i != 0) {
                rect = GetHeightedRect(temp, rect);
            }
            EditorGUI.PropertyField(rect, temp, true);
        }
        return rect;
    }
    public static Rect NewPropertyGroup(Rect PositionBefore, SerializedProperty[] property, GUIContent[] content) {
        for (int i = 0; i < property.Length; i++) {
            PositionBefore = NewProperty(PositionBefore, property[i], content[i]);
        }
        return PositionBefore;
    }
    public static Rect NewPropertyGroup(Rect PositionBefore, SerializedProperty[] property) {
        for (int i = 0; i < property.Length; i++) {
            PositionBefore = NewProperty(PositionBefore, property[i]);
        }
        return PositionBefore;
    }
    public static Rect Show(Rect PositionBefore, string Info) { 
        Rect ret = GetNormalRect(PositionBefore);
        EditorGUI.LabelField(ret, Info);
        return ret;

    }

    public static float HeightSum(this SerializedProperty property,string[] Fieldname) {
        float height = 0;
        for(int i = 0; i < Fieldname.Length; i++) {
            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(Fieldname[i]));
        }
        return height;
    }
}
