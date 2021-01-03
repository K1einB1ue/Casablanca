using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEditor;

[AttributeUsage(AttributeTargets.Class)]
public class ItemAttribute : Attribute {
    public ItemType ItemType;
    public int ItemID;
    public bool enable = true;
    public ItemAttribute(ItemType itemType, int itemID) {
        this.ItemType = itemType;
        this.ItemID = itemID;
    }
    public ItemAttribute(ItemType itemType, int itemID,bool enable) {
        this.ItemType = itemType;
        this.ItemID = itemID;
        this.enable = enable;
    }
}
public enum ItemType
{
    Error=-1,
    Empty=0,
    Magazine = 1,
    Bullet = 2,
    Gun = 3,
    Container = 4,
    Building = 5,
    Tool = 6,
    Materials = 7,
}
public class Empty : ContainerStatic
{
    public Empty() : base(ItemType.Empty) { }

    public override void update() { }
    public override void Collision(Collider collider) { }
}
public static class Items
{
    public static Item Empty {
        get {
            if (EPT == null) {
                EPT = new Empty();
            }
            return EPT;
        }
    }
    public static ItemStaticProperties.ItemStaticProperties EmptyStaticProperty { 
        get {
            if (EPTS == null) {
                EPTS = StaticPath.ItemLoad[ItemType.Empty, 0].ItemStaticProperties;
            }
            return EPTS;
        }
    }
    private static Item EPT;
    private static ItemStaticProperties.ItemStaticProperties EPTS;

    #region 物品获取函数及其实现
    public static void AddGenerators(ItemType itemType, int itemID,Type type){
        KeyValuePair<ItemType, int> keyValuePair = new KeyValuePair<ItemType, int>(itemType, itemID);
        if (Generators.TryGetValue(keyValuePair, out var func)) {
            Debug.LogError("物品ID多次映射! Type:" + itemType.ToString() + "  ID:" + itemID.ToString());
        }
        else {
            Generators.Add(keyValuePair, () => {
                Item tmp = (Item)type.Assembly.CreateInstance(type.FullName);
                return tmp;
            });
        }
    }

    public static void AddIsContainer(ItemType itemType, int itemID, Type type) {
        KeyValuePair<ItemType, int> keyValuePair = new KeyValuePair<ItemType, int>(itemType, itemID);
        IsContainer.Add(keyValuePair, true);
    }
    public static void FillIsContainerDictionary() {
        foreach (KeyValuePair<KeyValuePair<ItemType, int>, Func<Item>> keyValuePair in Generators) {
            if (IsContainer.TryGetValue(keyValuePair.Key, out bool value)) {

            }
            else {
                IsContainer.Add(keyValuePair.Key, false);
            }
        } 
    
    }
    public static Dictionary<KeyValuePair<ItemType, int>, bool> IsContainer = new Dictionary<KeyValuePair<ItemType, int>, bool>();
    public static Dictionary<KeyValuePair<ItemType, int>, Func<Item>> Generators = new Dictionary<KeyValuePair<ItemType, int>, Func<Item>>();

    public static Item GetItemByItemTypeAndItemIDWithoutItemProperty(ItemType itemType, int ItemID) {
        if (itemType == ItemType.Error) {
            Debug.LogError("未定义的场景初始化物品");
        }
        else if (itemType == ItemType.Empty && ItemID == 0) {
            return Empty;
        }
        else {
            KeyValuePair<ItemType, int> Key = new KeyValuePair<ItemType, int>(itemType, ItemID);
            if (Generators.TryGetValue(Key, out Func<Item> Func)) {
                return Func();
            }
        }
        return Empty;
    }

    public static Item GetItemByItemTypeAndItemIDStatic(ItemType itemType, int ItemID) {
        if (itemType == ItemType.Error) {
            Debug.LogError("未定义的场景初始化物品");
        }
        else if (itemType == ItemType.Empty && ItemID == 0) {
            return Empty;
        }
        else {
            KeyValuePair<ItemType, int> Key = new KeyValuePair<ItemType, int>(itemType, ItemID);
            if (Generators.TryGetValue(Key, out Func<Item> Func)) {
                Item tmp = Func.Invoke();
                ((Item_Detail)tmp).Info_Handler.Binding(new Item_Property(itemType, ItemID, null));
                return tmp;
            }
        }
        return Empty;
    }

    public static Item GetItemByItemTypeAndItemIDStatic(ItemType itemType, int ItemID,Action<Item> action) {
        if (itemType == ItemType.Error) {
            Debug.LogError("未定义的场景初始化物品");
        }
        else if (itemType == ItemType.Empty && ItemID == 0) {
            return Empty;
        }
        else {
            KeyValuePair<ItemType, int> Key = new KeyValuePair<ItemType, int>(itemType, ItemID);
            if (Generators.TryGetValue(Key, out Func<Item> Func)) {
                Item tmp = Func.Invoke();
                ((Item_Detail)tmp).Info_Handler.Binding(new Item_Property(itemType, ItemID, null));
                action.Invoke(tmp);
                return tmp;
            }
        }
        return Empty;
    }
    public static Item GetItemByItemTypeAndItemID_With_StaticProperties_And_PreInstanceProperties(ItemType itemType, int ItemID,ItemPreInstanceProperties.ItemPreInstanceProperties itemPreInstanceProperties) {
        if (itemType == ItemType.Error) {
            Debug.LogError("未定义的场景初始化物品");
        }
        else if (itemType == ItemType.Empty && ItemID == 0) {
            return Empty;
        }
        else {
            KeyValuePair<ItemType, int> Key = new KeyValuePair<ItemType, int>(itemType, ItemID);
            if (Generators.TryGetValue(Key, out Func<Item> Func)) {
                Item tmp=Func();
                tmp.Info_Handler.Binding(new Item_Property(itemType, ItemID, itemPreInstanceProperties));
                return tmp;
            }
        }
        return Empty;
    }

