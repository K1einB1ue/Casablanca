using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "新物品", menuName = "物品/新物品")]
public class ItemStore : ScriptableObject
{

    public ItemStaticProperties.ItemStaticProperties ItemStaticProperties;

}

//[CustomEditor(typeof(ItemStore))]
//public class ItemStoreEditor : Editor
//{
//    public override void OnInspectorGUI() {
//        var ele = serializedObject.FindProperty("ItemStaticProperties").FindPropertyRelative("ItemStaticValues").FindPropertyRelative("StaticValues_Element");


//        EditorGUI.BeginChangeCheck();
//        EditorGUILayout.PropertyField(ele);

//        if (EditorGUI.EndChangeCheck()) {
//            serializedObject.ApplyModifiedProperties();
//        }
//    }
//}