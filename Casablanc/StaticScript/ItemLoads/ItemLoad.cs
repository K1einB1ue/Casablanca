using System.Collections;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

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
                Debug.LogError("错误的物品请求:类型" + Type.ToString() + "   ID:" + ID.ToString());
                return null;
            }
        }
    }
}


public class ItemLoadEditorWindow : EditorWindow
{


    [MenuItem("物品/整体调整窗口")]
    static void AddWindow() {
        Rect rect = new Rect(0, 0, 500, 500);
        ItemLoadEditorWindow window = (ItemLoadEditorWindow)EditorWindow.GetWindowWithRect(typeof(ItemLoadEditorWindow), rect, true, "物品整体调整窗口");
        window.Show();
    }

    private void OnGUI() {
        for(int i=0;i< Selection.objects.Length; i++) {
            if(Selection.objects[i] is ItemStore) {
                ItemStore itemStore = (ItemStore)Selection.objects[i];
                GUILayout.Label("物品名:" + itemStore.name);
                GUILayout.Label("物品运行时映射状态:");
                for(int j=0;j< itemStore.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.RuntimePacks.Count; j++) {
                    Context_Pack context_Pack = itemStore.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.RuntimePacks[j];
                    GUILayout.Label("属性名:" + context_Pack.PropertyName + "  数据类型:" + context_Pack.___Data.ToString() + "  数据位置:" + context_Pack.PosInList.ToString());
                }
                GUILayout.Label("物品静态时映射状态:");
                for (int j = 0; j < itemStore.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.StaticPacks.Count; j++) {
                    Context_Pack context_Pack = itemStore.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.StaticPacks[j];
                    GUILayout.Label("属性名:" + context_Pack.PropertyName + "  数据类型:" + context_Pack.___Data.ToString() + "  数据位置:" + context_Pack.PosInList.ToString());
                }
                GUILayout.Space(20);
            }
        }

        if (GUILayout.Button("重置选中物品加载区")) {

        }

        if(GUILayout.Button("重置全部物品动态加载区")) {
            foreach(var load in StaticPath.ItemLoad.itemlist) {
                if (load != null) {
                    if (load.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping != null) {
                        load.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping = new ItemContextMapping();
                    }
                    if (load.ItemStaticProperties.ItemStaticValues.ItemStaticContext != null) {
                        load.ItemStaticProperties.ItemStaticValues.ItemStaticContext = new ItemStaticProperties.ItemStaticContext();
                    }
                }
            }
            Debug.Log("重置成功!");
        }
    }
}

public class GameObjectEditorWindow : EditorWindow
{
    public static string x = "X", y = "Y", z = "Z";
    public static string Name = "Empty";
    [MenuItem("变换/子物品调整窗口")]
    static void AddWindow() {
        Rect rect = new Rect(0, 0, 500, 500);
        GameObjectEditorWindow window = (GameObjectEditorWindow)EditorWindow.GetWindowWithRect(typeof(GameObjectEditorWindow), rect, true, "物品整体调整窗口");
        window.Show();
    }

    private void OnGUI() {
        x = GUILayout.TextField(x);
        y = GUILayout.TextField(y);
        z = GUILayout.TextField(z);
        if (GUILayout.Button("变换")) {
            if(Selection.activeObject is GameObject) {
                foreach (var tran in ((GameObject)Selection.activeObject).transform.GetSons()) {
                    Vector3 temp = new Vector3();
                    temp.x = tran.position.x + float.Parse(x);
                    temp.y = tran.position.x + float.Parse(y);
                    temp.z = tran.position.x + float.Parse(z);
                    tran.position = temp;
                }
            }          
        }
        Name = GUILayout.TextField(Name);
        if (GUILayout.Button("更改名字")) {
            if (Selection.activeObject is GameObject) {
                foreach (var tran in ((GameObject)Selection.activeObject).transform.GetSons()) {
                    tran.name = Name;
                }
            }
        }
    }
}