using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEditor;




public class ItemOnTheGround : MonoBehaviour, ObjectOnTheGround
{
    Object_Values_Handler ObjectOnTheGround.Object_Values_Handler { get => itemOntheGround.Item_Values_Handler; }
    object ObjectOnTheGround.Object { get => itemOntheGround; }
    public Item itemOntheGround;



    public Instance_Type Instance_Type = Instance_Type.Single;
    public Info_Type Info_Type = Info_Type.Properties;

    [SerializeField]
    private ItemInfoStore ItemInfoStore;
    [SerializeField]
    private ItemStore ItemStore;
    [SerializeField]
    private int ItemID = -1;
    [SerializeField]
    private ItemType ItemType = ItemType.Error;

    [SerializeField]
    private ItemPreInstanceProperties.ItemPreInstanceProperties ItemPreInstanceProperties = new ItemPreInstanceProperties.ItemPreInstanceProperties();

    [SerializeField]
    private List<ItemNodeDynamic> Cotent = new List<ItemNodeDynamic>();


    public int MultiCount = 0;

    [SerializeField]
    private bool InitInScene = false;



    private MaterialPack MaterialPack;



    private void OnEnable() {
        if (this.InitInScene) {
            ItemOnTheGroundInit();
            this.itemOntheGround.Info_Handler.ReplaceInstance(this.gameObject);
        }
    }



    private void Update() {
        if (itemOntheGround != null) {
            ((ObjectOnGroundBase)this.itemOntheGround).UpdateBase();
        }

    }

    private void OnDisable() {
        if (this.Info_Type == Info_Type.InfoStore) {
            if (this.itemOntheGround.Outercontainer == null) {
                this.ItemInfoStore.Save(this.itemOntheGround);
            }
        }
        else {

        }
    }


    private void OnCollisionEnter(Collision collision) {
        ((ObjectOnGroundBase)this.itemOntheGround).CollisionEnterBase(collision);
    }
    private void OnCollisionExit(Collision collision) {
        ((ObjectOnGroundBase)this.itemOntheGround).CollisionExitBase(collision);
    }
    private void OnTriggerStay(Collider other) {
        ((ObjectOnGroundBase)this.itemOntheGround).TriggerStayBase(other);
    }

