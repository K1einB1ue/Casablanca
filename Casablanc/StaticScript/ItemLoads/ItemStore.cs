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

[CustomEditor(typeof(ItemStore))]
public class ItemStoreEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        ItemStore itemStore = ((ItemStore)target);
        if (itemStore.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.Inited) {
            if (GUILayout.Button("重载静态存储映射")) {
                itemStore.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.Inited = false;
                EditorUtility.SetDirty(itemStore);
            }
        }
    }
}