    public static bool GetIsContainerByItemTypeAndItemID(ItemType itemType,int ItemID) {
        if (itemType == ItemType.Error) {
            Debug.LogError("未定义的场景初始化物品");
        }
        else if (itemType == ItemType.Empty && ItemID == 0) {
            return false;
        }
        else {
            KeyValuePair<ItemType, int> Key = new KeyValuePair<ItemType, int>(itemType, ItemID);
            if (IsContainer.TryGetValue(Key, out bool value)) {
                return value;
            }
        }
        return false;     
    }
    #endregion
   
}

public interface Item_Detail {
    Item_INFO_Handler Info_Handler { get; set; }
    Item_Held_Handler Item_Held_Handler { get; }
    Item_Object_Handler Item_Object_Handler { get; }
    Item_UI_Handler Item_UI_Handler { get; }
    Item_Size_Handler Item_Size_Handler { get; }
    Item_Property GetItemProperty();
}
public interface ItemOnGroundBase
{
    void CollisionBase(Collision collision);
    void TriggerBase(Collider other);
}

public interface ItemUI
{
    ref ItemIntro GetItemIntroRef();
    ItemIntro GetItemIntro();
    int GetUIheld();
    bool Displaycount();
}

public interface ItemScript
{
    Animator GetAnimator();
    Container Outercontainer { get; set; }    
    List<BoxCollider> GetBoxCollider();
}

public interface Item : Item_Detail
{
    int ID { get; }
    ItemType Type { get; }
    GameObject Instance { get; }
    bool IsContainer { get; }
    Container Outercontainer { get; set; }
    Container Outestcontainer { get; }
    int Held { get; }
    void OuterClear();
    void Destory();
    void update();
    void TriggerLoop1(bool Bool);
    void TriggerLoop2(bool Bool);
    void TriggerLoop3(bool Bool);
    void TriggerLoop4(bool Bool);
    void TriggerLoop5(bool Bool);
    void Use1();            
    void Use2();
    void Use3();
    void Use4();
    void Use5();
    void Use6(Item item,out Item itemoutEX);
    bool Trysum(Item itemIn);
    void threw(Vector3 Dir);
    void Drop(Vector3 vector3);
    void InstanceTo(Vector3 vector3);
    void BeHeldButDrop(Transform transform);
    void Beheld(Transform transform);
    ContainerState GetContainerState();
    List<ItemNodeDynamic> GetItemNodes();
}

public abstract class ItemStatic : Item, ItemScript, ItemUI, ItemOnGroundBase
{
    public ItemType Type => this.Info_Handler.Item_Property.ItemStaticProperties.ItemType;
    public int ID => this.Info_Handler.Item_Property.ItemStaticProperties.ItemID;
    public int Held => this.Info_Handler.Held;
    public virtual bool IsContainer => false;
    public GameObject Instance => this.Info_Handler.Instance;
    
    public virtual Item_INFO_Handler Info_Handler
    {
        get {
            if (this.INFO_Handler == null) {
                this.INFO_Handler = new Item_INFO_Handle_Layer_Normal();
            }
            return this.INFO_Handler;
        }
        set {
            this.INFO_Handler = value;

        }
    }
    public Item_Held_Handler Item_Held_Handler => Info_Handler;
    public Item_Object_Handler Item_Object_Handler => Info_Handler;
    public Item_UI_Handler Item_UI_Handler => Info_Handler;
    public Item_Size_Handler Item_Size_Handler => Info_Handler;
    public Container Outercontainer { get => this.Out_Container; set => this.Out_Container = value; }
    public Container Outestcontainer => (Container)PreGetOutestcontainer(this);
    public ItemIntro ItemIntro
    {
        get {
            if (this.itemIntro == null) {
                this.itemIntro = new ItemIntro();
            }
            return this.itemIntro;
        }
        set {
            itemIntro = value;
        }
    }


    private Container Out_Container;
    public Item_INFO_Handler INFO_Handler;
    private ItemIntro itemIntro;



   
    
    public Timer timer1 = new Timer();
    public Timer timer2 = new Timer();
    public Timer timer3 = new Timer();
    public Timer timer4 = new Timer();
    public Timer timer5 = new Timer();
    protected bool Trigger1 = false;
    protected bool Trigger2 = false;
    protected bool Trigger3 = false;
    protected bool Trigger4 = false;
    protected bool Trigger5 = false;


    protected ItemStatic() { }
    


    public virtual void Death() {
        this.Info_Handler.Item_Property.ItemRuntimeProperties.destoryTriiger = ItemRuntimeProperties.DestoryTriiger.Destroyed;
        ((Item)this).OuterClear();
        this.Info_Handler.Destory();
    }


    public virtual void update() {
        this.Info_Handler.Trigger_Binded(this._AfterItemProperty);
        if (this.Info_Handler.Item_Property.ItemStaticProperties.useWays == ItemStaticProperties.UseWays.CanUse) {
            timer1.TimeingLoop(this.Use1, ref this.Trigger1);
            timer2.TimeingLoop(this.Use2, ref this.Trigger2);
            timer3.TimeingLoop(this.Use3, ref this.Trigger3);
            timer4.TimeingLoop(this.Use4, ref this.Trigger4);
            timer5.TimeingLoop(this.Use5, ref this.Trigger5);          
        }
        this.Update();
        if (this.Info_Handler.DeathCheck()) {
            this.DeathLogic(this.Death);
        }
    }
    public virtual void Update() { }

