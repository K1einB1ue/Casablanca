using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization;
using System;
using UnityEditor;

public interface ScriptContainer
{
    StoragMethodAdjustment StoragMethod { get; set; }
    List<ItemNodeDynamic> GetItemNodes();
    void SetContainerState(ContainerState ContainerState);
    int PreItemCount(ref int I);
    void SetItem(int Pos, Item item);
}

public interface Container
{
    bool DecMagazine(int Pos, int num);
    bool DecHeld(int Pos,int num);
    void DropAndDel(Item item, Vector3 pos);
    void DropAllDel();
    void DropAllDel (Vector3 Pos);
    void DropAllDelMatch(Vector3 Pos, Func<Item,bool> action);
    Item FindItem(Item item);
    List<Item> FindItem(ItemType ItemType, int ID);
    List<Item> FindMatch(Func<Item,bool> action);
    void GetItemFormGround(GameObject gameObject);
    void GetItemFormGround(Item item);
    bool CheckSpace();
    void DelItem(Item item);
    bool AddItem(Item item);
    void SetItem(int Pos, Item item);
    void ShowContent();
    int ItemCount();
    int ItemBlockRemain();
    void Exchange(int x, int y);

    //Stack<int> FindItemPos(Item item, int mark);
    ContainerState GetContainerState();
    void UpdateDisplay();
}
public abstract class ContainerStatic:ItemStatic,Container, ScriptContainer
{

    StoragMethodAdjustment ScriptContainer.StoragMethod { get { return this.storagMethodAdjustment; } set { this.storagMethodAdjustment = value; } }
    private StoragMethodAdjustment storagMethodAdjustment = new StoragMethodAdjustment();
    /// <summary>
    /// 开启容器显示
    /// (如果内容物为外挂载,可视,则开启)
    /// </summary>
    public bool HardCore = false;
    public int containermax { get { return this.containerMax; } set { this.Containmax = true; this.containerMax = value; } }
    private int containerMax;
    private bool Containmax = false;
    public ContainerState ContainerState;
    public List<BoxCollider> boxColliders = new List<BoxCollider>();

    public override void update() {
        base.update();    
        CheckEmpty();
        ((Item_Trigger_Handler)this.Info_Handler).Trigger_Display(this.UpdateDisplay);
        //Itemupdate();                                                         //万恶之源 全场加载;
    }
    public override void Use1() {
        ((Container)this).ShowContent();
    }
    public override void Use2() {
        ((Container)this).DropAllDel();
    }
    public ContainerStatic(int size) {
        this.ContainerState = new ContainerState(size);
    }
    public ContainerStatic(ItemType itemType) { }

