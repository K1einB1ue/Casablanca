using System.Collections;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="物品静态加载",menuName ="静态加载/物品静态加载包")]
public class ItemLoad :ScriptableObject
{
    [SerializeField]
    public List<ItemStore> itemlist;

    [HideInInspector]
    public Dictionary<KeyValuePair<ItemType, int>, ItemStore> ItemStatics;

    public bool initital = false;

    private void Awake() {
        initital = false;
    }





    public ItemStore this[ItemType Type, int ID]
    {
        get {
            if (!this.initital) {
                this.initital = true;
                try {
                    ItemStatics = new Dictionary<KeyValuePair<ItemType, int>, ItemStore>();

                    for (int i = 0; i < itemlist.Count; i++) {
                        if (itemlist[i] != null) {
                            KeyValuePair<ItemType, int> Pair = new KeyValuePair<ItemType, int>(itemlist[i].ItemStaticProperties.ItemType, itemlist[i].ItemStaticProperties.ItemID);
                            if (ItemStatics.TryGetValue(Pair, out ItemStore item)) {
                                Debug.LogError(itemlist[i].name + "与" + itemlist[i].name + "物品冲突!");
                            }
                            else {
                                ItemStatics.Add(Pair, itemlist[i]);
                            }
                        }
                    }
                }
                catch (NullReferenceException) {
                    Debug.Log("Type:" + Type.ToString() + "/ID:" + ID.ToString());
                    return null;
                }

            }
            KeyValuePair<ItemType, int> keyValuePair = new KeyValuePair<ItemType, int>(Type, ID);
            if(ItemStatics.TryGetValue(keyValuePair,out ItemStore itemStore)){
                return itemStore;
            }
            else {
                Debug.LogError("在ItemLoad中无法取出该物品:Type=" + Type.ToString() + "/ID=" + ID.ToString());
                return null;
            }



        }


    }

}