    #region 触发循环的底层调用
    public virtual void Use1() { }
    public virtual void Use2() { }
    public virtual void Use3() { }
    public virtual void Use4() { }
    public virtual void Use5() { }
    /// <summary>
    /// 从Player使用的Use6需手动剔除.即出去的itemoutEX held为零需要手动剔除 但不需要改变外层背包 并变为Items.Empty;
    /// </summary>
    /// <param name="item">输入物品</param>
    /// <param name="itemoutEX">输出物品</param>
    public virtual void Use6(Item item,out Item itemoutEX) { itemoutEX = item; }
    public void TriggerLoop1(bool Bool) {
        this.Trigger1 = Bool;
    }
    public void TriggerLoop2(bool Bool) {
        this.Trigger2 = Bool;
    }
    public void TriggerLoop3(bool Bool) {
        this.Trigger3 = Bool;
    }
    public void TriggerLoop4(bool Bool) {
        this.Trigger4 = Bool;
    }
    public void TriggerLoop5(bool Bool) {
        this.Trigger5 = Bool;
    }
    #endregion

    #region 碰撞和触发器函数的底层调用
    public virtual void Collision(Item item) { }
    void ItemOnGroundBase.CollisionBase(Collision collision) {
        this.Collision(collision);
        this.Collision(collision.collider);
        if (collision.collider.TryGetComponent<ItemOnTheGround>(out ItemOnTheGround onTheGround)) {
            this.Collision(onTheGround.itemOntheGround);
        }else if(collision.collider.TryGetComponent<Player_Instance>(out Player_Instance player_Instance)){
            this.Collision(player_Instance.Instance);
        }
    }
    public virtual void Collision(Collider collider) { }
    public virtual void Collision(Collision collision) { }
    public virtual void Collision(Player player) { }
    void ItemOnGroundBase.TriggerBase(Collider other) {
        this.Trigger(other);
        if (other.TryGetComponent<ItemOnTheGround>(out ItemOnTheGround onTheGround)) {
            this.Trigger(onTheGround.itemOntheGround);
        }
        else if (other.TryGetComponent<Player_Instance>(out Player_Instance player_Instance)) {
            this.Trigger(player_Instance.Instance);
        }
    }
    public virtual void Trigger(Collider other) { }
    public virtual void Trigger(Item item) { }
    public virtual void Trigger(Player player) { }
    #endregion
    private void SetItem() {
        this.Info_Handler.AddItemComponent(this);
    }
    private void DeathLogic(Action action) {
        if (this.Info_Handler.Item_Property.ItemRuntimeProperties.deathTrigger == ItemRuntimeProperties.DeathTrigger.UnDeath) {
            action.Invoke();
            this.Info_Handler.Item_Property.ItemRuntimeProperties.deathTrigger = ItemRuntimeProperties.DeathTrigger.Death;
        }
    }
    /// <summary>
    /// 尝试合并物品,Held加至上限,held==0剔除物品,无需主动剔除;(是安全的)
    /// </summary>
    /// <param name="itemIn">尝试合并的物品也就是held减少的一方</param>
    /// <returns>是否发生了剔除</returns>
    bool Item.Trysum(Item itemIn) {
        if (this.Type == itemIn.Type&&this.ID==itemIn.ID) {
            itemIn.Item_Held_Handler.SetHeld(((Item)this).Item_Held_Handler.Addheld(itemIn.Held));
            if (itemIn.Held == 0) {
                if (itemIn.Outercontainer!=null) {
                    itemIn.Outercontainer.DelItem(itemIn);
                    itemIn.Destory();
                    itemIn = Items.Empty;
                    return true;
                }
            }
        }
        return false;
    }

    void Item.OuterClear() {
        if (this.Outercontainer != null) {
            this.Outercontainer.DelItem(this);
        }
    }


    void Item.threw(Vector3 Dir) {
        PlayerManager.Main._DropThrewpre(Dir);      
    }
    public virtual void Destory() {
        this.Info_Handler.Destory();
    }
    public virtual void BeHeldButDrop(Transform transform) {
        this.Info_Handler.BeHeldButDrop(transform);
        this.SetItem();
    }
    public virtual void Beheld(Transform transform) {
        this.Info_Handler.BeHeld(transform);
        this.SetItem();
    }
    public virtual void InstanceTo(Vector3 vector3) {
        this.Info_Handler.BeInstance(vector3);
        this.SetItem();
    }
    public virtual void Drop(Vector3 vector3) {
        this.Info_Handler.BeDropping(vector3);
        this.SetItem();
    }

    private Item PreGetOutestcontainer(Item item) {
        if (item.Outercontainer != null) {
            Item temp = Outercontainer;
            return PreGetOutestcontainer(temp);
        }
        else
        return this;
    }
    

    Animator ItemScript.GetAnimator() {
        try {
            return this.Instance.GetComponent<Animator>();
        }
        catch(NullReferenceException) {
            Debug.LogError(String.Format("该物品不存在Animator  Type:{0} ID:{1}", this.Type, this.ID));
            return null;
        }
        catch (Exception) {
            Debug.LogError("在捕捉物品Animatior时出现错误  于脚本Item.ItemScript.GetAnimator方法");
            return null;
        }
    }

    public virtual List<BoxCollider> GetBoxCollider() {
        List<BoxCollider> BoxColliders = new List<BoxCollider>();
        if (this.Instance) {
            BoxColliders= BoxColliders.Concat(this.Instance.GetComponents<BoxCollider>()).ToList<BoxCollider>();
        }
        return BoxColliders;
    }
    public virtual List<ItemNodeDynamic> GetItemNodes() {
        List<ItemNodeDynamic> itemNodes = new List<ItemNodeDynamic>();
        return itemNodes;
    }
    public virtual ContainerState GetContainerState() {
        return null;
    }

    private void _AfterItemProperty() {
        this.Info_Handler.Setsynchronization_BeforeInstance(__SynchronizationBeforeInstance);
        this.__SynchronizationAfterItemPropertyConstructor();
    }
    public virtual void __SynchronizationAfterItemPropertyConstructor() { }
    public virtual void __SynchronizationBeforeInstance() { }




