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
using UnityEngine.Events;

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
public class Empty : ContainerBase
{
    public Empty() : base(ItemType.Empty) { }

    public override void update() { }
    protected override void use1() { }
    protected override void use2() { }
    protected override void use3() { }
    protected override void use4() { }
    protected override void use5() { }

    public override void CollisionEnter(Collider collider) { }
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
    /// <summary>
    /// 调用后务必绑定ItemProperty
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static Item GetItemByItemTypeAndItemIDWithoutItemProperty(Item item) {
        if (item.Type == ItemType.Error) {
            Debug.LogError("未定义的场景初始化物品");
        }
        else if (item.Type == ItemType.Empty && item.ID == 0) {
            return Empty;
        }
        else {
            KeyValuePair<ItemType, int> Key = new KeyValuePair<ItemType, int>(item.Type, item.ID);
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
    Item_Element_Handler Item_Element_Handler { get; }
    Item_Status_Handler Item_Status_Handler { get; }
    Item_Logic_Handler Item_Logic_Handler { get; }
    Item_Values_Handler Item_Values_Handler { get; }
    Item_RuntimeProperty_Detail_Handler Item_RuntimeProperty_Detail_Handler { get; }
}

public interface ItemTimer
{
    ActionTimer Use1Timer { get; }
    ActionTimer Use2Timer { get; }
    ActionTimer Use3Timer { get; }
    ActionTimer Use4Timer { get; }
    ActionTimer Use5Timer { get; }
    ActionTimer Use6Timer { get; }
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
    void OnAttach();
    void OnUse();
    
}
public interface Item : Item_Detail,ItemTimer
{

    int ID { get; }
    ItemType Type { get; }
    bool IsEmpty { get; }
    GameObject Instance { get; }
    Animator Animator { get; }
    bool IsContainer { get; }
    Container Outercontainer { get; set; }
    Item Outestcontainer { get; }
    int Held { get; }
    float Mass { get; }
    bool CanGetNormally { get; }
    void OuterClear();
    /// <summary>
    /// 销毁GameObject和背包引用
    /// </summary>
    void DestoryFully();
    /// <summary>
    /// 销毁GameObject
    /// </summary>
    void Destory();

    void InterfaceUse1();            
    void InterfaceUse2();
    void InterfaceUse3();
    void InterfaceUse4();
    void InterfaceUse5();
    void InterfaceUse6(Item item,out Item itemoutEX);
    bool Trysum(Item itemIn);
    void Drop(Vector3 vector3);
    void InstanceTo(Vector3 vector3);
    void BeHeldButDrop(Transform transform);
    void Beheld(Transform transform);
    void BreakUp(Vector3 vector3);
    void BreakUp(Transform transform);
    ContainerState GetContainerState();
    List<ItemNodeDynamic> GetItemNodes();
    void UseBind(int Usenum, UnityAction Action);
    void UseInvoke(int Usenum);
    void UseForceInvoke(int Usenum);
   

    
    void OnSelected(MaterialPack materialPack);
}



public abstract class ItemBase : ObjectBase, Item, ItemScript, ItemUI
{

    public Animator Animator { get {
            try {
                return this.Instance.GetComponent<Animator>();
            }
            catch (NullReferenceException) {
                Debug.LogError(String.Format("该物品不存在Animator  Type:{0} ID:{1}", this.Type, this.ID));
                return null;
            }
            catch (Exception) {
                Debug.LogError("在捕捉物品Animator时出现错误  于脚本Item.ItemScript.GetAnimator方法");
                return null;
            }
        }
    }
    public bool IsEmpty => Items.Empty == this;
    public ItemType Type => this.Info_Handler.Item_Property.ItemStaticProperties.ItemType;
    public int ID => this.Info_Handler.Item_Property.ItemStaticProperties.ItemID;
    public virtual bool CanGetNormally => this.Item_Status_Handler.GetWays == GetWays.Hand;
    public int Held => this.Info_Handler.Held;
    public virtual bool IsContainer => false;
    public GameObject Instance => this.Info_Handler.Instance;
    
    public virtual Item_INFO_Handler Info_Handler
    {
        get {
            this.INFO_Handler ??= new Item_INFO_Handle_Layer_Normal();
            return this.INFO_Handler;
        }
        set {
            this.INFO_Handler = value;
        }
    }
    
    public Item_Element_Handler Item_Element_Handler => Info_Handler;
    public Item_Held_Handler Item_Held_Handler => Info_Handler;
    public Item_Object_Handler Item_Object_Handler => Info_Handler;
    public Item_UI_Handler Item_UI_Handler => Info_Handler;
    public Item_Size_Handler Item_Size_Handler => Info_Handler;
    public Item_Status_Handler Item_Status_Handler => Info_Handler;
    public Item_Logic_Handler Item_Logic_Handler => Info_Handler;
    public Item_Values_Handler Item_Values_Handler => Info_Handler;
    public Item_RuntimeProperty_Detail_Handler Item_RuntimeProperty_Detail_Handler => Info_Handler;
    public Container Outercontainer { get => this.Out_Container; set => this.Out_Container = value; }
    public Item Outestcontainer => PreGetOutestcontainer(this);
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


    private ActionTimer use1Timer = new ActionTimer();
    private ActionTimer use2Timer = new ActionTimer();
    private ActionTimer use3Timer = new ActionTimer();
    private ActionTimer use4Timer = new ActionTimer();
    private ActionTimer use5Timer = new ActionTimer();
    private ActionTimer use6Timer = new ActionTimer();


    public ActionTimer Use1Timer { get => use1Timer; }
    public ActionTimer Use2Timer { get => use2Timer; }
    public ActionTimer Use3Timer { get => use3Timer; }
    public ActionTimer Use4Timer { get => use4Timer; }
    public ActionTimer Use5Timer { get => use5Timer; }
    public ActionTimer Use6Timer { get => use6Timer; }

    public void UseBind(int Usenum, UnityAction Action) {
        switch (Usenum) {
            case 1: Use1Timer.Register(Action); break;
            case 2: Use2Timer.Register(Action); break;
            case 3: Use3Timer.Register(Action); break;
            case 4: Use4Timer.Register(Action); break;
            case 5: Use5Timer.Register(Action); break;
            case 6: Use6Timer.Register(Action); break;
            default: Debug.LogError("错误绑定");break;
        }
    }
    public void UseInvoke(int Usenum) {
        switch (Usenum) {
            case 1: Use1Timer.Invoke(); break;
            case 2: Use2Timer.Invoke(); break;
            case 3: Use3Timer.Invoke(); break;
            case 4: Use4Timer.Invoke(); break;
            case 5: Use5Timer.Invoke(); break;
            case 6: Use6Timer.Invoke(); break;
            default: Debug.LogError("错误绑定"); break;
        }
    }
    public void UseForceInvoke(int Usenum) {
        switch (Usenum) {
            case 1: Use1Timer.ForceInvoke(); break;
            case 2: Use2Timer.ForceInvoke(); break;
            case 3: Use3Timer.ForceInvoke(); break;
            case 4: Use4Timer.ForceInvoke(); break;
            case 5: Use5Timer.ForceInvoke(); break;
            case 6: Use6Timer.ForceInvoke(); break;
            default: Debug.LogError("错误绑定"); break;
        }
    }
    protected ItemBase() {
        Use1Timer.Register(use1);
        Use2Timer.Register(use2);
        Use3Timer.Register(use3);
        Use4Timer.Register(use4);
        Use5Timer.Register(use5);
    }
    


    public virtual void Death() {
        this.Item_Status_Handler.DestoryTriiger = DestoryTriiger.Destroyed;
        ((Item)this).OuterClear();
        this.Info_Handler.Destory();
    }

    public override void UpdateBase() {
        this.update();
        this.LogicUpdate();
        this.RenderUpdate();
    }
    public virtual void update() {
        this.Info_Handler.Trigger_Binded(this._AfterItemProperty);
        this.Update();
        if (this.Info_Handler.DeathCheck()) {
            this.DeathLogic(this.Death);
        }
        this.Info_Handler.Mass = this.Mass;
    }
    public virtual void Update() { }


    void Item.InterfaceUse1() {
        Use1Timer.Invoke();
    }
    void Item.InterfaceUse2() {
        Use2Timer.Invoke();
    }
    void Item.InterfaceUse3() {
        Use3Timer.Invoke();
    }
    void Item.InterfaceUse4() {
        Use4Timer.Invoke();
    }
    void Item.InterfaceUse5() {
        Use5Timer.Invoke();
    }


    protected virtual void use1() { Use1(); OnUse(); }
    protected virtual void use2() { Use2(); OnUse(); }
    protected virtual void use3() { Use3(); OnUse(); }
    protected virtual void use4() { Use4(); OnUse(); }
    protected virtual void use5() { Use5(); OnUse(); }
    void Item.InterfaceUse6(Item item,out Item itemoutEX) {
        this.Use6(item,out Item itemoutex);
        itemoutEX = itemoutex;
        if (!item.IsEmpty) {
            ((ItemScript)item).OnUse();
        }
        OnUse();
    }

    public void OnUse() {
        this.OnMassProbablyChange();
    }

    public void OnMassProbablyChange() {
        Info_Handler.Mass = this.Mass;
    }


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
    #endregion

    #region 碰撞和触发器函数的底层调用

    #endregion
    private void DeathLogic(Action action) {
        if (this.Item_Status_Handler.DeathTrigger == DeathTrigger.UnDeath) {
            action.Invoke();
            this.Item_Status_Handler.DeathTrigger = DeathTrigger.Death;
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


    public virtual void Destory() {
        this.Info_Handler.Destory();
    }
    public void DestoryFully() {
        ((Item)this).OuterClear();
        this.Destory();
    }
    public virtual void BeHeldButDrop(Transform transform) {
        this.Info_Handler.BeHeldButDrop(transform);
        this.Info_Handler.AddItemComponent(this);
    }
    public virtual void Beheld(Transform transform) {
        this.Info_Handler.BeHeld(transform);
        this.Info_Handler.AddItemComponent(this);
    }
    public virtual void InstanceTo(Vector3 vector3) {
        this.Info_Handler.BeInstance(vector3);
        this.Info_Handler.AddItemComponent(this);
    }
    public virtual void Drop(Vector3 vector3) {
        this.Info_Handler.BeDropping(vector3);
        this.Info_Handler.AddItemComponent(this);
    }

    public virtual void BreakUp(Vector3 vector3) {
    
    }
    public virtual void BreakUp(Transform transform) {
    
    }

    private Item PreGetOutestcontainer(Item item) {
        if (item.Outercontainer != null) {
            return PreGetOutestcontainer(item.Outercontainer);
        }
        else {
            return item;
        }
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
        ((Item)this).__LoadContext();
        ((Item)this).__UploadContext();
        ((Item)this).__StableContext();
        this.Info_Handler.Item_Property.Init();
        this.TimerInit();
        this.__SynchronizationAfterItemPropertyConstructor();
        this.OnMassProbablyChange();

        
        this.PropertyDestoryCheck();
    }
    private void PropertyDestoryCheck() {
        if (this.Outercontainer == null) {
            if (this.Item_Status_Handler.Player_Got || this.Item_Status_Handler.DeathTrigger == DeathTrigger.Death) {
                this.Destory();
            }
        }
    }
    private void TimerInit() {
        this.Use1Timer.SetTimer(() => { return Info_Handler.Item_Property.ItemStaticProperties.ItemStaticValues.StaticValues_State.Use1Timer; });
        this.Use2Timer.SetTimer(() => { return Info_Handler.Item_Property.ItemStaticProperties.ItemStaticValues.StaticValues_State.Use2Timer; });
        this.Use3Timer.SetTimer(() => { return Info_Handler.Item_Property.ItemStaticProperties.ItemStaticValues.StaticValues_State.Use3Timer; });
        this.Use4Timer.SetTimer(() => { return Info_Handler.Item_Property.ItemStaticProperties.ItemStaticValues.StaticValues_State.Use4Timer; });
        this.Use5Timer.SetTimer(() => { return Info_Handler.Item_Property.ItemStaticProperties.ItemStaticValues.StaticValues_State.Use5Timer; });
        this.Use6Timer.SetTimer(() => { return Info_Handler.Item_Property.ItemStaticProperties.ItemStaticValues.StaticValues_State.Use6Timer; });
        __TimerOverride(Use1Timer, Use2Timer, Use3Timer, Use4Timer, Use5Timer, Use6Timer);
    }
    public virtual void __SynchronizationAfterItemPropertyConstructor() { }
    public virtual void __SynchronizationBeforeInstance() { }
    public virtual void __TimerOverride(ActionTimer Use1, ActionTimer Use2, ActionTimer Use3, ActionTimer Use4, ActionTimer Use5, ActionTimer Use6) { }



    public virtual int GetUIheld() {
        return this.Info_Handler.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial;
    }
    public virtual bool Displaycount() {
        return this.Info_Handler.Item_Property.ItemStaticProperties.ItemStaticValues.StaticValues_Status.numDisplayWays == ItemStaticProperties.NumDisplayWays.DisplayHeld;
    }

    public ref ItemIntro GetItemIntroRef() {
        return ref this.itemIntro;
    }

    public ItemIntro GetItemIntro() {
        return this.ItemIntro;
    }

    public virtual float Mass { get {
            if (this.Info_Handler.Binded) {
                float sum = 0;
                foreach (var ele in this.Info_Handler.Elements) {
                    sum += ele.KG;
                }
                return sum * this.Held;
            }
            return 0;
        } 
    }

    public virtual void RenderUpdate() {
        if (this.Instance) {
            for (int i = 0; i < this.Item_UI_Handler.MaterialPack.materials.Count; i++) {
                this.Item_UI_Handler.MaterialPack.materials[i].SetFloat("CantGet", this.CanGetNormally ? 0 : 1);
                this.Item_UI_Handler.MaterialPack.materials[i].SetFloat("Rate", 1.0f - this.Item_UI_Handler.HPrate);
                this.Item_UI_Handler.MaterialPack.materials[i].SetFloat("Cover", this.Outestcontainer is AllContainer.CharacterStaticBag ? 1.0f : 0.0f);
                if (Input.GetKey(KeyCode.P) || this.Item_Logic_Handler.BeSelected) {
                    this.Item_UI_Handler.MaterialPack.materials[i].SetFloat("Framed", 1.0f);
                }
                else {
                    this.Item_UI_Handler.MaterialPack.materials[i].SetFloat("Framed", 0.0f);
                }
            }
        }
    }
    private void LogicUpdate() {
        if (this.Item_Logic_Handler.BeSelected) {
            OnSelected(this.Item_UI_Handler.MaterialPack);
        }
    }

    public virtual void OnSelected(MaterialPack materialPack) {
        
    }
    public virtual void OnDisSelected(MaterialPack materialPack) {

    }









    public virtual void OnAttach() { }
    public virtual void OnHand() { this.Item_Status_Handler.GetWays = GetWays.Tool; }
    public virtual void OffHand() { this.Item_Status_Handler.GetWays = GetWays.Hand; }
}
public class IntroInfo
{
    public class Pair<T>
    {
        public T First;
        public T Second;
    }


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
                            pair.First = tmp;
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
                    Type t = Type.GetType(Cuts[i].First);
                    MethodInfo methodInfo = t.GetMethod(Cuts[i].Second);
                    var obj = t.Assembly.CreateInstance(Cuts[i].First);
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
    Item_Rigid_Handler, Item_Thermodynamics_Handler, Item_Element_Handler, Item_Status_Handler,
    Item_RuntimeProperty_Detail_Handler
{
    Item_Property Item_Property { get; set; }
    bool Binded { get; }
    void Binding(Item_Property Bingding);
    void AddItemComponent(Item item);
    void ReplaceInstance(GameObject Instance);
    void Destory();
}
public interface Item_RuntimeProperty_Detail_Handler
{
    RuntimeProperty_Detail_Info RuntimeProperty_Detail_Info { get; }
    RuntimeProperty_Detail_Type RuntimeProperty_Detail_Type { get; }
}
public interface Item_Thermodynamics_Handler: IThermodynamics_Unit { }
public interface Item_Status_Handler
{
    bool Player_Got { get; set; }
    GetWays GetWays { get; set; }
    DeathTrigger DeathTrigger { get; set; }
    DestoryTriiger DestoryTriiger { get; set; }
    ItemStaticProperties.NumDisplayWays NumDisplayWays { get; }
    ItemStaticProperties.UseWays UseWays { get; }
    ItemStaticProperties.Death_WithinItem_Ways Death_WithinItem_Ways { get; }
    ItemStaticProperties.DisplayWays DisplayWays { get; }
}
public interface Item_synchronization_Handler {
    public void Setsynchronization_BeforeInstance(Action action);
}
public interface Item_Element_Handler
{
    IEnumerable<Element> Elements { get; }
    IEnumerable<Element> Liquid { get; }
    IEnumerable<Element> Solid { get; }
    IEnumerable<Element> Gas { get; }
    IEnumerable<Element> Plasma { get; }
    IEnumerable<Element> ElementMatch(Func<Element, bool> match);
}
public interface Item_Rigid_Handler
{
    float Mass { get; set; }
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
    float HPrate { get; }
    ItemStaticProperties.ItemStaticGraphs Graph { get; }
    MaterialPack MaterialPack { get; }
    Vector3 Center { get; }
    Vector2 CenterInScreen { get; }

}
public interface Item_Logic_Handler {
    bool DeathCheck();
    bool BeSelected { get; set; }
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
public interface Item_Values_Handler: Object_Values_Handler {

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
        get { return this.item_Property; }
        set { this.item_Property = value; }
    }
    public Item_INFO_Handle_Layer_Base() { }

    private Item_Property item_Property;
    private Item_INFO_Handle_Temp INFO_Handle_Temp = new Item_INFO_Handle_Temp();




    public virtual void OnBinded() {
        RigidBody_Init();
    }

    #region INFO
    void Item_INFO_Handler.Binding(Item_Property Bingding) {
        this.item_Property = Bingding;
        this.INFO_Handle_Temp.Binded = true;
    }

    void Item_INFO_Handler.ReplaceInstance(GameObject Instance) {
        this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Unity.Instance = Instance;
        this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Bool.Instanced = true;
        this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Ins.objectOnTheGround = Instance.GetComponent<ItemOnTheGround>();
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
    #region Thermodynamics

    private List<Element> Elements => this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Element.Elements;
    public float SHC { get {
            float Temp = 0;
            foreach (var element in this.Elements) {
                Temp += element.SHC;
            }
            return Temp * this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial;
        }
    }
    public float Temperature { get {
            float Temp = 0;
            foreach (var element in this.Elements) {
                Temp += element.temperature;
            }
            return Temp / this.Elements.Count;
        }
    }
    public void ThermodynamicsAdjustment(float Energy) {
        float Element_SHC = 0;
        foreach (var element in this.Elements) {
            Element_SHC += element.SHC;
        }
        float Div = Energy / Element_SHC;
        foreach (var element in this.Elements) {
            element.ThermodynamicsAdjustment(Div * element.SHC);
        }
    }

    #endregion

    #region RuntimeProperty_Detail
    RuntimeProperty_Detail_Info Item_RuntimeProperty_Detail_Handler.RuntimeProperty_Detail_Info => Item_Property.ItemRuntimeProperties.Detail_Info;
    RuntimeProperty_Detail_Type Item_RuntimeProperty_Detail_Handler.RuntimeProperty_Detail_Type => Item_Property.ItemRuntimeProperties.Detail_Type;
    #endregion


    #region Status
    bool Item_Status_Handler.Player_Got { get => this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Status.Player_Got; set => this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Status.Player_Got = value; }
    GetWays Item_Status_Handler.GetWays { get => this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Status.GetWays__Initial; set => this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Status.GetWays__Initial = value; }
    DeathTrigger Item_Status_Handler.DeathTrigger { get => this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Status.deathTrigger; set => this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Status.deathTrigger = value; }
    DestoryTriiger Item_Status_Handler.DestoryTriiger { get => this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Status.destoryTriiger; set => this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Status.destoryTriiger = value; }
    ItemStaticProperties.NumDisplayWays Item_Status_Handler.NumDisplayWays => Item_Property.ItemStaticProperties.ItemStaticValues.StaticValues_Status.numDisplayWays;
    ItemStaticProperties.UseWays Item_Status_Handler.UseWays => Item_Property.ItemStaticProperties.ItemStaticValues.StaticValues_Status.useWays;
    ItemStaticProperties.Death_WithinItem_Ways Item_Status_Handler.Death_WithinItem_Ways => Item_Property.ItemStaticProperties.ItemStaticValues.StaticValues_Status.Death_WithinItem_Ways;
    ItemStaticProperties.DisplayWays Item_Status_Handler.DisplayWays => Item_Property.ItemStaticProperties.ItemStaticValues.StaticValues_Status.DisplayWays;
    #endregion




    #region Element
    IEnumerable<Element> Item_Element_Handler.Elements { get { foreach (var ele in this.Elements) { yield return ele; } } }
    IEnumerable<Element> Item_Element_Handler.Liquid => ElementMatch((var) => { return var.ElementState == ElementState.Liquid; });
    IEnumerable<Element> Item_Element_Handler.Solid => ElementMatch((var) => { return var.ElementState == ElementState.Solid; });
    IEnumerable<Element> Item_Element_Handler.Gas => ElementMatch((var) => { return var.ElementState == ElementState.Gas; });
    IEnumerable<Element> Item_Element_Handler.Plasma => ElementMatch((var) => { return var.ElementState == ElementState.Plasma; });
    public IEnumerable<Element> ElementMatch(Func<Element, bool> match) {
        foreach (var ele in this.Elements) {
            if (match(ele)) {
                yield return ele;
            }
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

    public bool BeSelected { get => this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Bool.BeSelected; set => this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Bool.BeSelected = value; }
    public virtual bool DeathCheck() {
        if (this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_State.HP_Current__Initial <= 0.0f) {
            return true;
        }
        return false;
    }


    /// <summary>
    /// 返回添加后超过上限 所剩余数量
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
    /// 返回真实减少的数量
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
    #endregion
    #region Trigger
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
    public float HPrate => this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_State.HP_Current__Initial / this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_State.HP_Curren_Max__Initial;

    Vector3 Item_UI_Handler.Center {
        get {
            Vector3 temp = new Vector3();
            if (this.Instance) {
                Transform tmp = this.Instance.transform.Find("Graph");
                if (tmp != null) {
                    temp = tmp.position;
                }
                else {
                    tmp = this.Instance.transform.FindSon("Point");
                    temp = tmp.position;
                }
            }
            return temp;
        }
    }
    Vector2 Item_UI_Handler.CenterInScreen {
        get {
            Vector3 temp = Camera.main.WorldToScreenPoint(((Item_UI_Handler)this).Center, Camera.MonoOrStereoscopicEye.Mono);
            return new Vector2(temp.x, temp.y);
        }
    }

    MaterialPack Item_UI_Handler.MaterialPack {
        get {
            return this.Instance.GetComponent<ItemOnTheGround>().GetMaterialPack();
        }
    }
    #endregion

    #region Rigid
    public float Mass { get => this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Mass.Mass; set {
            this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Mass.Mass = value;
            if (this.Instance) {
                if (this.Instance.TryGetComponent<Rigidbody>(out var rigidbody)) {
                    rigidbody.mass = value;
                }
            }
        }
    }
    #endregion


    #region Object
    private void RigidBody_Init() {
        if (this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Rigid.HaveRigidBody) {
            if (this.Instance) {
                if (!this.Instance.TryGetComponent<Rigidbody>(out var rigidbody)) {
                    var rigid = this.Instance.AddComponent<Rigidbody>();
                    rigid.mass = Mass;
                    if (this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_PreInstance.RigidBodyDetail != null) {
                        var detail = this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_PreInstance.RigidBodyDetail;
                        if (detail.EnableDefaultDetail) {
                            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                        }
                        if (detail.EnableDetail) {
                            if (detail.X__PositionFreeze) {
                                rigid.constraints |= RigidbodyConstraints.FreezePositionX;
                            }
                            if (detail.Y__PositionFreeze) {
                                rigid.constraints |= RigidbodyConstraints.FreezePositionY;
                            }
                            if (detail.Z__PositionFreeze) {
                                rigid.constraints |= RigidbodyConstraints.FreezePositionZ;
                            }
                            if (detail.X__RotationFreeze) {
                                rigid.constraints |= RigidbodyConstraints.FreezeRotationX;
                            }
                            if (detail.Y__RotationFreeze) {
                                rigid.constraints |= RigidbodyConstraints.FreezeRotationY;
                            }
                            if (detail.Z__RotationFreeze) {
                                rigid.constraints |= RigidbodyConstraints.FreezeRotationZ;
                            }
                        }
                        this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_PreInstance.RigidBodyDetail = null;
                    }
                }
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
        if (!this.Instance.TryGetComponent<Rigidbody>(out var rigidbody)) {
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

    public ObjectOnTheGround ObjectOnTheGround => this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Ins.objectOnTheGround;
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
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                    rigidbody.isKinematic = false;
                    rigidbody.useGravity = false;
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
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                    rigidbody.isKinematic = false;
                    rigidbody.useGravity = true;
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
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                    rigidbody.isKinematic = false;
                    rigidbody.useGravity = true;
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
                InstanceObj.gameObject.layer = 8;
            }
        }
    }


    public void AddItemComponent(Item item) {
        if (item != Items.Empty) {
            if (InstanceObj.TryGetComponent<ItemOnTheGround>(out ItemOnTheGround itemOnTheGround)) {
                itemOnTheGround.itemOntheGround = item;
            }
            this.Item_Property.ItemRuntimeProperties.ItemRuntimeTemps.RuntimeTemps_Ins.objectOnTheGround = itemOnTheGround;
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

    #region Dialog

    IEnumerable<DialogMachine> Object_Values_Handler.DialogMachines {
        get {
            if (this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Dialog.DialogMachine != null){
                yield return this.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Dialog.DialogMachine;
            }
            if(((Item)this.ObjectOnTheGround.Object).IsContainer && this.ObjectOnTheGround.Object!=Items.Empty) {
                Container tmp = ((Container)this.ObjectOnTheGround.Object);
                for (int i = 0; i < tmp.Size; i++) {
                    if (tmp[i] != Items.Empty) {
                        if (tmp[i].Instance) {
                            foreach (var Dialogmachine in tmp[i].Info_Handler.DialogMachines) {
                                yield return Dialogmachine;
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion
}



[Serializable]
public partial class Item_Property
{
    public ItemRuntimeProperties.ItemRuntimeProperties ItemRuntimeProperties;
    public ItemStaticProperties.ItemStaticProperties ItemStaticProperties;


    public Item_Property(ItemType ItemType,int ItemID, ItemPreInstanceProperties.ItemPreInstanceProperties itemPreInstanceProperties) {
        this.ItemStaticProperties = StaticPath.ItemLoad[ItemType, ItemID].ItemStaticProperties;
        this.ItemRuntimeProperties = new ItemRuntimeProperties.ItemRuntimeProperties(this.ItemStaticProperties,itemPreInstanceProperties);
    }
    public Item_Property(Item CopyTypeAndID, ItemPreInstanceProperties.ItemPreInstanceProperties itemPreInstanceProperties) {
        this.ItemStaticProperties = StaticPath.ItemLoad[CopyTypeAndID.Type, CopyTypeAndID.ID].ItemStaticProperties;
        this.ItemRuntimeProperties = new ItemRuntimeProperties.ItemRuntimeProperties(this.ItemStaticProperties, itemPreInstanceProperties);
    }
    public Item_Property(ItemRuntimeProperties.ItemRuntimeProperties itemRuntimeProperties) {
        if (itemRuntimeProperties.Detail_Info == RuntimeProperty_Detail_Info.Properties) {
            if (itemRuntimeProperties.Detail_Type == RuntimeProperty_Detail_Type.AllDetail) {
                this.ItemRuntimeProperties = itemRuntimeProperties;
                this.ItemStaticProperties = StaticPath.ItemLoad[this.ItemRuntimeProperties.ItemType, this.ItemRuntimeProperties.ItemID].ItemStaticProperties;
            }
            else if (itemRuntimeProperties.Detail_Type == RuntimeProperty_Detail_Type.Default) {
                try {
                    this.ItemStaticProperties = StaticPath.ItemLoad[itemRuntimeProperties.ItemType, itemRuntimeProperties.ItemID].ItemStaticProperties;
                    this.ItemRuntimeProperties = new ItemRuntimeProperties.ItemRuntimeProperties(this.ItemStaticProperties, null);
                }
                catch {
                    Debug.Log("");
                }
            }
        }
        else if(itemRuntimeProperties.Detail_Info== RuntimeProperty_Detail_Info.Store) {
            if (itemRuntimeProperties.Detail_Type == RuntimeProperty_Detail_Type.AllDetail) {
                this.ItemRuntimeProperties = itemRuntimeProperties;

                this.ItemStaticProperties = itemRuntimeProperties.ItemStore.ItemStaticProperties;
            }
            else if (itemRuntimeProperties.Detail_Type == RuntimeProperty_Detail_Type.Default) {
                this.ItemStaticProperties = itemRuntimeProperties.ItemStore.ItemStaticProperties;
                this.ItemRuntimeProperties = new ItemRuntimeProperties.ItemRuntimeProperties(this.ItemStaticProperties, null);
            }
        }


        this.ItemRuntimeProperties.ItemRuntimeTemps = new ItemRuntimeProperties.ItemRuntimeTemps();
    }

}
public enum RuntimeProperty_Detail_Info
{
    Store,
    Properties,
}
public enum RuntimeProperty_Detail_Type
{
    AllDetail,
    Default,
}
namespace ItemRuntimeProperties
{
    
    
    
    [Serializable]
    public class ItemRuntimeValues {
        public RuntimeValues.RuntimeValues_State RuntimeValues_State = new RuntimeValues.RuntimeValues_State();
        public RuntimeValues.RuntimeValues_Held RuntimeValues_Held = new RuntimeValues.RuntimeValues_Held();
        public RuntimeValues.RuntimeValues_Size RuntimeValues_Size = new RuntimeValues.RuntimeValues_Size();
        public RuntimeValues.RuntimeValues_Rigid RuntimeValues_Rigid = new RuntimeValues.RuntimeValues_Rigid();
        public RuntimeValues.RuntimeValues_Element RuntimeValues_Element = new RuntimeValues.RuntimeValues_Element();
        public RuntimeValues.RuntimeValues_Status RuntimeValues_Status = new RuntimeValues.RuntimeValues_Status();
        public RuntimeValues.RuntimeValues_Dialog RuntimeValues_Dialog = new RuntimeValues.RuntimeValues_Dialog();
        public ItemRuntimeContext ItemRuntimeContext = new ItemRuntimeContext();
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
        [Serializable]
        public class RuntimeValues_Element
        {
            public List<Element> Elements = new List<Element>();
        }
        [Serializable]
        public class RuntimeValues_Status
        {
            public bool Player_Got = false;
            public GetWays GetWays__Initial = GetWays.Hand;
            public DeathTrigger deathTrigger = DeathTrigger.UnDeath;
            public DestoryTriiger destoryTriiger = DestoryTriiger.UnDestroyed;
        }
        [Serializable]
        public class RuntimeValues_Dialog
        {
            public DialogMachine DialogMachine;
        }
    }
    
    public class ItemRuntimeTemps {
        public RuntimeTemps.RuntimeTemps_Bool RuntimeTemps_Bool = new RuntimeTemps.RuntimeTemps_Bool();
        public RuntimeTemps.RuntimeTemps_Action RuntimeTemps_Action = new RuntimeTemps.RuntimeTemps_Action();
        public RuntimeTemps.RuntimeTemps_Unity RuntimeTemps_Unity = new RuntimeTemps.RuntimeTemps_Unity();
        public RuntimeTemps.RuntimeTemps_PreInstance RuntimeTemps_PreInstance = new RuntimeTemps.RuntimeTemps_PreInstance();
        public RuntimeTemps.RuntimeTemps_Mass RuntimeTemps_Mass = new RuntimeTemps.RuntimeTemps_Mass();
        public RuntimeTemps.RuntimeTemps_Ins RuntimeTemps_Ins = new RuntimeTemps.RuntimeTemps_Ins();
    }
    namespace RuntimeTemps {
        public class RuntimeTemps_Mass {
            public float Mass;
        }
        public class RuntimeTemps_Bool {
            public bool Instanced = false;
            public bool BeHeld = false;
            public bool BeSelected = false;
        }
        public class RuntimeTemps_Ins
        {
            public ObjectOnTheGround objectOnTheGround;
        }
        public class RuntimeTemps_Action {
            public Action synchronization_beforeInstance;
        }
        public class RuntimeTemps_Unity {
            public Transform ParTransform;
            public GameObject Instance;
        }
        public class RuntimeTemps_PreInstance
        {
            public ItemPreInstanceProperties.RigidBodyDetail RigidBodyDetail;
        }    
    }


    [Serializable]
    public class ItemRuntimeContext : ItemContext {
        
    }




    [Serializable]
    public class ItemRuntimeProperties
    {
        public ItemType ItemType = ItemType.Empty;
        public int ItemID = 0;

        public RuntimeProperty_Detail_Info Detail_Info = RuntimeProperty_Detail_Info.Properties;
        public RuntimeProperty_Detail_Type Detail_Type = RuntimeProperty_Detail_Type.AllDetail;

        public ItemStore ItemStore; 

        [HideInInspector]
        public bool Inited = false;
        public ItemRuntimeValues ItemRuntimeValues = new ItemRuntimeValues();
        public ItemRuntimeTemps ItemRuntimeTemps = new ItemRuntimeTemps();
        

        public ItemRuntimeProperties(ItemStaticProperties.ItemStaticProperties itemStaticProperties, ItemPreInstanceProperties.ItemPreInstanceProperties itemPreInstanceProperties) {
            if (!this.Inited) {
                this.ItemType = itemStaticProperties.ItemType;
                this.ItemID = itemStaticProperties.ItemID;


                RuntimeValues.RuntimeValues_Status Init_Status = this.ItemRuntimeValues.RuntimeValues_Status;
                ItemStaticProperties.StaticValues.StaticValues_Status staticValues_Status = itemStaticProperties.ItemStaticValues.StaticValues_Status;
                Init_Status.GetWays__Initial = staticValues_Status.GetWays;

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
                }
                else if(itemPreInstanceProperties.RigidbodyWays== ItemPreInstanceProperties.RigidbodyWays.ForceNon) {
                    Init_Rigid.HaveRigidBody = false;
                }
                else if(itemPreInstanceProperties.RigidbodyWays== ItemPreInstanceProperties.RigidbodyWays.ForceUse) {
                    Init_Rigid.HaveRigidBody = true;
                }

                RuntimeValues.RuntimeValues_Element Init_Element = this.ItemRuntimeValues.RuntimeValues_Element;
                ItemStaticProperties.StaticValues.StaticValues_Element staticValues_Element = itemStaticProperties.ItemStaticValues.StaticValues_Element;
                Init_Element.Elements = staticValues_Element.Elements;





                if (itemPreInstanceProperties != null) {
                    RuntimeValues.RuntimeValues_Dialog Binds_Dialog = ItemRuntimeValues.RuntimeValues_Dialog;
                    Binds_Dialog.DialogMachine = itemPreInstanceProperties.PreInstance_Binds.DialogMachine;

                    this.ItemRuntimeTemps.RuntimeTemps_PreInstance.RigidBodyDetail = itemPreInstanceProperties.rigidBodyDetail;
                }

            }
            this.Inited = true;        
        }

    }
}
#region 动静态共用部分
public enum GetWays
{
    Hand,
    Tool,
}
public enum DeathTrigger
{
    UnDeath,
    Death,
}
public enum DestoryTriiger
{
    UnDestroyed,
    Destroyed,
}
#endregion
namespace ItemStaticProperties
{

    public enum Death_WithinItem_Ways {
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
    public class DisplayWays {
        public bool Displayable = false;
        public bool Display_things = false;
        [Header("未实装")]
        public List<ItemStore> DisPlaythings_Which_is_Attching = new List<ItemStore>();
    }
    [Serializable]
    public class ItemStaticValues {
        public StaticValues.StaticValues_Status StaticValues_Status = new StaticValues.StaticValues_Status();
        public StaticValues.StaticValues_State StaticValues_State = new StaticValues.StaticValues_State();
        public StaticValues.StaticValues_Held StaticValues_Held = new StaticValues.StaticValues_Held();
        public StaticValues.StaticValues_Size StaticValues_Size = new StaticValues.StaticValues_Size();
        public StaticValues.StaticValues_Rigid StaticValues_Rigid = new StaticValues.StaticValues_Rigid();      
        public StaticValues.StaticValues_Element StaticValues_Element = new StaticValues.StaticValues_Element();
        public ItemStaticContext ItemStaticContext = new ItemStaticContext();
    }
    namespace StaticValues {
        [Serializable]
        public class StaticValues_State {
            public float HP_Origin_Max = 100;
            public float HP_Origin = 100;
            public float DEF_Origin = 0;

            public float Use1Timer = 0.1f;
            public float Use2Timer = 0.1f;
            public float Use3Timer = 0.1f;
            public float Use4Timer = 0.1f;
            public float Use5Timer = 0.1f;
            public float Use6Timer = 0.1f;
        }
        [Serializable]
        public class StaticValues_Held {
            public int Held_Origin_Max = 30;
            public int Held_Origin = 30;
        }
        [Serializable]
        public class StaticValues_Size {
            public int Size_Origin = 1;
        }
        [Serializable]
        public class StaticValues_Rigid {
            public bool HaveRigidBody = true;
        }
        [Serializable]
        public class StaticValues_Element {
            public List<Element> Elements = new List<Element>();
        }
        [Serializable]
        public class StaticValues_Status {
            public GetWays GetWays = GetWays.Hand;
            public NumDisplayWays numDisplayWays = NumDisplayWays.DisplayHeld;
            public UseWays useWays = UseWays.CanUse;
            public Death_WithinItem_Ways Death_WithinItem_Ways = Death_WithinItem_Ways.DropItem;
            public DisplayWays DisplayWays = new DisplayWays();
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
    public class ItemStaticContext: ItemContext {
        public ItemContextMapping ItemContextMapping = new ItemContextMapping();
    }













    [Serializable]
    public class ItemStaticProperties
    {
        public ItemStaticGraphs ItemStaticGraphs = new ItemStaticGraphs();
        public ItemType ItemType = ItemType.Empty;
        public int ItemID = 0;
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
    public class RigidBodyDetail
    {
        public bool EnableDefaultDetail = false;
        public bool EnableDetail = false;
        public bool X__RotationFreeze = false;
        public bool Y__RotationFreeze = false;
        public bool Z__RotationFreeze = false;
        public bool X__PositionFreeze = false;
        public bool Y__PositionFreeze = false;
        public bool Z__PositionFreeze = false;
    }

    [Serializable]
    public class PreInstance_Binds
    {
        public DialogMachine DialogMachine;
    }


    [Serializable]
    public class ItemPreInstanceProperties
    {
        public InstanceWays InstanceWays = InstanceWays.SummonAnyWay;
        public RigidbodyWays RigidbodyWays = RigidbodyWays.Default;
        public RigidBodyDetail rigidBodyDetail = new RigidBodyDetail();
        public PreInstance_Binds PreInstance_Binds = new PreInstance_Binds();
    }
}




[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class LoadAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property)]
public class LoadPropertiesAttribute : Attribute
{
    public PropertyType LoadType;
    public LoadPropertiesAttribute(PropertyType propertyType = PropertyType.Static) {
        this.LoadType = propertyType;
    }


}































[CustomPropertyDrawer(typeof(ItemRuntimeProperties.ItemRuntimeProperties))]
public class ItemRuntimePropertiesEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        using (new EditorGUI.PropertyScope(position, label, property)) {
        Rect Next;
            EditorGUIUtility.labelWidth = 60;
            position.height = EditorGUIUtility.singleLineHeight;
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            position.y -= EditorGUIUtility.singleLineHeight + 2;

            SerializedProperty[] Base = new SerializedProperty[2] {
        property.FindPropertyRelative("Detail_Info"),
        property.FindPropertyRelative("Detail_Type"),
        };
            GUIContent[] BaseName = new GUIContent[2] {
            new GUIContent("信息描述方法"),
            new GUIContent("细节描述方法"),
        };
            Next = EditorEx.NewPropertyGroup(position, Base, BaseName);
            string select_Info = property.FindPropertyRelative("Detail_Info").enumNames[property.FindPropertyRelative("Detail_Info").enumValueIndex];
            string select_Type = property.FindPropertyRelative("Detail_Type").enumNames[property.FindPropertyRelative("Detail_Type").enumValueIndex];
            if (select_Type == "AllDetail") {
                if (select_Info == "Store") {
                    SerializedProperty[] StoreType = new SerializedProperty[2] {
                    property.FindPropertyRelative("ItemStore"),
                    property.FindPropertyRelative("ItemRuntimeValues"),
                    };
                    EditorEx.NewPropertyGroup(Next, StoreType);

                }

                
                else if (select_Info == "Properties") {
                    SerializedProperty[] PropertiesType = new SerializedProperty[3] {
                    property.FindPropertyRelative("ItemType"),
                    property.FindPropertyRelative("ItemID"),
                    property.FindPropertyRelative("ItemRuntimeValues"),
                    };
                    EditorEx.NewPropertyGroup(Next, PropertiesType);
                }
            }else if(select_Type== "Default") {
                if (select_Info == "Store") {
                    SerializedProperty[] StoreType = new SerializedProperty[1] {
                    property.FindPropertyRelative("ItemStore"),
                    };
                    EditorEx.NewPropertyGroup(Next, StoreType);

                }


                else if (select_Info == "Properties") {
                    SerializedProperty[] PropertiesType = new SerializedProperty[2] {
                    property.FindPropertyRelative("ItemType"),
                    property.FindPropertyRelative("ItemID"),
                    };
                    EditorEx.NewPropertyGroup(Next, PropertiesType);
                }
            }

            EditorGUI.indentLevel = indent;
        }
        
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        string select_Info = property.FindPropertyRelative("Detail_Info").enumNames[property.FindPropertyRelative("Detail_Info").enumValueIndex];
        string select_Type = property.FindPropertyRelative("Detail_Type").enumNames[property.FindPropertyRelative("Detail_Type").enumValueIndex];
        if (select_Type == "AllDetail") {
            if (select_Info == "Store") {
                return 3 * (EditorGUIUtility.singleLineHeight + 2) + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("ItemRuntimeValues"));
            }
            else if (select_Info == "Properties") {
                return 4 * (EditorGUIUtility.singleLineHeight + 2) + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("ItemRuntimeValues"));
            }
        }
        else if (select_Type == "Default") {
            if (select_Info == "Store") {
                return 3 * (EditorGUIUtility.singleLineHeight + 2);
            }
            else if (select_Info == "Properties") {
                return 4 * (EditorGUIUtility.singleLineHeight + 2);
            }
        }
        return 50;
    }
}

[CustomPropertyDrawer(typeof(ItemStaticProperties.ItemStaticContext))]
public class ItemStaticContextPropertiesEditor : PropertyDrawer
{
    public static bool IsOpen = false;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        Rect Next = EditorEx.GetStartRect(position);
        EditorGUIUtility.labelWidth = 60;


        if (IsOpen = EditorGUI.Foldout(Next, IsOpen, "静态存储映射")) {
            Next = EditorEx.GetNormalRect(Next);
            var ItemContextMapping = property.FindPropertyRelative("ItemContextMapping");
            var StaticPacks = ItemContextMapping.FindPropertyRelative("StaticPacks");

            using (new EditorGUI.IndentLevelScope()) {
                for (int i = 0; i < StaticPacks.arraySize; i++) {
                    var StaticPack = StaticPacks.GetArrayElementAtIndex(i);
                    var name = StaticPack.FindPropertyRelative("PropertyName").stringValue;
                    var ___Data = StaticPack.FindPropertyRelative("___Data").intValue;
                    var PosInList = StaticPack.FindPropertyRelative("PosInList").intValue;
                    var Content = new GUIContent(name);



                    switch (___Data) {
                        case 1:
                            Next = EditorEx.CustomIntProperty(Next, property.FindPropertyRelative("IntData").GetArrayElementAtIndex(PosInList), Content);
                            break;
                        case 2:
                            Next = EditorEx.CustomBoolProperty(Next, property.FindPropertyRelative("BoolData").GetArrayElementAtIndex(PosInList), Content);
                            break;
                        case 3:
                            Next = EditorEx.CustomFloatProperty(Next, property.FindPropertyRelative("FloatData").GetArrayElementAtIndex(PosInList), Content);
                            break;
                        case 4:
                            Next = EditorEx.NewProperty(Next, property.FindPropertyRelative("StringData").GetArrayElementAtIndex(PosInList), Content);
                            break;
                        case 5:
                            Next = EditorEx.CustEnumProperty(Next, property.FindPropertyRelative("EnumData").GetArrayElementAtIndex(PosInList), Content);
                            break;
                        default: Debug.Log("编辑器错误"); break;
                    }

                }
            }
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        if (IsOpen)
            return EditorEx.NormalHeightByNum(property.FindPropertyRelative("ItemContextMapping").FindPropertyRelative("StaticPacks").arraySize + 1);
        else
            return EditorEx.NormalHeightByNum(1);
    }
}


[CustomPropertyDrawer(typeof(ItemRuntimeProperties.ItemRuntimeContext))]
public class ItemRuntimeContextPropertiesEditor : PropertyDrawer
{
    public static bool Inited = false;
    public static bool IsOpen = false;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        
        property.serializedObject.Update();

        Rect Next = EditorEx.GetStartRect(position);
        Next = EditorEx.GetNormalRect(Next);
        EditorGUIUtility.labelWidth = 60;


        ItemStore itemStore = null;
        ItemNodeDynamic Node = null;
        //if (property.serializedObject.targetObject is ItemInfoStore) {

        Node = ((ItemNodeDynamic)EditorEx.GetObjectByPath(property.serializedObject.targetObject, property.propertyPath, 4));


        if (!
          (Node.GetItemTypeAndItemId().Item1 == ItemType.Container && Node.GetItemTypeAndItemId().Item2 == 0) ||
          (Node.GetItemTypeAndItemId().Item1 == ItemType.Empty && Node.GetItemTypeAndItemId().Item2 == 0) ||
          (Node.GetItemTypeAndItemId().Item1 == ItemType.Error)) { 



            itemStore = StaticPath.ItemLoad[Node.GetItemTypeAndItemId().Item1, Node.GetItemTypeAndItemId().Item2];


            //}
            //else if (property.serializedObject.targetObject is ItemOnTheGround) {
            //    ItemOnTheGround item = ((ItemOnTheGround)property.serializedObject.targetObject);
            //    foreach(var field in item.GetType().GetFields()) {
            //        Debug.Log(field.Name);
            //    }
            //    Debug.Log(property.propertyPath);
            //    //ItemNodeDynamic temp = ((ItemNodeDynamic)EditorEx.GetObjectByPath(property.serializedObject.targetObject, property.propertyPath, 4));
            

            //}


            if (itemStore != null) {
                if (IsOpen = EditorGUI.Foldout(Next, IsOpen, "动态存储映射")) {
                    Next = EditorEx.GetNormalRect(Next);

                    using (var scope = new EditorGUI.ChangeCheckScope()) {

                        var ItemContextMapping = itemStore.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping;

                        var RuntimePacks = ItemContextMapping.RuntimePacks;

                        using (new EditorGUI.IndentLevelScope()) {
                            for (int i = 0; i < RuntimePacks.Count; i++) {
                                var RuntimePack = RuntimePacks[i];
                                var name = RuntimePack.PropertyName;
                                var ___Data = RuntimePack.___Data;
                                var PosInList = RuntimePack.PosInList;
                                var Content = new GUIContent(name);

                                switch (___Data) {
                                    case 1:
                                        Next = EditorEx.CustomIntProperty(Next, property.FindPropertyRelative("IntData").GetArrayElementAtIndex(PosInList), Content);
                                        break;
                                    case 2:
                                        Next = EditorEx.CustomBoolProperty(Next, property.FindPropertyRelative("BoolData").GetArrayElementAtIndex(PosInList), Content);
                                        break;
                                    case 3:
                                        Next = EditorEx.CustomFloatProperty(Next, property.FindPropertyRelative("FloatData").GetArrayElementAtIndex(PosInList), Content);
                                        break;
                                    case 4:
                                        Next = EditorEx.NewProperty(Next, property.FindPropertyRelative("StringData").GetArrayElementAtIndex(PosInList), Content);
                                        break;
                                    default: Debug.Log("编辑器错误"); break;
                                }

                            }
                        }


                        if (scope.changed) {
                            Debug.Log("Test");
                        }
                    }
                }
            }


        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        ItemStore itemStore = null;

        ItemNodeDynamic Node = ((ItemNodeDynamic)EditorEx.GetObjectByPath(property.serializedObject.targetObject, property.propertyPath, 4));
        itemStore = StaticPath.ItemLoad[Node.ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemType, Node.ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemID];
        //}
        //else if (property.serializedObject.targetObject is ItemOnTheGround) {
        //    ItemOnTheGround item = ((ItemOnTheGround)property.serializedObject.targetObject);
        //    itemStore = item.StaticItemStore;

        if (itemStore != null) {
            if (IsOpen)
                return EditorEx.NormalHeightByNum(itemStore.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.RuntimePacks.Count + 1);
            else
                return EditorEx.NormalHeightByNum(1);
        }
        return EditorEx.NormalHeightByNum(1);
    }
}