    protected void CheckEmpty() {
        for(int i = 0; i < ContainerState.size; i++) {
            if (ContainerState.Contents[i] != Items.Empty) {
                if (((Item_Detail)ContainerState.Contents[i]).Info_Handler.Getheld() <= 0) {
                    ContainerState.Contents[i] = Items.Empty;
                }
            }
        }
    }
    bool Container.CheckSpace() {
        for(int i = 0; i < ContainerState.size; i++) {
            if (ContainerState.Contents[i] == Items.Empty) {
                return true;
            }
        }
        return false;
    }
    protected void Itemupdate() {
        for (int i = 0; i < ContainerState.size; i++) {
            ContainerState.Contents[i].update();
        }
    }
    public override void Beheld(Transform transform) {
        base.Beheld(transform);
        if (this.Info_Handler.Item_Property.ItemStaticProperties.DisplayWays.Display_things){
            for (int i = 0; i< this.ContainerState.size; i++) {
                this.ContainerState.Contents[i].Beheld(this.itemGraph.GetInstance().transform.Find("Attach").Find((i + 1).ToString()));
            }
        }
    }
    public override void Instance(Vector3 vector3) {
        base.Instance(vector3);
        if (this.Info_Handler.Item_Property.ItemStaticProperties.DisplayWays.Display_things) {
            for (int i = 0; i < this.ContainerState.size; i++) {
                this.ContainerState.Contents[i].Beheld(this.itemGraph.GetInstance().transform.Find("Attach").Find((i + 1).ToString()));
            }
        }
    }
    public override void Drop(Vector3 vector3) {
        base.Drop(vector3);
        if (this.Info_Handler.Item_Property.ItemStaticProperties.DisplayWays.Display_things) {
            for (int i = 0; i < this.ContainerState.size; i++) {
                this.ContainerState.Contents[i].BeHeldButDrop(this.itemGraph.GetInstance().transform.Find("Attach").Find((i + 1).ToString()));
            }
        }
    }
    public override void Destory() {
        base.Destory();
        if (this.ContainerState != null) {
            for (int i = 0; i < this.ContainerState.size; i++) {
                this.ContainerState.Contents[i].Destory();
            }
        }
    }
    void Container.Exchange(int x, int y) {
        if(x>=0&&x<ContainerState.size&&y>=0&&y<ContainerState.size) {
            Item tmp = this.ContainerState.Contents[x];
            this.ContainerState.Contents[x] = this.ContainerState.Contents[y];
            this.ContainerState.Contents[y] = tmp;
            tmp = null;
        }
    }
    /// <summary>
    /// 会优先清除外部背包 无需手动设置
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    bool Container.AddItem(Item item) {
        for (int i = 0; i < ContainerState.size; i++) {
            if (item.TypeGet() == ContainerState.Contents[i].TypeGet() && item.IDGet() == ContainerState.Contents[i].IDGet()) {
                int HeldRemain = ContainerState.Contents[i].Addheld(item.Getheld());
                if (HeldRemain == 0) {
                    item.Destory();
                    this.ContainerReset(item);
                }
                else {
                    item.Setheld(HeldRemain);
                }
            }
            if (ContainerState.Contents[i]==Items.Empty) {
                ContainerState.Contents[i] = item;
                item.Destory();
                this.ContainerReset(item);
                this.ContainerState.Count += ((ItemScript)item).GetSize();

                return true;
            }
        }
        for (int i = 0; i < this.FindBag().Count; i++) {
            if (((Container)this.FindBag()[i]).AddItem(item)) {
                return true;
            }
        }
        return false;
    }