    public virtual int GetUIheld() {
        return this.Info_Handler.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial;
    }
    public virtual bool Displaycount() {
        return this.Info_Handler.Item_Property.ItemStaticProperties.numDisplayWays == ItemStaticProperties.NumDisplayWays.DisplayHeld;
    }

    public ref ItemIntro GetItemIntroRef() {
        return ref this.itemIntro;
    }

    public ItemIntro GetItemIntro() {
        return this.ItemIntro;
    }


    public Item_Property GetItemProperty() {
        return this.Info_Handler.Item_Property;
    }

}

public class IntroInfo
{
    public bool Enable;
    private bool HasVar;
    private bool Inited;
    private int VarCount;
    public string Info;
    public List<Pair<string>> Cuts;

    public IntroInfo(string input) {
        this.Enable = true;
        this.HasVar = false;
        this.Inited = false;
        this.VarCount = 0;
        this.Info = input;
    }

    private void Init() {
        bool flag = false;
        bool flag2 = false;
        string tmp = "";
        string tmp2 = "";
        Pair<string> pair = new Pair<string>(); ;
        for (int i = 0; i < this.Info.Length; i++) {         
            if (!flag) {
                tmp = "";
                tmp2 = "";
            }
            else {
                if (!flag2) {
                    tmp += Info[i];
                }
                else {
                    tmp2 += Info[i];
                }
            }
            if (Info[i] == '>') {
                if ((Info[i - 1] == 'F')) {
                    if (Info[i - 2] == '<') {
                        flag = true;
                    }
                }
            }
            else if (flag&&Info[i] == '|') {
                flag2 = true;
            }
            else if (Info[i] == '<') {
                if ((Info[i + 1] == '/')) {
                    if (Info[i + 2] == 'F') {
                        flag = false;
                        flag2 = false;
                        this.HasVar = true;
                        this.VarCount++;
                        if (tmp != ""&&tmp2!="") {
                            pair.first = tmp;
                            pair.Second = tmp2;
                            Cuts.Add(pair);
                            pair = new Pair<string>();
                        }
                    }
                }                
            }
        }
        this.Inited = true;
    }

    public string GetString() {
        if (this.Enable) {
            string Output = "";
            if (!this.Inited) {
                this.Init();
            }
            Output = this.Info;
            if (this.HasVar)
            for (int i = 0; i < this.VarCount; i++) {
                    Type t = Type.GetType(Cuts[i].first);
                    MethodInfo methodInfo = t.GetMethod(Cuts[i].Second);
                    var obj = t.Assembly.CreateInstance(Cuts[i].first);
                    Output.Replace("<F>" + Cuts[i] + "</F>", (string)methodInfo?.Invoke(obj, null));
            }
            return Output;
        }
        return "";
    }
}
public class ItemIntro
{
    int size;
    public List<IntroInfo> Intros=new List<IntroInfo>();
    public ItemIntro() {
        this.size = 0;
    }
    public ItemIntro(TextAsset textAsset) {
        string tmp = textAsset.text;
        string[] temp = tmp.Split('\n');
        this.size = temp.Length;
        for(int i = 0; i < this.size; i++) {
            this.Intros.Add(new IntroInfo(temp[i]));
        }
    }
    public ItemIntro(string str) {
        string tmp = str;
        string[] temp = tmp.Split('\n');
        this.size = temp.Length;
        for (int i = 0; i < this.size; i++) {
            this.Intros.Add(new IntroInfo(temp[i]));
        }
    }

    public ItemIntro(int size) {
        this.size = size;
        for(int i = 0; i < size; i++) {
            Intros.Add(new IntroInfo(""));
        }
    }
    public void Add(string input) {
        Intros.Add(new IntroInfo(input));
    }
    public void Expand(int size) {
        for(int i = 0; i < size; i++) {
            Intros.Add(new IntroInfo(""));
        }
    }
    public void ExpandTo(int size) {
        for (int i = this.size; i < size; i++) {
            Intros.Add(new IntroInfo(""));
        }
    }
    public void Disable(int x) {
        Intros[x].Enable = false;
    }
    public void Enable(int x) {
        Intros[x].Enable = true;
    }

    public string GetString() {
        string output = "";
        bool head = false;
        for(int i = 0; i < this.size; i++) {
            if (head) {
                output += "\n" + this.Intros[i].GetString();
            }
            else {
                output += this.Intros[i].GetString();
                head = true;
            }
        }
        return output;
    }



}










public interface Item_INFO_Handler : 
    Item_UI_Handler, Item_Logic_Handler, Item_Values_Handler,Item_Held_Handler,
    Item_Trigger_Handler, Item_Object_Handler, Item_Size_Handler, Item_synchronization_Handler, 
    Item_Rigid_Handler
{
    Item_Property Item_Property { get; set; }
    bool Binded { get; }
    void Binding(Item_Property Bingding);
    void AddItemComponent(Item item);
    void ReplaceInstance(GameObject Instance);
    void Destory();
}

public interface Item_synchronization_Handler {
    public void Setsynchronization_BeforeInstance(Action action);
}
public interface Item_Rigid_Handler
{

}

public interface Item_Object_Handler {
    void BeHeld(Transform transform);
    void BeInstance(Vector3 vector3);
    void BeDropping(Vector3 vector3);
    void BeHeldButDrop(Transform transform);

    GameObject Instance { get; }
    bool IsHeld { get; }
    bool IsInstanced { get; }
}
public interface Item_Trigger_Handler {
    void Trigger_Binded(Action action);
    void Trigger_Display(Action action);
}
public interface Item_UI_Handler {
    float GetHPrate();
    ItemStaticProperties.ItemStaticGraphs Graph { get; }
    Vector3 Center { get; }
    Vector2 CenterInScreen { get; }
    //Vector3 GetCenter();
    //Vector2 GetCenterInScreen();
}

