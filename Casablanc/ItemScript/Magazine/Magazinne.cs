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
        this.bullet.Setheld(held);
        this.bullet.SetMaxheld(max);
        this.containermax = max;
    }
    public void SetMagazinesize(int max) {
        this.bullet.SetMaxheld(max);
        this.containermax = max;
    }


    public override void Use1() {
        ToolComponent.threw(this);
    }
    public override void Use6(Item item, out Item itemoutEX) {
        if (item.TypeGet() == ItemType.Bullet) {
            if (item.IDGet() == this.magazinestate.BulletID) {
                if (this.bullet == Items.Empty) {
                    this.bullet = item;
                    this.SetMagazinesize(this.bullet.Getheld(), this.containermax);
                    item.Destory();
                    ((Item_Detail)item).Info_Handler.Item_Property.ItemRuntimeProperties.GetWays__Initial = ItemStaticProperties.GetWays.Tool;
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
            return (float)this.bullet.Getheld() / (float)this.bullet.GetMaxheld();
        }
        else {
            return 0;
        }
    }


    List<Item> FindMarchBullet() {
        Container temp = ((ItemScript)this).Outercontainer;
        return temp.FindMatch(item => {
            if (item.TypeGet() == ItemType.Bullet && item.IDGet() == this.magazineState.BulletID) {
                 return true;
            }
            return false;
        });
    }

    public virtual void TryLoad() {
        List<Item> temp = this.FindMarchBullet();
        for (int i = 0; i < temp.Count && this.bullet.Getheld() < this.containermax; i++) {
            if (this.bullet == Items.Empty) {
                if (temp[i].Getheld() <= this.containermax) {
                    ((Container)this).SetItem(0, temp[i]);
                }
                else {
                    Item tmp = Items.GetItemByItemTypeAndItemIDWithoutItemProperty(ItemType.Bullet, this.magazineState.BulletID);
                    tmp.Setheld(this.containermax);
                    temp[i].Decheld(this.containermax);
                    this.bullet = tmp;
                }
                this.SetMagazinesize(this.bullet.Getheld(), this.containermax);
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


