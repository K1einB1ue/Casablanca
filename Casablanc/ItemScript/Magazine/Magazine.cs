using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UIMagazine
{
    float GetBulletRate();
}
public interface Magazine
{
    MagazineState MagazineState { get; set; }
    Item Bullet { get; set; }
    //bool GetBulletUse(int use);
    //void SetMagazinesize(int held, int max);
    //void SetMagazinesize(int max);
    //void TryLoad();


    bool BulletUse(int Usenum);
    void TryReload();
}
public class MagazineBase : ContainerBase, Magazine, UIMagazine
{
    public Item Bullet { 
        get {
            return this.Container_GetItem(0);
        } 
        set {
            this.Container_SetItem(0, value);
        } 
    }
    public MagazineBase() : base(1) {}

    public MagazineState MagazineState { get { magazineState ??= new MagazineState(this); return magazineState; } set => magazineState = value; }
    private MagazineState magazineState;
    //bool Magazine.GetBulletUse(int use) {
    //    return ((Container)this).DecMagazine(0, use);
    //}
    //public void SetMagazinesize(int held, int max) {
    //    this.Bullet.Item_Held_Handler.SetMax(max);
    //    this.Bullet.Item_Held_Handler.SetHeld(held);
    //    this.containermax = max;
    //}
    //public void SetMagazinesize(int max) {
    //    this.Bullet.Item_Held_Handler.SetMax(max);
    //    this.containermax = max;
    //}


    public override void Use1() {
        ToolComponent.threw(this);
    }
    public override void Use6(Item item, out Item itemoutEX) {
        if (item.Type == ItemType.Bullet) {
            //if (item.ID == this.MagazineState.BulletID) {
            //    if (this.Bullet == Items.Empty) {
            //        this.Bullet = item;
            //        this.SetMagazinesize(this.Bullet.Held, this.containermax);
            //        item.Destory();
            //        item.Item_Status_Handler.GetWays = GetWays.Tool;
            //        itemoutEX = Items.Empty;
            //        return;
            //    }
            //    else {
            //        this.Bullet.Trysum(item);
            //        itemoutEX = item;
            //        return;
            //    }
            //}
            
            BulletIN((Bullet)item);
            if (item.Held > 0) {              
                itemoutEX = item;
            }
            else {
                item.Destory();
                itemoutEX = Items.Empty;
            }
            return;
        }
        itemoutEX = item;
    }











    public bool BulletUse(int Usenum) {
        if (Bullet != Items.Empty) {
            int ActuallyDec = this.Bullet.Item_Held_Handler.Decheld(Usenum);
            if (ActuallyDec == 0) {
                this.Bullet = Items.Empty;
                return false;
            }
            return true;
        }
        else {
            return false;
        }
    }

    public void TryReload() {
        List<Item> temp = this.FindMarchBullet();
        for (int i = 0; i < temp.Count && this.Bullet.Held < this.MagazineState.MagazineSize; i++) {
            BulletIN((Bullet)temp[i]);
        }
    }

    private void BulletIN(Bullet bullet) {
        if (bullet.ID == this.MagazineState.BulletID) {
            if (this.Bullet == Items.Empty) {              
                Item tmp = Items.GetItemByItemTypeAndItemIDWithoutItemProperty(bullet);
                tmp.Info_Handler.Binding(new Item_Property(bullet, null));
                tmp.Item_Held_Handler.SetHeld(0);
                tmp.Item_Held_Handler.SetMax(this.MagazineState.MagazineSize);
                this.Bullet = tmp;
                bullet.Item_Held_Handler.Addheld(this.Bullet.Item_Held_Handler.Addheld(bullet.Item_Held_Handler.Decheld(this.MagazineState.MagazineSize)));
                if (bullet.Item_Held_Handler.Held == 0) {
                    bullet.OuterClear();
                }
                else if (bullet.Item_Held_Handler.Held < 0) {
                    bullet.OuterClear();
                    Debug.LogError("潜在的错误可能");
                }
                
            }
            else {
                this.Bullet.Trysum(bullet);
            }
        }
    }


    private IEnumerable<Item> BulletOUT() {
        Item bullet = this.Bullet;
        this.Bullet = Items.Empty;
        while (bullet.Item_Held_Handler.Held > 0) {
            Item tmp = Items.GetItemByItemTypeAndItemIDWithoutItemProperty(bullet);
            if (tmp.Item_Held_Handler.HeldMax < bullet.Item_Held_Handler.HeldMax) {
                tmp.Item_Held_Handler.SetHeld(tmp.Item_Held_Handler.HeldMax);
                bullet.Item_Held_Handler.SetHeld(bullet.Held - tmp.Item_Held_Handler.HeldMax); 
                yield return tmp;
            }
            else {
                yield return bullet;
            }           
        }
    }


















    float UIMagazine.GetBulletRate() {
        if (this.ContainerState.Contents[0] != Items.Empty) {
            return (float)this.Bullet.Held / (float)this.Bullet.Item_Held_Handler.HeldMax;
        }
        else {
            return 0;
        }
    }


    List<Item> FindMarchBullet() {
        Container temp = this.Outercontainer;
        return temp.FindMatch(item => {
            if (item.Type == ItemType.Bullet && item.ID == this.MagazineState.BulletID) {
                 return true;
            }
            return false;
        });
    }

    //public void TryLoad() {
    //    List<Item> temp = this.FindMarchBullet();
    //    for (int i = 0; i < temp.Count && this.Bullet.Held < this.containermax; i++) {
    //        if (this.Bullet == Items.Empty) {
    //            if (temp[i].Held <= this.containermax) {
    //                ((Container)this).SetItem(0, temp[i]);
    //            }
    //            else {
    //                Item tmp = Items.GetItemByItemTypeAndItemIDWithoutItemProperty(ItemType.Bullet, this.magazineState.BulletID);
    //                tmp.Item_Held_Handler.SetHeld(this.containermax);
    //                temp[i].Item_Held_Handler.Decheld(this.containermax);
    //                this.Bullet = tmp;
    //            }
    //            this.SetMagazinesize(this.Bullet.Held, this.containermax);
    //        }
    //        else {
    //            this.Bullet.Trysum(temp[i]);
    //        }
    //    }
    //}

    public override void Use3() {
        //this.TryLoad();
        this.TryReload();
    }
    public override void OnAttach() {
        this.Item_Status_Handler.GetWays = GetWays.Tool; 
    }
}

public class MagazineState:StateBase
{
    public MagazineState(Item item) : base(item) { }
    public int BulletID => (int)this.BulletType;


    [LoadProperties(PropertyType.Static)]
    public BulletType BulletType { get => (BulletType)This.Get(nameof(BulletType)); set => This.Set(nameof(BulletType), (int)value); }

    [LoadProperties(PropertyType.Static)]
    public int MagazineSize { get => (int)This.Get(nameof(MagazineSize)); set => This.Set(nameof(MagazineSize), value); }
    
}