public interface Item_Logic_Handler {
    bool DeathCheck();

  
}
public interface Item_Size_Handler
{
    int Size { get; }
}
public interface Item_Held_Handler {
    int Held { get; }
    int HeldMax { get; }
    int HeldOriginMax { get; }
    bool IsUseUp { get; }
    int Addheld(int num);
    int Decheld(int num);
    void SetMax(int max);
    void SetHeld(int max);

}
public interface Item_Values_Handler {
    void BeDmged(float DMG);
    void BeFixed(float FIX);
    
}
public class Item_INFO_Handle_Layer_Normal : Item_INFO_Handle_Layer_Base {
    public Item_INFO_Handle_Layer_Normal(){ }
}
public class Item_INFO_Handle_Temp
{
    public Item_INFO_Handle_Temp_Trigger_Group Trigger_Group = new Item_INFO_Handle_Temp_Trigger_Group();
    public bool Binded = false;
}
public class Item_INFO_Handle_Temp_Trigger_Group
{
    public bool Trigger_Binded = false;
    public bool Trigger_Display = false;

}
public abstract class Item_INFO_Handle_Layer_Base : Item_INFO_Handler
{
    public bool Binded {
        get {
            return this.INFO_Handle_Temp.Binded;
        }
    }
    public Item_Property Item_Property {
        get {  return this.item_Property; }
        set { this.item_Property = value; }
    }
    public Item_INFO_Handle_Layer_Base() { }

    private Item_Property item_Property;
    private Item_INFO_Handle_Temp INFO_Handle_Temp = new Item_INFO_Handle_Temp();




    public virtual void OnBinded() {
        RigidBody_Init();
    }

    private void RigidBody_Init() {
        if (this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Rigid.HaveRigidBody) {
            if (this.Instance) {
                this.Instance.AddComponent<Rigidbody>();
            }
        }
        else {
            if (this.Instance) {
                if (this.Instance.TryGetComponent<Rigidbody>(out var rigidbody)) {
                    GameObject.Destroy(rigidbody);
                }
            }
        }
    }
    private void RigidBody_Apply() {
        if(!this.Instance.TryGetComponent<Rigidbody>(out var rigidbody)) {
            this.Instance.AddComponent<Rigidbody>();
        }
        this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Rigid.HaveRigidBody = true;
    }
    private void RigidBody_Remove() {
        if (this.Instance.TryGetComponent<Rigidbody>(out var rigidbody)) {
            GameObject.Destroy(rigidbody);         
        }
        this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Rigid.HaveRigidBody = false;
    }

    #region INFO
    void Item_INFO_Handler.Binding(Item_Property Bingding) {
        this.item_Property = Bingding;
        this.INFO_Handle_Temp.Binded = true;
    }

    void Item_INFO_Handler.ReplaceInstance(GameObject Instance) {
        this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Unity.Instance = Instance;
        this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Bool.Instanced = true;
    }

    void Item_INFO_Handler.Destory() {
        if (Instanced && InstanceObj) {
            GameObject.Destroy(InstanceObj);
            Instanced = false;
            BeHelding = false;
        }
        else if (Instanced && !InstanceObj) {
            Instanced = false;
            BeHelding = false;
        }
    }
    #endregion
 

    #region Values
    int Item_Size_Handler.Size => this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Size.Size__Initial;

    int Item_Held_Handler.HeldOriginMax => this.Item_Property.ItemStaticProperties.ItemStaticValues.StaticValues_Held.Held_Origin_Max;
    int Item_Held_Handler.HeldMax => this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current_Max__Initial;
    int Item_Held_Handler.Held => this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial;
    public virtual void BeDmged(float DMG) {
        this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_State.HP_Current__Initial -= DMG - this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_State.DEF_Current__Initial;
    }
    public virtual void BeFixed(float FIX) {
        this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_State.HP_Current__Initial += FIX;
    }
    bool Item_Held_Handler.IsUseUp => this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial <= 0;
    public virtual bool DeathCheck() {
        if (this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_State.HP_Current__Initial <= 0.0f) {
            return true;
        }
        return false;
    }

    
    /// <summary>
    /// 返回剩余数量
    /// </summary>
    public virtual int Addheld(int num) {
        if (this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial + num > this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current_Max__Initial) {
            int heldbefore = this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial;
            this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial = this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current_Max__Initial;
            return heldbefore + num - this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current_Max__Initial;
        }
        else {
            this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial += num;
            return 0;
        }
    }
    /// <summary>
    /// 返回真实填充的数量(即原放入减去超出上限的部分)
    /// </summary>
    public virtual int Decheld(int num) {
        if (this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial - num <= 0) {
            int tmp = this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial;
            this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial = 0;
            return tmp;
        }
        else {
            this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial -= num;
            return num;
        }
    }
    void Item_Held_Handler.SetMax(int max) {
        this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current_Max__Initial = max;
    }
    void Item_Held_Handler.SetHeld(int num) {
        if (0 <= num && num <= this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current_Max__Initial) {
            this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial = num;
        }
        else if (num < 0) {
            this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial = 0;
        }
        else {
            this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial = this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current_Max__Initial;
        }
    }
    void Item_Trigger_Handler.Trigger_Binded(Action action) {
        if (this.Binded) {
            if (!this.INFO_Handle_Temp.Trigger_Group.Trigger_Binded) {
                action.Invoke();
                OnBinded();
                this.INFO_Handle_Temp.Trigger_Group.Trigger_Binded = true;
            }
        }
    }
    void Item_Trigger_Handler.Trigger_Display(Action action) {
        if (this.Binded) {
            if (!this.INFO_Handle_Temp.Trigger_Group.Trigger_Display) {
                action.Invoke();
                this.INFO_Handle_Temp.Trigger_Group.Trigger_Display = true;
            }
        }
    }
    #endregion
    #region UI
    ItemStaticProperties.ItemStaticGraphs Item_UI_Handler.Graph => this.Item_Property.ItemStaticProperties.ItemStaticGraphs;
    public virtual float GetHPrate() {
        return this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_State.HP_Current__Initial / this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_State.HP_Curren_Max__Initial;
    }
    Vector3 Item_UI_Handler.Center
    {
        get {
            Vector3 temp = new Vector3();
            if (this.Instance) {
                Transform tmp = this.Instance.transform.Find("Graph");
                if (tmp != null) {
                    temp = tmp.position;
                }
                else {
                    tmp = this.Instance.transform.Find("Point");
                    temp = tmp.position;
                }
            }
            return temp;
        }
    }
    Vector2 Item_UI_Handler.CenterInScreen
    {
        get {
            Vector3 temp = Camera.main.WorldToScreenPoint(((Item_UI_Handler)this).Center, Camera.MonoOrStereoscopicEye.Mono);
            return new Vector2(temp.x, temp.y);
        }
    }
    #endregion