    void Container.ShowContent() {

    }
    void Container.GetItemFormGround(GameObject gameObject) {
        if (gameObject.GetComponent<ItemOnTheGround>() != null) {
            if (((Item_Detail)gameObject.GetComponent<ItemOnTheGround>().itemOntheGround).Info_Handler.Item_Property.ItemRuntimeProperties.GetWays__Initial== ItemStaticProperties.GetWays.Hand) {
                ((Container)this).AddItem((gameObject.GetComponent<ItemOnTheGround>().itemOntheGround));
            }
            if (((Item_Detail)gameObject.GetComponent<ItemOnTheGround>().itemOntheGround).Info_Handler.Item_Property.ItemRuntimeProperties.GetWays__Initial == ItemStaticProperties.GetWays.Tool){
                Debug.Log("'这需要什么才能拿下来吧.....' 摇头");
            }
        }
    }
    void Container.GetItemFormGround(Item item) {
        if (((Item_Detail)item).Info_Handler.Item_Property.ItemRuntimeProperties.GetWays__Initial== ItemStaticProperties.GetWays.Hand) {
            ((Container)this).AddItem(item);
        }
        if (((Item_Detail)item).Info_Handler.Item_Property.ItemRuntimeProperties.GetWays__Initial == ItemStaticProperties.GetWays.Tool) {
            Debug.Log("'这需要什么工具拿下来吧.....' 摇头");
        }
    }
    List<Item> Container.FindItem(ItemType itemtype,int ID) {                                                                        //强调种类
        List<Item> ItemList = new List<Item> { };
        for(int i = 0; i < ContainerState.size; i++) {
            if (ContainerState.Contents[i].TypeGet() == itemtype && ContainerState.Contents[i].IDGet() == ID) {
                ItemList.Add(ContainerState.Contents[i]);
            }
        }
        for(int i=0;i< FindBag().Count; i++) {
            ItemList=ItemList.Concat(((Container)FindBag()[i]).FindItem(itemtype, ID)).ToList<Item>();
        }
        return ItemList;
    }
    List<Item> Container.FindMatch(Func<Item,bool> action) {
        List<Item> ItemList = new List<Item> { };
        for (int i = 0; i < ContainerState.size; i++) {
            if (action(this.ContainerState.Contents[i])) {
                ItemList.Add(ContainerState.Contents[i]);
            }
        }
        for (int i = 0; i < FindBag().Count; i++) {
            ItemList = ItemList.Concat(((Container)FindBag()[i]).FindMatch(action)).ToList<Item>();
        }
        return ItemList;
    }
    Item Container.FindItem(Item item) {                                                                                    //强调唯一
        for (int i = 0; i < ContainerState.size; i++) {
            if (object.ReferenceEquals(ContainerState.Contents[i],item)) {
                return ContainerState.Contents[i];
            }
        }
        for (int i = 0; i < FindBag().Count; i++) {
            ((Container)FindBag()[i]).FindItem(item);
        }
        return null;
    }
    private List<Item> FindAllItemInThis() {
        List<Item> items = new List<Item>();
        for(int i = 0; i < this.GetContainerState().size; i++) {
            if (GetContainerState().Contents[i]!=Items.Empty) {
                items.Add(GetContainerState().Contents[i]);
            }
        }
        return items;
    }
    /// <summary>
    /// 会清除外背包
    /// </summary>
    /// <param name="item"></param>
    void Container.DelItem(Item item) {
        if (item != Items.Empty) {
            for (int i = 0; i < ContainerState.size; i++) {
                if (object.ReferenceEquals(ContainerState.Contents[i], item)) {
                    ((ItemScript)item).Outercontainer = null;
                    this.ContainerState.Count -= ((ItemScript)item).GetSize();
                    ContainerState.Contents[i] = Items.Empty;
                }
            }
            for (int i = 0; i < FindBag().Count; i++) {
                ((Container)FindBag()[i]).DelItem(item);
            }
            return;
        }
    }
    public void  ExChangeItem(Item itemNeed,Item itemNoNeed) {
        for (int i = 0; i < ContainerState.size; i++) {
            if (ContainerState.Contents[i].TypeGet() == itemNoNeed.TypeGet() && ContainerState.Contents[i].IDGet() == itemNoNeed.IDGet()) {
                this.ContainerState.Contents[i] = itemNeed;
                break;
            }
        }
    }
    int ScriptContainer.PreItemCount(ref int I) {
        int layer = I;
        for(int i = 0; i< ContainerState.size; i++) {;
            if (ContainerState.Contents[i]!=Items.Empty) {
                I++;
            }
        }
        for(int i = 0; i < FindBag().Count; i++) {
            ((ScriptContainer)FindBag()[i]).PreItemCount(ref I);
        }
        if (layer == 0) return I;
        else return 0;
    }
    int Container.ItemCount() {
        int I = 0;
        ref int O = ref I;
        return ((ScriptContainer)this).PreItemCount(ref O);
    }
    public List<Item> FindBag() {
        List<Item> BagList = new List<Item>() { };
        for (int i=0;i<ContainerState.size;i++) {
            if (ContainerState.Contents[i].TypeGet() == ItemType.Container) {
                BagList.Add(ContainerState.Contents[i]);
            }
        }
        return BagList;   
    }
    
    void ScriptContainer.SetContainerState(ContainerState ContainerState) {
        this.ContainerState = ContainerState;
    }
    void Container.DropAllDel() {
        for(int i=0; i < this.FindAllItemInThis().Count; i++) {
            if (this.itemGraph.Instanced) {
                ((Container)this).DropAndDel(this.FindAllItemInThis()[i], this.itemGraph.GetInstance().transform.position);
            }
        }
    }
    void Container.DropAndDel(Item item,Vector3 pos) {
        foreach(Item items in Beforedrop(item)) {
            ((Item_Detail)items).Info_Handler.Item_Property.ItemRuntimeProperties.GetWays__Initial = ItemStaticProperties.GetWays.Hand;
            items.Drop(pos);
        }
        //((Container)this).DelItem(item);
    }
    bool Container.DecHeld(int Pos,int num) {
        if (this.ContainerState.Contents[Pos].Decheld(num)==0) {
            this.ContainerState.Contents[Pos] = Items.Empty;
            return false;
        }
        return true;
    }
    /// <summary>
    /// 消耗弹药数而不消除实体,让规格限定成为可能;
    /// </summary>
    /// <param name="Pos">子弹在背包中的位置</param>
    /// <param name="num">子弹消耗的量</param>
    /// <returns>如果能正常消除正常数量 则返回True</returns>
    bool Container.DecMagazine(int Pos, int num) {
        if (this.ContainerState.Contents[Pos].Decheld(num) == 0) {
            return false;
        }
        return true;
    }
    void Container.DropAllDel(Vector3 Pos) {
        for (int i = 0; i < this.FindAllItemInThis().Count; i++) {
            ((Container)this).DropAndDel(this.FindAllItemInThis()[i], Pos);
        }
    }
    void Container.DropAllDelMatch(Vector3 Pos, Func<Item, bool> func) {
        foreach(Item item in ((Container)this).FindMatch(func)) {
            ((Container)this).DropAndDel(item, Pos);
        }
    }

