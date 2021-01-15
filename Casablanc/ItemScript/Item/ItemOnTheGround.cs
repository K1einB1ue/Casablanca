using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEditor;




public class ItemOnTheGround : MonoBehaviour
{
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

    private bool TypeIDGive =false;


    private MaterialPack materialPack;

    [HideInInspector]
    public bool selected = false;


    private void Awake() {
        if (materialPack == null) {
            materialPack = new MaterialPack();
        }
        if (this.Instance_Type != Instance_Type.Muilti) {
            materialPack.materials.Add(this.transform.Find("Graph").GetComponent<Renderer>().material);
        }
        else {           
            for(int i=0;i< MultiCount; i++) {
                //materialPack.materials.Add(this.transform.Find("Graph" + i.ToString()).GetComponent<Renderer>().material);
            }
        }   
    }



    private void OnEnable() {
        if (this.InitInScene) {
            ItemOnTheGroundInit();
            this.itemOntheGround.Info_Handler.ReplaceInstance(this.gameObject);
            this.TypeIDGive = true;
        }   
    }



    private void Update() {
        if (itemOntheGround != null) {
            itemOntheGround.update();
        }
        if (Instance_Type != Instance_Type.Muilti) {
            materialPack.Update(this.itemOntheGround, this.selected);
        }
    }



    private void OnCollisionEnter(Collision collision) {
        ((ItemOnGroundBase)this.itemOntheGround).CollisionBase(collision);
    }
    private void OnTriggerStay(Collider other) {
        ((ItemOnGroundBase)this.itemOntheGround).TriggerBase(other);
    }

    private void ItemOnTheGroundInit() {
        if (this.Info_Type == Info_Type.InfoStore) {
            itemOntheGround = this.ItemInfoStore.GetNonSaveContainer();
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
                    ItemType itemType = Cotent[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.Detail_Info switch
                    {
                        RuntimeProperty_Detail_Info.Properties => Cotent[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemType,
                        RuntimeProperty_Detail_Info.Store => Cotent[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemStore.ItemStaticProperties.ItemType,
                        _ => ItemType.Error,
                    };
                    int itemID = Cotent[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.Detail_Info switch
                    {
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
            int temp = int.Parse(Ls[i].name.Replace("Graph", ""));
            if (temp >= item.MultiCount) {
                GameObject.DestroyImmediate(Ls[i]);
            }
            else {
                if (Ls[i].TryGetComponent<ItemLeader>(out var itemLeader)) {
                    if (itemLeader.Target != item.gameObject) {
                        Debug.LogWarning("ItemLeader的Target出现了非母物品的状态 请确认.");
                    }
                }
                else {
                    ItemLeader leader = Ls[i].AddComponent<ItemLeader>();
                    leader.Target = item.gameObject;
                    Debug.LogWarning("为Graph" + i.ToString() + "添加了Itemleader");
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
                ItemLeader leader = gameObject.AddComponent<ItemLeader>();
                leader.Target = item.gameObject;
                Debug.LogWarning("为Graph" + i.ToString() + "添加了Itemleader");
            }
        }
    }

}


[Serializable]
public class MaterialPack
{
    public List<Material> materials = new List<Material>();

    public void Update(Item itemOntheGround,bool selected) {
        for(int i = 0; i < materials.Count; i++) {
            materials[i].SetFloat("CantGet", itemOntheGround.Item_Status_Handler.GetWays == GetWays.Hand ? 0 : 1);
            materials[i].SetFloat("Rate", 1.0f - itemOntheGround.Item_UI_Handler.GetHPrate());
            if (KeyPress.P || selected) {
                materials[i].SetFloat("Boolean_590330D2", 1.0f);
            }
            else {
                materials[i].SetFloat("Boolean_590330D2", 0.0f);
            }
        }
    }
}

