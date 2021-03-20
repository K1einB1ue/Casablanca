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
        //获取类
        if (this.ItemStaticInfoPackage.itemStaticDescribeWays == ItemStaticDescribeWays.ItemStroe) {
            item = Items.GetItemByItemTypeAndItemIDWithoutItemProperty(this.ItemStaticInfoPackage.ItemStore.ItemStaticProperties.ItemType, this.ItemStaticInfoPackage.ItemStore.ItemStaticProperties.ItemID);
        }
        else if (this.ItemStaticInfoPackage.itemStaticDescribeWays == ItemStaticDescribeWays.ItemTypeAndID) {
            item = Items.GetItemByItemTypeAndItemIDWithoutItemProperty(this.ItemStaticInfoPackage.ItemStaticProperties.ItemType, this.ItemStaticInfoPackage.ItemStaticProperties.ItemID);
        }
        else {
            item = Items.Empty;
        }

        //对类值进行初始化工作
        if (this.ItemStaticInfoPackage.itemStaticDescribeWays== ItemStaticDescribeWays.ItemStroe) {
            item.Info_Handler.Binding(new Item_Property(this.ItemStaticInfoPackage.ItemStore.ItemStaticProperties.ItemType, this.ItemStaticInfoPackage.ItemStore.ItemStaticProperties.ItemID, this.ItemPreInstanceInfoPackage.ItemPreInstanceProperties));
        }
        else if (this.ItemStaticInfoPackage.itemStaticDescribeWays == ItemStaticDescribeWays.ItemTypeAndID) {
            item.Info_Handler.Binding(new Item_Property(this.ItemStaticInfoPackage.ItemStaticProperties.ItemType, this.ItemStaticInfoPackage.ItemStaticProperties.ItemID, this.ItemPreInstanceInfoPackage.ItemPreInstanceProperties));
        }


        //对类实例个体进行深入初始化工作
        /*预计会有个物品原型初始化*/


        return item;
    }
    public (ItemType, int) GetItemTypeAndItemId() {
        if (this.ItemStaticInfoPackage.itemStaticDescribeWays == ItemStaticDescribeWays.ItemStroe) {
            return (this.ItemStaticInfoPackage.ItemStore.ItemStaticProperties.ItemType, this.ItemStaticInfoPackage.ItemStore.ItemStaticProperties.ItemID);
        }
        else if (this.ItemStaticInfoPackage.itemStaticDescribeWays == ItemStaticDescribeWays.ItemTypeAndID) {
            return (this.ItemStaticInfoPackage.ItemStaticProperties.ItemType, this.ItemStaticInfoPackage.ItemStaticProperties.ItemID);
        }
        else {
            Debug.LogWarning("错误");
            return (ItemType.Error, 0);
        }
    }
    public Item GetContainer() {

        Item container = this.GetItem();
        

        if (container.GetContainerState() != null) {
            if (ItemContain != null) {
                for (int i = 0; i < Mathf.Min(container.GetContainerState().size,ItemContain.Count); i++) {

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

      
        Item item = Items.GetItemByItemTypeAndItemIDWithoutItemProperty(this.GetItemTypeAndItemId().Item1,this.GetItemTypeAndItemId().Item2/*this.ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemType, this.ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemID*/);
        item.Info_Handler.Binding(new Item_Property(this.ItemRuntimeInfoPackage.ItemRuntimeProperties));
        return item;
    }
    public Item GetContainer() {
        Item container = this.GetItem();
        if (container.GetContainerState() != null) {
            if (ItemContain!=null) { 
                for (int i = 0; i < Mathf.Min(container.GetContainerState().size,ItemContain.Count); i++) {
                    Item item;

                    ItemType itemType = ItemContain[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.Detail_Info switch
                    {
                        RuntimeProperty_Detail_Info.Properties => ItemContain[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemType,
                        RuntimeProperty_Detail_Info.Store => ItemContain[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemStore.ItemStaticProperties.ItemType,
                        _ => ItemType.Error,
                    };
                    int itemID = ItemContain[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.Detail_Info switch
                    {
                        RuntimeProperty_Detail_Info.Properties => ItemContain[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemID,
                        RuntimeProperty_Detail_Info.Store => ItemContain[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemStore.ItemStaticProperties.ItemID,
                        _ => 0,
                    };

                    if (Items.GetIsContainerByItemTypeAndItemID(itemType,itemID/*this.ItemContain[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemType, this.ItemContain[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemID*/)) {
                        item = this.ItemContain[i].GetContainer();
                    }
                    else {
                        item = this.ItemContain[i].GetItem();
                    }
                ((ScriptContainer)container).SetItem(i, item);
                }
            }
        }
        return container;
    }
    public (ItemType, int) GetItemTypeAndItemId() {
        if(ItemRuntimeInfoPackage.ItemRuntimeProperties.Detail_Info== RuntimeProperty_Detail_Info.Store&& ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemStore == null) {
            return (ItemType.Empty, 0);
        }
        ItemType itemType = ItemRuntimeInfoPackage.ItemRuntimeProperties.Detail_Info switch {
            RuntimeProperty_Detail_Info.Properties => ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemType,
            RuntimeProperty_Detail_Info.Store => ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemStore.ItemStaticProperties.ItemType,
            _ => ItemType.Error,
        };
        int itemID = ItemRuntimeInfoPackage.ItemRuntimeProperties.Detail_Info switch {
            RuntimeProperty_Detail_Info.Properties => ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemID,
            RuntimeProperty_Detail_Info.Store => ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemStore.ItemStaticProperties.ItemID,
            _ => 0,
        };
        return (itemType, itemID);
    }
}

public enum ItemStaticDescribeWays
{
    ItemStroe,
    ItemTypeAndID,
    ItemDetailStore,
    ItemDetailPack,

}

[Serializable]
public class ItemStaticInfoPackage
{
    public ItemStaticDescribeWays itemStaticDescribeWays = ItemStaticDescribeWays.ItemStroe;
    public ItemStore ItemStore;
    public ItemStaticProperties.ItemStaticProperties ItemStaticProperties;

    public ItemStaticProperties.ItemStaticProperties GetItemStaticProperty() {
        if (this.itemStaticDescribeWays == ItemStaticDescribeWays.ItemStroe) {
            return StaticPath.ItemLoad[this.ItemStore.ItemStaticProperties.ItemType, this.ItemStore.ItemStaticProperties.ItemID].ItemStaticProperties;
        }
        if (this.itemStaticDescribeWays == ItemStaticDescribeWays.ItemTypeAndID) {
            return this.ItemStaticProperties;
        }
        return null;
    }
}
[Serializable]
public class ItemRuntimeInfoPackage
{

    public ItemRuntimeProperties.ItemRuntimeProperties ItemRuntimeProperties;

    public ItemRuntimeInfoPackage(Item item) {
        ItemRuntimeProperties = item.Info_Handler.Item_Property.ItemRuntimeProperties;
    }
}
[Serializable]
public class ItemPreInstanceInfoPackage
{
    public ItemPreInstanceType ItemPreInstanceType = ItemPreInstanceType.Standard;
    public ItemPreInstanceProperties.ItemPreInstanceProperties ItemPreInstanceProperties = new ItemPreInstanceProperties.ItemPreInstanceProperties();
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