    private void ItemOnTheGroundInit() {
        if (this.Info_Type == Info_Type.InfoStore) {
            itemOntheGround = this.ItemInfoStore.GetItem();
            this.ItemType = this.itemOntheGround.Type;
            this.ItemID = this.itemOntheGround.ID;
            if (this.itemOntheGround.IsContainer) {
                ((Container)this.itemOntheGround).UpdateDisplay();
            }
        }
        else if (this.Info_Type == Info_Type.Properties) {
            this.itemOntheGround = Items.GetItemByItemTypeAndItemID_With_StaticProperties_And_PreInstanceProperties(this.ItemType, this.ItemID, this.ItemPreInstanceProperties);
            if (this.Cotent.Count > 0) {
                for (int i = 0; i < Cotent.Count; i++) {
                    if (Items.GetIsContainerByItemTypeAndItemID(Cotent[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemType, Cotent[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemID)) {
                        ((ScriptContainer)this.itemOntheGround).SetItem(i, Cotent[i].GetContainer());
                    }
                    else {
                        ((ScriptContainer)this.itemOntheGround).SetItem(i, Cotent[i].GetItem());
                    }
                }
            }
        }
        else if (this.Info_Type == Info_Type.Store) {
            this.itemOntheGround = Items.GetItemByItemTypeAndItemID_With_StaticProperties_And_PreInstanceProperties(this.ItemStore.ItemStaticProperties.ItemType, this.ItemStore.ItemStaticProperties.ItemID, this.ItemPreInstanceProperties);
            if (this.Cotent.Count > 0) {
                for (int i = 0; i < Cotent.Count; i++) {
                    ItemType itemType = Cotent[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.Detail_Info switch {
                        RuntimeProperty_Detail_Info.Properties => Cotent[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemType,
                        RuntimeProperty_Detail_Info.Store => Cotent[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemStore.ItemStaticProperties.ItemType,
                        _ => ItemType.Error,
                    };
                    int itemID = Cotent[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.Detail_Info switch {
                        RuntimeProperty_Detail_Info.Properties => Cotent[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemID,
                        RuntimeProperty_Detail_Info.Store => Cotent[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemStore.ItemStaticProperties.ItemID,
                        _ => 0,
                    };

                    if (Items.GetIsContainerByItemTypeAndItemID(itemType, itemID)) {
                        ((ScriptContainer)this.itemOntheGround).SetItem(i, Cotent[i].GetContainer());
                    }
                    else {
                        ((ScriptContainer)this.itemOntheGround).SetItem(i, Cotent[i].GetItem());
                    }
                }
            }
        }
    }

    public (ItemType, int) GetItemTypeAndItemId() {
        if (this.Info_Type == Info_Type.InfoStore) {
            return this.ItemInfoStore.GetItemTypeAndItemId();
        }
        else if (this.Info_Type == Info_Type.Properties) {
            return (this.ItemType, this.ItemID);
        }
        else if (this.Info_Type == Info_Type.Store) {
            return (ItemStore.ItemStaticProperties.ItemType, ItemStore.ItemStaticProperties.ItemID);
        }
        else {
            Debug.LogWarning("错误");
            return (ItemType.Error, 0);
        }

    }
    public ItemStore StaticItemStore {
        get {
            if(this.GetItemTypeAndItemId().Item1!= ItemType.Error) {
                return StaticPath.ItemLoad[this.GetItemTypeAndItemId().Item1, this.GetItemTypeAndItemId().Item2];
            }
            else {
                Debug.LogWarning("错误");
                return null;
            }
        }
    }

    public MaterialPack GetMaterialPack() {
        if (this.MaterialPack != null) {
            return this.MaterialPack;
        }
        else {
            MaterialPack materialPack = new MaterialPack();
            if (this.Instance_Type != Instance_Type.Muilti) {
                materialPack.materials.Add(this.transform.Find("Graph").GetComponent<Renderer>().material);
            }
            else {
                foreach (var tran in this.gameObject.transform.FindSons((Transform trans) => {
                    if (trans.TryGetComponent<ObjectLeader>(out var itemLeader)) {
                        return itemLeader.ShaderTrigger;
                    }
                    return false;
                })) {
                    materialPack.materials.Add(tran.GetComponent<Renderer>().material);
                }
            }
            this.MaterialPack = materialPack;
            return materialPack;
        }
    }
}





public class Force_Info
{
    public bool Rigid_AdjustMent = false;
    public RigidBody_Type SceneDetail_RigidBody = RigidBody_Type.ForcetoNone;
}
public enum RigidBody_Type
{
    UseDefault,
    ForcetoHave,
    ForcetoNone
}
public enum Instance_Type {
    Single,
    Muilti,
}
public enum Info_Type {
    InfoStore,
    Properties,
    Store,
}
[CustomEditor(typeof(ItemOnTheGround))]
public class ItemOnTheGroundEditor : Editor
{
    SerializedProperty Info_Type;
    SerializedProperty Instance_Type;
    SerializedProperty ItemID;
    SerializedProperty ItemType;
    SerializedProperty ItemInfoStore;
    SerializedProperty ItemPreInstanceProperties;
    SerializedProperty MultiCount;
    SerializedProperty InitInScene;
    SerializedProperty Cotent;
    SerializedProperty ItemStore;
    public override void OnInspectorGUI() {
        serializedObject.Update();
        ItemOnTheGround item = target as ItemOnTheGround;
        Info_Type = serializedObject.FindProperty("Info_Type");
        Instance_Type = serializedObject.FindProperty("Instance_Type");

        InitInScene = serializedObject.FindProperty("InitInScene");
        ItemID = serializedObject.FindProperty("ItemID");
        ItemType = serializedObject.FindProperty("ItemType");
        ItemInfoStore = serializedObject.FindProperty("ItemInfoStore");
        ItemPreInstanceProperties = serializedObject.FindProperty("ItemPreInstanceProperties");
        MultiCount = serializedObject.FindProperty("MultiCount");
        Cotent = serializedObject.FindProperty("Cotent");
        ItemStore = serializedObject.FindProperty("ItemStore");

        //base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(Info_Type);
        EditorGUILayout.PropertyField(Instance_Type);



        if (item.Info_Type== global::Info_Type.InfoStore) {
            EditorGUILayout.PropertyField(ItemInfoStore);
        }
        else if (item.Info_Type == global::Info_Type.Properties) {
            EditorGUILayout.PropertyField(ItemType);
            EditorGUILayout.PropertyField(ItemID);
            EditorGUILayout.PropertyField(ItemPreInstanceProperties);
            EditorGUILayout.PropertyField(Cotent);
        }else if(item.Info_Type== global::Info_Type.Store) {
            EditorGUILayout.PropertyField(ItemStore);
            EditorGUILayout.PropertyField(ItemPreInstanceProperties);
            EditorGUILayout.PropertyField(Cotent);
        }

        if (item.Instance_Type == global::Instance_Type.Muilti) {
            EditorGUILayout.PropertyField(MultiCount);
            if (GUILayout.Button("生成子物体", GUILayout.Width(200))) {
                this.MultiAdjustment(item);
            }
        }
        EditorGUILayout.PropertyField(InitInScene);

        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
        }
    }
    private void MultiAdjustment(ItemOnTheGround item) {
        List<int> vs = new List<int>();
        List<GameObject> Ls = new List<GameObject>();
        foreach (var Tran in FindEx.Find(item.transform, (str) => { return str.Contains("Graph"); })) {
            Ls.Add(Tran.gameObject);
        }
        for (int i = 0; i < Ls.Count; i++) {
            var num = Ls[i].name.Replace("Graph", "");
            int temp = -1;
            if (num != "") {
                temp = int.Parse(num);
            }                    
            if (temp >= item.MultiCount) {
                GameObject.DestroyImmediate(Ls[i]);
            }
            else {
                if (Ls[i].TryGetComponent<ObjectLeader>(out var itemLeader)) {
                    if (itemLeader.Target != item.gameObject) {
                        Debug.Log("ItemLeader的Target出现了非母物品的状态 请确认.");
                    }
                }
                else {
                    ObjectLeader leader = Ls[i].AddComponent<ObjectLeader>();
                    leader.Target = item.gameObject;
                    Debug.Log("为Graph" + i.ToString() + "添加了Itemleader");
                }
                vs.Add(temp);
            }
        }
        for (int i = 0; i < item.MultiCount; i++) {
            if (!vs.Contains(i)) {
                GameObject gameObject = new GameObject();
                gameObject.transform.parent = item.transform;
                gameObject.transform.position = item.transform.position;
                gameObject.name = "Graph" + i.ToString();
                ObjectLeader leader = gameObject.AddComponent<ObjectLeader>();
                leader.Target = item.gameObject;
                Debug.Log("为Graph" + i.ToString() + "添加了Itemleader");
            }
        }
    }

}


public class MaterialPack
{
    public List<Material> materials = new List<Material>();

}



