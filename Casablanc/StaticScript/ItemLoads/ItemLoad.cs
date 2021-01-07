using System.Collections;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemLoad :ScriptableObject
{
    [SerializeField]
    public List<ItemStore> itemlist;

    [HideInInspector]
    public Dictionary<KeyValuePair<ItemType, int>, ItemStore> ItemStatics;


    private void OnEnable() {
        ItemStatics = new Dictionary<KeyValuePair<ItemType, int>, ItemStore>();
        foreach (var item in itemlist) {
            if (item != null) {
                KeyValuePair<ItemType, int> key;

                key = new KeyValuePair<ItemType, int>(item.ItemStaticProperties.ItemType, item.ItemStaticProperties.ItemID);

                if (ItemStatics.TryGetValue(key, out var itemStore)) {
                    Debug.LogError("物品静态加载有重复物品!请检查!");
                }
                else {
                    ItemStatics[key] = item;
                }
            }
        }
    }
    public ItemStore this[ItemType Type, int ID] {
        get {
            KeyValuePair<ItemType, int> key = new KeyValuePair<ItemType, int>(Type, ID);
            if (ItemStatics.TryGetValue(key, out var itemStore)) {
                return itemStore;
            }
            else {
                Debug.LogError("错误的物品请求");
                return null;
            }
        }
    }
}
