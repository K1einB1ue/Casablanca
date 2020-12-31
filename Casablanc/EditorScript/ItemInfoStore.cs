using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "新存储", menuName = "物品/动静态混合存储")]
public class ItemInfoStore : ScriptableObject
{
    public ItemNodeStatic itemNodesS;
    public ItemNodeDynamic itemNodesD;
    public bool init = false;
    public Container GetContainer() {
        if (!init) {
            init = true;
            return (Container)this.itemNodesS.GetContainer();
        }
        else {
            return (Container)this.itemNodesD.GetContainer();
        }
    }

    public Item GetItem() {
        if (!init) {
            init = true;
            return this.itemNodesS.GetContainer();
        }
        else {
            return this.itemNodesD.GetContainer();      
        }
    }
    public Container GetNonSaveContainer() {
        return (Container)this.itemNodesS.GetContainer();
    }
    public Item GetNonSaveItem() {
        return this.itemNodesS.GetContainer();
    }
    public void StoreContainer(ItemNodeDynamic itemNodeDynamic) {
        this.itemNodesD = itemNodeDynamic;
        EditorUtility.SetDirty(this);
    }
    public void StoreContainer(Player player) {
        this.itemNodesD=player.ItemInfoPackup();
        EditorUtility.SetDirty(this);
    }



}


[CustomEditor(typeof(ItemInfoStore))]
public class ItemInfoStoreEditor : Editor
{
    SerializedProperty itemNodesS;
    SerializedProperty itemNodesD;
    SerializedProperty init;
    public override void OnInspectorGUI() {
        serializedObject.Update();
        ItemInfoStore itemInfoStore = target as ItemInfoStore;
        itemNodesS = serializedObject.FindProperty("itemNodesS");
        itemNodesD = serializedObject.FindProperty("itemNodesD");
        init = serializedObject.FindProperty("init");
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(itemNodesS);
        if (!itemInfoStore.itemNodesS||itemInfoStore.init) {
            EditorGUILayout.PropertyField(itemNodesD);
            
        }

        EditorGUILayout.PropertyField(init);
        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
        }
        //base.OnInspectorGUI();
    }
}