    #region Object

    public GameObject Instance => InstanceObj;
    bool Item_Object_Handler.IsHeld => BeHelding;
    bool Item_Object_Handler.IsInstanced => Instanced;

    private Action Sys_BeforeInstance => this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Action.synchronization_beforeInstance;
    private bool Instanced { get => this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Bool.Instanced; set => this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Bool.Instanced = value; }
    private Transform ParTrans { get => this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Unity.ParTransform; set => this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Unity.ParTransform = value; }
    private GameObject InstanceObj { get => this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Unity.Instance; set => this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Unity.Instance = value; }
    private bool BeHelding { get => this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Bool.BeHeld; set => this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Bool.BeHeld = value; }
    private GameObject Origin => this.Item_Property.ItemStaticProperties.ItemStaticGraphs.StaticGraphs_Object.Main;


    
    public void BeHeld(Transform transform) {
        if (this.Item_Property.ItemStaticProperties.ItemStaticGraphs.StaticGraphs_Object.Main) {
            if (!Instanced) {
                Instanced = true;
                BeHelding = true;
                ParTrans = transform;
                InstanceObj = GameObject.Instantiate(Origin, transform, false);
                Sys_BeforeInstance?.Invoke();
                this.RigidBody_Remove();
                /*
                if (InstanceObj.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) {
                    rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    rigidbody.isKinematic = false;
                    rigidbody.useGravity = false;
                    rigidbody.freezeRotation = true;
                }
                */
                InstanceObj.layer = 8;
            }
        }
    }
    public void BeInstance(Vector3 vector3) {
        if (Origin) {
            if (!Instanced) {
                Instanced = true;
                BeHelding = false;
                InstanceObj = GameObject.Instantiate(Origin, vector3, new Quaternion());
                Sys_BeforeInstance?.Invoke();
                this.RigidBody_Apply();
                if (InstanceObj.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) {
                    rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
                    rigidbody.isKinematic = false;
                    rigidbody.useGravity = false;
                    rigidbody.freezeRotation = false;
                }
                InstanceObj.layer = 9;
            }
        }
    }
    public void BeDropping(Vector3 vector3) {
        if (Origin) {
            if (!Instanced) {
                Instanced = true;
                BeHelding = false;
                InstanceObj = GameObject.Instantiate(Origin, vector3, new Quaternion());
                Sys_BeforeInstance?.Invoke();
                this.RigidBody_Apply();
                if (InstanceObj.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) {
                    rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
                    rigidbody.isKinematic = false;
                    rigidbody.useGravity = true;
                    rigidbody.freezeRotation = false;
                }
                InstanceObj.layer = 9;
            }
            else {
                Sys_BeforeInstance?.Invoke();
                if (InstanceObj) {
                    InstanceObj.transform.parent = null;
                }
                this.RigidBody_Apply();
                if (InstanceObj.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) {
                    rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
                    rigidbody.isKinematic = false;
                    rigidbody.useGravity = true;
                    rigidbody.freezeRotation = false;
                }
                InstanceObj.layer = 9;
            }
        }
    }

    public void BeHeldButDrop(Transform transform) {
        if (Origin) {
            if (!Instanced) {
                Instanced = true;
                BeHelding = true;
                ParTrans = transform;
                InstanceObj = GameObject.Instantiate(Origin, transform, false);         
                Sys_BeforeInstance?.Invoke();
                RigidBody_Remove();
                /*
                if (InstanceObj.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) {
                    rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
                    rigidbody.isKinematic = true;
                    rigidbody.useGravity = true;
                    rigidbody.freezeRotation = false;
                }*/
                InstanceObj.gameObject.layer = 8;
            }
        }
    }


    public void AddItemComponent(Item item) {
        if (item != Items.Empty) {
            if (InstanceObj.TryGetComponent<ItemOnTheGround>(out ItemOnTheGround itemOnTheGround)) {
                itemOnTheGround.itemOntheGround = item;
            }
        }
    }

    #endregion
    #region synchronization
    void Item_synchronization_Handler.Setsynchronization_BeforeInstance(Action action) {
        this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Action.synchronization_beforeInstance = action;
    }
    #endregion
    #region Logic
    #endregion
}







[Serializable]
public class Item_Property {
    public ItemRuntimeProperties.ItemRuntimeProperties ItemRuntimeProperties;
    public ItemStaticProperties.ItemStaticProperties ItemStaticProperties;