    public override bool IsContainer() {
        return true;
    }
    public override void Death() {
        ((Container)this).DropAllDel(this.itemGraph.GetInstance().transform.position + Vector3.up * 0.5f);
        ((Item)this).OuterClear();
        GameObject.Destroy(this.itemGraph.GetInstance());
    }
    public override ContainerState GetContainerState() {
        return this.ContainerState;
    }
    int Container.ItemBlockRemain() {
        return this.ContainerState.size - this.ContainerState.Count;
    }

    public override List<ItemNodeDynamic> GetItemNodes() {
        List<ItemNodeDynamic> itemNodes = new List<ItemNodeDynamic>();
        if (((ScriptContainer)this).StoragMethod.storagMethod == StoragMethod.Normal) { 
            if (this.ContainerState != null) {
                for (int i = 0; i < this.ContainerState.size; i++) {
                    ItemNodeDynamic node;
                    node = new ItemNodeDynamic(this.ContainerState.Contents[i]);
                    itemNodes.Add(node);
                    node.ItemContain = this.ContainerState.Contents[i].GetItemNodes();
                }
            }
        }
        else if(((ScriptContainer)this).StoragMethod.storagMethod== StoragMethod.Ignore) {
            if (this.ContainerState != null) {
                for (int i = 0; i < this.ContainerState.size; i++) {
                    ItemNodeDynamic node;
                    if (((ScriptContainer)this).StoragMethod.IgnoreMap.Contains(i)) {
                        if (this.ContainerState.Contents[i].GetContainerState() != null) {
                            node = new ItemNodeDynamic(ContainerState.Contents[i].GetContainerState().Contents[0]);
                        }
                        else {
                            node = new ItemNodeDynamic(Items.Empty);
                        }
                    }
                    else {
                        node = new ItemNodeDynamic(this.ContainerState.Contents[i]);
                    }
                    itemNodes.Add(node);
                    node.ItemContain = this.ContainerState.Contents[i].GetItemNodes();
                }
            }
        }
        return itemNodes;
    }
    public virtual void SetItem(int Pos, Item item) {
        if (((ScriptContainer)this).StoragMethod.storagMethod == StoragMethod.Normal) {
            this.ContainerState.Contents[Pos] = item;
            this.ContainerReset(item);
        }else if (((ScriptContainer)this).StoragMethod.storagMethod == StoragMethod.Ignore) {
            if (((ScriptContainer)this).StoragMethod.IgnoreMap.Contains(Pos)) {
                if (this.ContainerState.Contents[Pos].GetContainerState() != null) {
                    this.ContainerState.Contents[Pos].GetContainerState().Contents[0] = item;
                    this.ContainerReset(item);
                }
                else {
                    Debug.LogError("发生了错误的物品设置:SetItem(int Pos,Item item);");
                }
            }
            else {
                this.ContainerState.Contents[Pos] = item;
                this.ContainerReset(item);
            }
        }
        if (Info_Handler.Binded) {
            if (this.Info_Handler.Item_Property.ItemStaticProperties.DisplayWays.Display_things && ((Item_Detail)item).Info_Handler.Item_Property.ItemStaticProperties.DisplayWays.Displayable) {
                UpdateDisplay();
            }
        }
    }


