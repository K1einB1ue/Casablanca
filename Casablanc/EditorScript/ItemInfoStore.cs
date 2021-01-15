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
    public bool save = false;
    public bool init = false;
    public Container GetContainer() {
        if (save) {
            if (!init) {
                init = true;
                return (Container)this.itemNodesS.GetContainer();
            }
            else {
                return (Container)this.itemNodesD.GetContainer();
            }
        }
        else {
            return (Container)this.itemNodesS.GetContainer();
        }
    }

    public Item GetItem() {
        if (save) {
            if (!init) {
                init = true;
                return this.itemNodesS.GetContainer();
            }
            else {
                return this.itemNodesD.GetContainer();
            }
        }
        else {
            return this.itemNodesS.GetContainer();
        }
    }
    public Container GetNonSaveContainer() {
        return (Container)this.itemNodesS.GetContainer();
    }
    public Item GetNonSaveItem() {
        return this.itemNodesS.GetContainer();
    }

    public void Save(Item item) {
        ItemNodeDynamic node = null;
        if (item.IsContainer) {
            node = new ItemNodeDynamic((Container)item);
            node.ItemContain = ((Container)node).GetItemNodes();
        }
        else {
            node = new ItemNodeDynamic(item);
        }
        this.itemNodesD = node;
        EditorUtility.SetDirty(this);
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
    SerializedProperty save;
    SerializedProperty init;
    public override void OnInspectorGUI() {
        serializedObject.Update();
        ItemInfoStore itemInfoStore = target as ItemInfoStore;
        itemNodesS = serializedObject.FindProperty("itemNodesS");
        itemNodesD = serializedObject.FindProperty("itemNodesD");
        init = serializedObject.FindProperty("init");
        save = serializedObject.FindProperty("save");
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(save);
        EditorGUILayout.PropertyField(itemNodesS);
        if (itemInfoStore.save) {
            if (!itemInfoStore.itemNodesS || itemInfoStore.init) {
                EditorGUILayout.PropertyField(itemNodesD);
            }
            EditorGUILayout.PropertyField(init);
        }
        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
        }
        //base.OnInspectorGUI();
    }
}