    public Item_Property() { }
    public Item_Property(ItemType ItemType,int ItemID, ItemPreInstanceProperties.ItemPreInstanceProperties itemPreInstanceProperties) {
        this.ItemStaticProperties = StaticPath.ItemLoad[ItemType, ItemID].ItemStaticProperties;

        this.ItemRuntimeProperties = new ItemRuntimeProperties.ItemRuntimeProperties(this.ItemStaticProperties,itemPreInstanceProperties);
    }
    public Item_Property(ItemRuntimeProperties.ItemRuntimeProperties itemRuntimeProperties) {
        if(itemRuntimeProperties.Detail_Type== RuntimeProperty_Detail_Type.None) {
            this.ItemRuntimeProperties = itemRuntimeProperties;
            this.ItemStaticProperties = StaticPath.ItemLoad[this.ItemRuntimeProperties.ItemType, this.ItemRuntimeProperties.ItemID].ItemStaticProperties;
        }
        else if(itemRuntimeProperties.Detail_Type== RuntimeProperty_Detail_Type.Default) {
            this.ItemStaticProperties = StaticPath.ItemLoad[itemRuntimeProperties.ItemType, itemRuntimeProperties.ItemID].ItemStaticProperties;
            this.ItemRuntimeProperties = new ItemRuntimeProperties.ItemRuntimeProperties(this.ItemStaticProperties, null);
        }
        this.ItemRuntimeProperties.ItemRuntimeTemps = new ItemRuntimeProperties.ItemRuntimeTemps();
    }

}

public enum RuntimeProperty_Detail_Type
{
    None,
    Default,
    PreInstance,
}
namespace ItemRuntimeProperties
{
    
    public enum DeathTrigger {
        UnDeath,
        Death,
    }
    public enum DestoryTriiger {
        UnDestroyed,
        Destroyed,
    }
    [Serializable]
    public class ItemRuntimeValues {
        public RuntimeValues.RuntimeValues_State RuntimeValues_State = new RuntimeValues.RuntimeValues_State();
        public RuntimeValues.RuntimeValues_Held RuntimeValues_Held = new RuntimeValues.RuntimeValues_Held();
        public RuntimeValues.RuntimeValues_Size RuntimeValues_Size = new RuntimeValues.RuntimeValues_Size();
        public RuntimeValues.RuntimeValues_Rigid RuntimeValues_Rigid = new RuntimeValues.RuntimeValues_Rigid();
    }
    namespace RuntimeValues
    {
        [Serializable]
        public class RuntimeValues_State
        {
            public float HP_Curren_Max__Initial = 100;
            public float HP_Current__Initial = 100;
            public float DEF_Current__Initial = 0;
        }
        [Serializable]
        public class RuntimeValues_Held
        {
            public int Held_Current_Max__Initial = 30;
            public int Held_Current__Initial = 30;
        }
        [Serializable]
        public class RuntimeValues_Size {
            public int Size__Initial = 1;
        }
        [Serializable]
        public class RuntimeValues_Rigid
        {
            public bool HaveRigidBody = true;
        }
    }
    
    public class ItemRuntimeTemps {
        public RuntimeTemps.RuntimeTemps_Bool RuntimeTemps_Bool = new RuntimeTemps.RuntimeTemps_Bool();
        public RuntimeTemps.RuntimeTemps_Action RuntimeTemps_Action = new RuntimeTemps.RuntimeTemps_Action();
        public RuntimeTemps.RuntimeTemps_Unity RuntimeTemps_Unity = new RuntimeTemps.RuntimeTemps_Unity();   
    }
    namespace RuntimeTemps {
        public class RuntimeTemps_Bool {
            public bool Instanced = false;
            public bool BeHeld = false;
            
        }
        public class RuntimeTemps_Action {
            public Action synchronization_beforeInstance;
        }
        public class RuntimeTemps_Unity {
            public Transform ParTransform;
            public GameObject Instance;

        }
    
    }

  

    [Serializable]
    public class ItemRuntimeProperties
    {
        public ItemType ItemType = ItemType.Empty;
        public int ItemID = 0;
        public RuntimeProperty_Detail_Type Detail_Type = RuntimeProperty_Detail_Type.None;
        [HideInInspector]
        public bool Inited = false;
        public ItemStaticProperties.GetWays GetWays__Initial = ItemStaticProperties.GetWays.Hand;
        public DeathTrigger deathTrigger = DeathTrigger.UnDeath;
        public DestoryTriiger destoryTriiger = DestoryTriiger.UnDestroyed;
        public ItemRuntimeValues ItemRuntimeValues = new ItemRuntimeValues();
        public ItemRuntimeTemps ItemRuntimeTemps = new ItemRuntimeTemps();

