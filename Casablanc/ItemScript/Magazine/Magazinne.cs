using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UIMagazine
{
    float GetBulletRate();
}
public interface Magazine
{
    MagazineState magazineState { get; set; }
    Item bullet { get; set; }
    bool GetBulletUse(int use);
    void GetBulletIntoMagazine(Bullet bullet);
    void SetMagazinesize(int held, int max);
    void SetMagazinesize(int max);
    void TryLoad();
}
public class MagazineStatic : ContainerStatic, Magazine, UIMagazine
{
    public Item bullet { 
        get {
            return this.ContainerState.Contents[0];
        } 
        set {
            ((ScriptContainer)this).SetItem(0, value);
        } 
    }
    public MagazineStatic() : base(1) {}

    public MagazineState magazineState { 
        get {
            if (this.magazinestate == null) {
                this.magazinestate = new MagazineState();
            }
            return this.magazinestate;
        } 
        set {
            this.magazinestate = value;
        } 
    }
    private MagazineState magazinestate;
    bool Magazine.GetBulletUse(int use) {
        return (((Container)this).DecMagazine(0, use));
    }
    void Magazine.GetBulletIntoMagazine(Bullet bullet) {
        this.ContainerState.Contents[0].Trysum((Item)bullet);
    }
    public void SetMagazinesize(int held, int max) {
        this.bullet.Item_Held_Handler.SetMax(max);
        this.bullet.Item_Held_Handler.SetHeld(held);
        this.containermax = max;
    }
    public void SetMagazinesize(int max) {
        this.bullet.Item_Held_Handler.SetMax(max);
        this.containermax = max;
    }


    public override void Use1() {
        ToolComponent.threw(this);
    }
    public override void Use6(Item item, out Item itemoutEX) {
        if (item.Type == ItemType.Bullet) {
            if (item.ID == this.magazinestate.BulletID) {
                if (this.bullet == Items.Empty) {
                    this.bullet = item;
                    this.SetMagazinesize(this.bullet.Held, this.containermax);
                    item.Destory();
                    item.Item_Status_Handler.GetWays = GetWays.Tool;
                    itemoutEX = Items.Empty;
                    return;
                }
                else {
                    this.bullet.Trysum(item);
                    itemoutEX = item;
                    return;
                }
            }
        }
        itemoutEX = item;
    }

    float UIMagazine.GetBulletRate() {
        if (this.ContainerState.Contents[0] != Items.Empty) {
            return (float)this.bullet.Held / (float)this.bullet.Item_Held_Handler.HeldMax;
        }
        else {
            return 0;
        }
    }


    List<Item> FindMarchBullet() {
        Container temp = this.Outercontainer;
        return temp.FindMatch(item => {
            if (item.Type == ItemType.Bullet && item.ID == this.magazineState.BulletID) {
                 return true;
            }
            return false;
        });
    }

    public virtual void TryLoad() {
        List<Item> temp = this.FindMarchBullet();
        for (int i = 0; i < temp.Count && this.bullet.Held < this.containermax; i++) {
            if (this.bullet == Items.Empty) {
                if (temp[i].Held <= this.containermax) {
                    ((Container)this).SetItem(0, temp[i]);
                }
                else {
                    Item tmp = Items.GetItemByItemTypeAndItemIDWithoutItemProperty(ItemType.Bullet, this.magazineState.BulletID);
                    tmp.Item_Held_Handler.SetHeld(this.containermax);
                    temp[i].Item_Held_Handler.Decheld(this.containermax);
                    this.bullet = tmp;
                }
                this.SetMagazinesize(this.bullet.Held, this.containermax);
            }
            else {
                this.bullet.Trysum(temp[i]);
            }
        }
    }

    public override void Use3() {
        this.TryLoad();
    }
}

public class MagazineState
{
    public int BulletID => (int)this.BulletType;
    public BulletType BulletType = BulletType.Empty;
}