    public override List<BoxCollider> GetBoxCollider() {
        List<BoxCollider> BoxColliders = new List<BoxCollider>();
        if (((ItemLogic)this).IsInstanced()) {
            BoxColliders=BoxColliders.Concat(this.itemGraph.GetInstance().GetComponents<BoxCollider>()).ToList<BoxCollider>();
        }
        for (int i = 0; i < ContainerState.size; i++) {
            BoxColliders=BoxColliders.Concat(((ItemScript)ContainerState.Contents[i]).GetBoxCollider()).ToList<BoxCollider>();
        }
        return BoxColliders;
    }
    private void ContainerReset(Item item) {
        if (((ItemScript)item).Outercontainer != null) {
            ((ItemScript)item).Outercontainer.DelItem(item);
        }
        ((ItemScript)item).Outercontainer = this;
        if (this.Containmax) {
            item.SetMaxheld(this.containermax);
        }
    }
    private List<Item> Beforedrop(Item item) {
        List<Item> drops = new List<Item>();
        if (item.GetMaxheld() == 1) {
            drops.Add(item);
        }
        else {
            while (item.Getheld() > ((ItemScript)item).GetOriginMax()) {
                item.Decheld(((ItemScript)item).GetOriginMax());
                drops.Add(Items.GetItemByItemTypeAndItemIDWithoutItemProperty(item.TypeGet(), item.IDGet()));
            }
            item.SetMaxheld(((ItemScript)item).GetOriginMax());
            drops.Add(item);
        }
        ((ItemScript)item).Outercontainer.DelItem(item);
        return drops;
    }


    public void UpdateDisplay() {
        if (this.Info_Handler.Item_Property.ItemStaticProperties.DisplayWays.Display_things) {
            if (itemGraph.Instanced) {
                for (int i = 0; i < this.ContainerState.size; i++) {
                    if (((Item_Detail)this.ContainerState.Contents[i]).Info_Handler.Item_Property.ItemStaticProperties.DisplayWays.Displayable) {
                        this.ContainerState.Contents[i].BeHeldButDrop(this.itemGraph.GetInstance().transform.Find("Attach").Find((i + 1).ToString()));
                        if (((ItemLogic)this.ContainerState.Contents[i]).IsContainer()) {
                            if (((Item_Detail)this.ContainerState.Contents[i]).Info_Handler.Item_Property.ItemStaticProperties.DisplayWays.Display_things) {
                                ((Container)this.ContainerState.Contents[i]).UpdateDisplay();
                            }
                        }
                    }
                }
            }
        }
    }



    public void UpdateCollider() {
        if (this.HardCore) {
            if (itemGraph.Instanced) {
                for (int i = 0; i < this.ContainerState.size; i++) {
                    this.boxColliders=this.boxColliders.Concat(((ItemScript)this.ContainerState.Contents[i]).GetBoxCollider()).ToList<BoxCollider>();
                }
            }
        }
    }


}
public class ContainerState
{
    public int Count = 0;
    public int size;
    

    public Item[] Contents {
        get {
            if (this.items == null) {
                this.items = new Item[this.size];
                for (int i = 0; i < size; i++) {
                    this.items[i] = Items.Empty;
                }
            }
            return this.items;
        }
        set {
            this.items = value;
        }
    }
    private Item[] items;

    public ContainerState(int size) {
        this.size = size;
    }


}
[Serializable]
public class ItemPreInstanceInfoPackage {
    public ItemPreInstanceType ItemPreInstanceType = ItemPreInstanceType.Standard;
    public ItemPreInstanceProperties.ItemPreInstanceProperties ItemPreInstanceProperties = new ItemPreInstanceProperties.ItemPreInstanceProperties();
}

public enum ItemStaticDescribeWays {
    ItemStroe,
    ItemTypeAndID,
    ItemDetailStore,
    ItemDetailPack,

}
[Serializable]
public class ItemStaticInfoPackage {
    public ItemStaticDescribeWays itemStaticDescribeWays = ItemStaticDescribeWays.ItemStroe;
    public ItemStore ItemStore;
    public ItemStaticProperties.ItemStaticProperties ItemStaticProperties;

    public ItemStaticProperties.ItemStaticProperties GetItemStaticProperty() {
        if (this.itemStaticDescribeWays== ItemStaticDescribeWays.ItemStroe) {           
            return StaticPath.ItemLoad[this.ItemStore.ItemStaticProperties.ItemType, this.ItemStore.ItemStaticProperties.ItemID].ItemStaticProperties;
        }
        if(this.itemStaticDescribeWays== ItemStaticDescribeWays.ItemTypeAndID) {           
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
        ItemRuntimeProperties = ((Item_Detail)item).GetItemProperty().ItemRuntimeProperties;
    }
}

public class StoragMethodAdjustment
{
    public StoragMethod storagMethod = StoragMethod.Normal;
    public List<int> IgnoreMap;

}
public enum StoragMethod
{
    Normal,
    Ignore
}