        public ItemRuntimeProperties(ItemStaticProperties.ItemStaticProperties itemStaticProperties, ItemPreInstanceProperties.ItemPreInstanceProperties itemPreInstanceProperties) {
            if (!this.Inited) {
                this.ItemType = itemStaticProperties.ItemType;
                this.ItemID = itemStaticProperties.ItemID;
                
                GetWays__Initial = itemStaticProperties.GetWays;

                RuntimeValues.RuntimeValues_State Init_State = this.ItemRuntimeValues.RuntimeValues_State;
                ItemStaticProperties.StaticValues.StaticValues_State staticValues_State = itemStaticProperties.ItemStaticValues.StaticValues_State;
                Init_State.DEF_Current__Initial = staticValues_State.DEF_Origin;
                Init_State.HP_Current__Initial = staticValues_State.HP_Origin;
                Init_State.HP_Curren_Max__Initial = staticValues_State.HP_Origin_Max;

                RuntimeValues.RuntimeValues_Held Init_Held = this.ItemRuntimeValues.RuntimeValues_Held;
                ItemStaticProperties.StaticValues.StaticValues_Held staticValues_Held = itemStaticProperties.ItemStaticValues.StaticValues_Held;
                Init_Held.Held_Current_Max__Initial = staticValues_Held.Held_Origin_Max;
                Init_Held.Held_Current__Initial = staticValues_Held.Held_Origin;

                RuntimeValues.RuntimeValues_Size Init_Size = this.ItemRuntimeValues.RuntimeValues_Size;
                ItemStaticProperties.StaticValues.StaticValues_Size staticValues_Size = itemStaticProperties.ItemStaticValues.StaticValues_Size;
                Init_Size.Size__Initial = staticValues_Size.Size_Origin;

                RuntimeValues.RuntimeValues_Rigid Init_Rigid = this.ItemRuntimeValues.RuntimeValues_Rigid;
                ItemStaticProperties.StaticValues.StaticValues_Rigid staticValues_Rigid = itemStaticProperties.ItemStaticValues.StaticValues_Rigid;
                if (itemPreInstanceProperties == null) {
                    Init_Rigid.HaveRigidBody = staticValues_Rigid.HaveRigidBody;
                }
                else if (itemPreInstanceProperties.RigidbodyWays == ItemPreInstanceProperties.RigidbodyWays.Default) {                                  
                    Init_Rigid.HaveRigidBody = staticValues_Rigid.HaveRigidBody;
                }else if(itemPreInstanceProperties.RigidbodyWays== ItemPreInstanceProperties.RigidbodyWays.ForceNon) {
                    Init_Rigid.HaveRigidBody = false;
                }
                else if(itemPreInstanceProperties.RigidbodyWays== ItemPreInstanceProperties.RigidbodyWays.ForceUse) {
                    Init_Rigid.HaveRigidBody = true;
                }



            }
            this.Inited = true;        
        }

    }

    
}


namespace ItemStaticProperties
{
    public enum GetWays
    {
        /// <summary>
        /// 可以用手拾取,最低等级
        /// </summary>
        Hand,
        /// <summary>
        /// 可以被工具拾取
        /// </summary>
        Tool,

    }

    public enum LockWays {
        /// <summary>
        /// 不可被上锁的
        /// </summary>
        CantBeLocked,
        /// <summary>
        /// 可以被特定道具上锁
        /// </summary>
        CanBeLockedBySpecialItem,
        /// <summary>
        /// 甚至可以徒手上锁的
        /// </summary>
        CanBeLockedByHand,
    }
    public enum Death_WithinItem_Ways
    {
        /// <summary>
        /// 死亡时释放物品
        /// </summary>
        DropItem,
        /// <summary>
        /// 死亡时销毁物品
        /// </summary>
        TotallyDestory,
    }
    public enum UseWays {
        CanUse,
        CantUse,
    }
    public enum NumDisplayWays {
        DisplayHeld,
        NoDisplay,
    }
    [Serializable]
    public class DisplayWays
    {
        public bool Displayable = false;
        public bool Display_things = false;
        [Header("未实装")]
        public List<ItemStore> DisPlaythings_Which_is_Attching = new List<ItemStore>();
    }
    [Serializable]
    public class ItemStaticValues
    {
        public StaticValues.StaticValues_State StaticValues_State = new StaticValues.StaticValues_State();
        public StaticValues.StaticValues_Held StaticValues_Held = new StaticValues.StaticValues_Held();
        public StaticValues.StaticValues_Size StaticValues_Size = new StaticValues.StaticValues_Size();
        public StaticValues.StaticValues_Rigid StaticValues_Rigid = new StaticValues.StaticValues_Rigid();
    }
    namespace StaticValues
    {
        [Serializable]
        public class StaticValues_State
        {
            public float HP_Origin_Max = 100;
            public float HP_Origin = 100;
            public float DEF_Origin = 0;
        }
        [Serializable]
        public class StaticValues_Held
        {
            public int Held_Origin_Max = 30;
            public int Held_Origin = 30;

        }
        [Serializable]
        public class StaticValues_Size {
            public int Size_Origin = 1;
        }
        [Serializable]
        public class StaticValues_Rigid
        {
            public bool HaveRigidBody = true;
        }
    }
    [Serializable]
    public class ItemStaticGraphs {
        public StaticGraphs.StaticGraphs_Object StaticGraphs_Object = new StaticGraphs.StaticGraphs_Object();
        public StaticGraphs.StaticGraphs_Sprite StaticGraphs_Sprite = new StaticGraphs.StaticGraphs_Sprite();
    }
    namespace StaticGraphs {
        [Serializable]
        public class StaticGraphs_Sprite {
            public Sprite UI_Ingrid;
        }
        [Serializable]
        public class StaticGraphs_Object {
            public GameObject Main;
        }

    
    
    }
    [Serializable]
    public class ItemStaticProperties
    {
        public ItemStaticGraphs ItemStaticGraphs = new ItemStaticGraphs();
        public ItemType ItemType = ItemType.Empty;
        public int ItemID = 0;
        public GetWays GetWays = GetWays.Hand;
        public NumDisplayWays numDisplayWays = NumDisplayWays.DisplayHeld;
        public UseWays useWays = UseWays.CanUse;
        public LockWays LockWays = LockWays.CantBeLocked;
        public Death_WithinItem_Ways Death_WithinItem_Ways = Death_WithinItem_Ways.DropItem;
        public DisplayWays DisplayWays = new DisplayWays();
        public ItemStaticValues ItemStaticValues = new ItemStaticValues();       
    }
    
}
public enum ItemPreInstanceType {
    Standard,
    Rare,


}

namespace ItemPreInstanceProperties 
{
    public enum RigidbodyWays
    {
        Default,
        ForceUse,
        ForceNon,
    }
    public enum InstanceWays {
        SummonAnyWay,
        SummonInARate,
        DontSummon,
    }

    [Serializable]
    public class ItemPreInstanceProperties
    {
        public InstanceWays InstanceWays = InstanceWays.SummonAnyWay;
        public RigidbodyWays RigidbodyWays = RigidbodyWays.Default;
    }
}


