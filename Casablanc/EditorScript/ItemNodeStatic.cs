using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;



[CreateAssetMenu(fileName = "新静态物品节点", menuName = "物品/静态物品节点")]
public class ItemNodeStatic : ScriptableObject, ItemNode
{
    //[Header("属性下定制")]
    public ItemStaticInfoPackage ItemStaticInfoPackage;
    public ItemPreInstanceInfoPackage ItemPreInstanceInfoPackage;

    public List<ItemNodeStatic> ItemContain;


    public Item GetItem() {
        Item item;
        if (this.ItemStaticInfoPackage.itemStaticDescribeWays == ItemStaticDescribeWays.ItemStroe) {
            item = Items.GetItemByItemTypeAndItemIDWithoutItemProperty(this.ItemStaticInfoPackage.ItemStore.ItemStaticProperties.ItemType, this.ItemStaticInfoPackage.ItemStore.ItemStaticProperties.ItemID);
        }
        else if (this.ItemStaticInfoPackage.itemStaticDescribeWays == ItemStaticDescribeWays.ItemTypeAndID) {
            item = Items.GetItemByItemTypeAndItemIDWithoutItemProperty(this.ItemStaticInfoPackage.ItemStaticProperties.ItemType, this.ItemStaticInfoPackage.ItemStaticProperties.ItemID);
        }
        else {
            item = Items.Empty;
        }
        

        if (this.ItemStaticInfoPackage.itemStaticDescribeWays== ItemStaticDescribeWays.ItemStroe) {
            ((Item_Detail)item).Info_Handler.Binding(new Item_Property(this.ItemStaticInfoPackage.ItemStore.ItemStaticProperties.ItemType, this.ItemStaticInfoPackage.ItemStore.ItemStaticProperties.ItemID, this.ItemPreInstanceInfoPackage.ItemPreInstanceProperties));
        }
        else if (this.ItemStaticInfoPackage.itemStaticDescribeWays == ItemStaticDescribeWays.ItemTypeAndID) {
            ((Item_Detail)item).Info_Handler.Binding(new Item_Property(this.ItemStaticInfoPackage.ItemStaticProperties.ItemType, this.ItemStaticInfoPackage.ItemStaticProperties.ItemID, this.ItemPreInstanceInfoPackage.ItemPreInstanceProperties));
        }

        /*预计会有个物品原型初始化*/


        return item;
    }
    public Item GetContainer() {

        Item container = this.GetItem();
        

        if (container.GetContainerState() != null) {
            for (int i = 0; i < container.GetContainerState().size; i++) {
                          
                Item item;
                if (Items.GetIsContainerByItemTypeAndItemID(this.ItemContain[i].ItemStaticInfoPackage.GetItemStaticProperty().ItemType, this.ItemContain[i].ItemStaticInfoPackage.GetItemStaticProperty().ItemID)) {
                    item = this.ItemContain[i].GetContainer();
                }
                else {
                    item = this.ItemContain[i].GetItem();
                }
                
            ((ScriptContainer)container).SetItem(i, item);
            }
        }
        return container;
    }

}




public interface ItemNode
{
    Item GetItem();
    Item GetContainer();

}
[Serializable]
public class ItemNodeDynamic: ItemNode
{
    public ItemRuntimeInfoPackage ItemRuntimeInfoPackage;
    public List<ItemNodeDynamic> ItemContain;
    public ItemNodeDynamic() { }

    public ItemNodeDynamic(Item item) {
        this.ItemRuntimeInfoPackage = new ItemRuntimeInfoPackage(item);
    }

    public Item GetItem() {
        Item item = Items.GetItemByItemTypeAndItemIDWithoutItemProperty(this.ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemType, this.ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemID);

        ((Item_Detail)item).Info_Handler.Binding(new Item_Property(this.ItemRuntimeInfoPackage.ItemRuntimeProperties));
        return item;
    }
    public Item GetContainer() {
        Item container = this.GetItem();
        if (container.GetContainerState() != null) {
            for (int i = 0; i < container.GetContainerState().size; i++) {
                Item item;
                if (Items.GetIsContainerByItemTypeAndItemID(this.ItemContain[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemType, this.ItemContain[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemID)) {
                    item = this.ItemContain[i].GetContainer();
                }
                else {
                    item = this.ItemContain[i].GetItem();
                }
            ((ScriptContainer)container).SetItem(i, item);
            }
        }
        return container;
    }

}



[CustomEditor(typeof(ItemNodeStatic))]
public class ItemNodeStaticEditor : Editor
{
    SerializedProperty ItemStaticInfoPackage;
    SerializedProperty itemStaticDescribeWays;
    SerializedProperty ItemStore;
    SerializedProperty ItemStaticProperties;
    SerializedProperty ItemPreInstanceInfoPackage;
    SerializedProperty ItemContain;

    public override void OnInspectorGUI() {
        serializedObject.Update();
        ItemNodeStatic itemNodeStatic = target as ItemNodeStatic;
        ItemStaticInfoPackage = serializedObject.FindProperty("ItemStaticInfoPackage");
        itemStaticDescribeWays = ItemStaticInfoPackage.FindPropertyRelative("itemStaticDescribeWays");
        ItemStore = ItemStaticInfoPackage.FindPropertyRelative("ItemStore");
        ItemStaticProperties = ItemStaticInfoPackage.FindPropertyRelative("ItemStaticProperties");
        ItemPreInstanceInfoPackage = serializedObject.FindProperty("ItemPreInstanceInfoPackage");
        ItemContain = serializedObject.FindProperty("ItemContain");


        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(itemStaticDescribeWays);
        if (itemNodeStatic.ItemStaticInfoPackage.itemStaticDescribeWays == ItemStaticDescribeWays.ItemStroe) {
            EditorGUILayout.PropertyField(ItemStore);
        }
        else if (itemNodeStatic.ItemStaticInfoPackage.itemStaticDescribeWays == ItemStaticDescribeWays.ItemTypeAndID) {
            EditorGUILayout.PropertyField(ItemStaticProperties.FindPropertyRelative("ItemType"));
            EditorGUILayout.PropertyField(ItemStaticProperties.FindPropertyRelative("ItemID"));
        }
        EditorGUILayout.PropertyField(ItemPreInstanceInfoPackage);
        EditorGUILayout.PropertyField(ItemContain);

        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
        }
        //base.OnInspectorGUI();
    }


}