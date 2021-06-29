using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class EditorEx
{
    public static Rect GetStartRect(Rect Position) {
        Rect ret = new Rect(Position) {
            width = 500,
            height = EditorGUIUtility.singleLineHeight + 2,
            y = Position.y - EditorGUIUtility.singleLineHeight - 2,
        };
        return ret;
    }
    public static Rect GetNormalRect(Rect Position) {
        Rect ret = new Rect(Position) {
            width = 500,
            height = EditorGUIUtility.singleLineHeight + 2,
            y = Position.y + EditorGUIUtility.singleLineHeight + 2,
        };
        return ret;
    }
    public static Rect GetHeightedRect(SerializedProperty property,Rect Position) {
        Rect ret = new Rect(Position) {
            width = 500,
            height = EditorGUIUtility.singleLineHeight + 2,
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
    public static float NormalHeightByNum(int Size) {
        return (EditorGUIUtility.singleLineHeight + 2) * Size;
    }


    public static Rect CustomIntProperty(Rect PositionBefore, SerializedProperty property,GUIContent GUIContent) {
        Rect ret = GetNormalRect(PositionBefore);
        using (var Scope= new EditorGUI.ChangeCheckScope()) {
            EditorGUI.LabelField(PositionBefore, GUIContent);

            Rect IntRect = new Rect() {
                y = PositionBefore.y,
                xMin = PositionBefore.x + 200,
                xMax = 250,
                width = PositionBefore.width,
                height = PositionBefore.height,
            };

            var newint = EditorGUI.IntField(IntRect, property.intValue);
            if (Scope.changed) {
                property.intValue = newint;
            }
        }
        return ret;
    }
    public static Rect CustomFloatProperty(Rect PositionBefore, SerializedProperty property, GUIContent GUIContent) {
        Rect ret = GetNormalRect(PositionBefore);
        using (var Scope = new EditorGUI.ChangeCheckScope()) {
            EditorGUI.LabelField(PositionBefore, GUIContent);

            Rect floatRect = new Rect() {
                y = PositionBefore.y,
                xMin = PositionBefore.x + 200,
                xMax = 250,
                width = PositionBefore.width,
                height = PositionBefore.height,
            };

            var newfloat = EditorGUI.FloatField(floatRect, property.floatValue);
            if (Scope.changed) {
                property.floatValue = newfloat;
            }
        }
        return ret;
    }

    public static Rect CustomBoolProperty(Rect PositionBefore, SerializedProperty property,GUIContent GUIContent) {
        Rect ret = GetNormalRect(PositionBefore);
        using (var Scope = new EditorGUI.ChangeCheckScope()) {
            EditorGUI.LabelField(PositionBefore, GUIContent);

            Rect BoolRect = new Rect() {
                y = PositionBefore.y,
                x = PositionBefore.x + 200,
                width = 60,
                height = PositionBefore.height,
            };

            var newbool = EditorGUI.Toggle(BoolRect, property.boolValue);
            if (Scope.changed) {
                property.boolValue = newbool;
            }
        }
        return ret;
    }

    public static Rect CustEnumProperty(Rect PositionBefore,SerializedProperty property,GUIContent GUIContent) {
        Rect ret = GetNormalRect(PositionBefore);
        using (var Scope = new EditorGUI.ChangeCheckScope()) {
            var Fields = Type.GetType(property.FindPropertyRelative("First").stringValue).GetFields(BindingFlags.Static | BindingFlags.Public);
            var NumStore = property.FindPropertyRelative("Second");
            int Select = -1;
            List<string> Enum = new List<string>();
            Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();
            for(int i = 0; i < Fields.Length; i++) {
                Enum.Add(Fields[i].Name);                
                if ((int)Fields[i].GetValue(null) == NumStore.intValue) {                   
                    Select = i;                   
                }
                keyValuePairs.Add(i, (int)Fields[i].GetValue(null));
            }
            EditorGUI.LabelField(PositionBefore, GUIContent);

            Rect EnumRect = new Rect() {
                y = PositionBefore.y,
                x = PositionBefore.x + 100,
                width = EditorGUIUtility.currentViewWidth-200,
                height = PositionBefore.height,
            };
            Select = EditorGUI.Popup(EnumRect, Select, Enum.ToArray());

            if (Scope.changed) {
                NumStore.intValue = keyValuePairs[Select];
            }
        }
        return ret;
    }


    public static object GetObjectByPath(object target, string Path, int Ignore = 0) {
        string[] Pathes = Path.Split('.');
        Type type_tmp = target.GetType();
        object obj_tmp = target;
        bool Array = false;
        IEnumerable List = null;

        for (int i = 0; i < Pathes.Length - Ignore; i++) {
            if (Pathes[i] == "Array") {
                Array = true;
                List = ((IEnumerable)obj_tmp);
            }
            else if (Array) {
                int pos = int.Parse(Pathes[i].Replace("data[", "").Replace("]", ""));
                var ptr = List.GetEnumerator();
                for (int j = 0; j <= pos; j++) {
                    ptr.MoveNext();
                }
                obj_tmp = ptr.Current;
                type_tmp = obj_tmp.GetType();
                Array = false;
            }
            else {
                var info = type_tmp.GetField(Pathes[i], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                obj_tmp = info.GetValue(obj_tmp);
                type_tmp = obj_tmp.GetType();
            }
        }
        return obj_tmp;
    }
}
