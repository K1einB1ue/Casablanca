using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public interface UIGun
{
    float GetBulletRate();
}
public interface GunBool
{
    bool IsLegalMagazine(Item magazine);
}
public interface Gun
{
    GunState GunState { get; }
    int BulletKind { get; set; }
    Item magazine { get; set; }
    Item GetPushinMagazine(Item magazine);
    Item GetPulloutMagazine();
    Vector3 GetOutPointPos();
    Vector3 GetLinePointPos();
    Vector3 GetRandom();
    Vector3 GetParticle();
    void SetinitMagazine(Magazine magazine);
    void update();
    void TryReload();
    void TryFocus();
    void TryFire();
    List<Item> FindMarchMagazine();

}

public abstract class GunStatic:ContainerBase,Gun, UIGun, GunBool
{
    public Item magazine { get => this.Container_GetItem(0); set => this.Container_SetItem(0, value); }

    public int BulletKind
    {
        get {
            return (int)this.GunState.BulletType;
        }
        set {
            this.GunState.BulletType = (BulletType)(value + 1);
        }
    }
    public GunState GunState { get { gunState ??= new GunState(this); return gunState; } set => gunState = value; }
    private GunState gunState;
    public List<int> MagazineTypes;

    public GunStatic():base(1){ }
     
    public virtual void TryFire(){
        if (this.magazine != Items.Empty) {
            if (this.GunState.ShootMode == ShootMode.OneByOne) {
                if (((Magazine)this.magazine).BulletUse(1)) {
                    this.Fire();
                    for (int i = 0; i < 5; i++) {
                        this.Smoke();
                    }
                }
            }else if (this.GunState.ShootMode == ShootMode.Muliti) {
                if (((Magazine)this.magazine).BulletUse(this.GunState.ShootBulletNum)) {
                    for (int i = 0; i < this.GunState.ShootBulletNum; i++) {
                        this.Fire();
                        for (int j = 0; j < 5; j++) {
                            this.Smoke();
                        }
                    }
                }
            }
            else if (this.GunState.ShootMode == ShootMode.Liquid) {
                Debug.LogError("禁止使用液体枪,原因:还未开发.");
            }
        }
    }
    public void TryFocus(){
        throw new System.NotImplementedException();
    }
    public virtual void TryReload(){
        if (this.Item_Status_Handler.DisplayWays.Display_things&&this.magazine.Item_Status_Handler.DisplayWays.Displayable) {//如果弹夹是外暴露的
            if (this.magazine != Items.Empty) {
                if (this.magazine.Info_Handler.IsInstanced) {
                    this.magazine.Item_Status_Handler.GetWays = GetWays.Hand;
                    this.magazine.Drop(new Vector3());
                }
                this.magazine.Outercontainer.DelItem(this.magazine);
            }
            else if (this.Outercontainer != null) {
                List<Item> magazines = ((Gun)this).FindMarchMagazine();
                magazines.Sort((Item x, Item y) => { return ((Magazine)y).Bullet.Held.CompareTo(((Magazine)x).Bullet.Held); });
                if (magazines.Count != 0) {
                    this.magazine = magazines[0];
                }
            }
        }
        else{//如果弹夹是内嵌的
            if (this.magazine != Items.Empty) {
                if (!((Item)this.magazine.Outercontainer).Outercontainer.AddItem(this.magazine)) {
                    this.magazine.Drop(((Item)(this.magazine).Outercontainer).Info_Handler.Instance.transform.position);
                }
                this.magazine.Outercontainer.DelItem(this.magazine);
            }
            else if (this.Outercontainer != null) {
                List<Item> magazines = ((Gun)this).FindMarchMagazine();
                magazines.Sort((Item x, Item y) => { return ((Magazine)y).Bullet.Held.CompareTo(((Magazine)x).Bullet.Held); });
                if (magazines.Count != 0) {
                    this.magazine = magazines[0];
                }
            }
        }
    }
    protected virtual void Fire(){
        ((Bullet)((Magazine)this.magazine).Bullet).Shoot(this);
    }
    public virtual List<Item> FindMarchMagazine() {
        Container temp = this.Outercontainer;
        return temp.FindMatch(item=> {
                if(item.Type== ItemType.Magazine&&((GunBool)this).IsLegalMagazine(item)) {
                    if (((Magazine)item).MagazineState.BulletID == this.BulletKind) {
                        return true;
                    }
                }
                return false;           
            });
    }

    public override void Use6(Item item, out Item itemoutEX) {
        if (item.Type == ItemType.Magazine) {
            if (((Magazine)item).MagazineState.BulletID == this.BulletKind&& ((GunBool)this).IsLegalMagazine(item)) {
                if (this.magazine == Items.Empty) {
                    this.magazine = item;
                    itemoutEX = Items.Empty;
                    return;
                }
            }
        }
        itemoutEX = item;
    }
    public override void SetItem(int Pos, Item item) {
        base.SetItem(Pos, item);
        item.Item_Status_Handler.GetWays = GetWays.Tool;
    }
    public override void Use3() {
        TryReload();   
    }
    public override void Use2() {
        return;
    }
    public override void Use1() {
        TryFire();
    }
    public override void __TimerOverride(ActionTimer Use1, ActionTimer Use2, ActionTimer Use3, ActionTimer Use4, ActionTimer Use5, ActionTimer Use6) {
        Use1.SetTimer(() => { return 60 / this.GunState.Firing_Rate; });
    }
    Vector3 Gun.GetRandom() {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    Vector3 Gun.GetOutPointPos() {
        return Info_Handler.Instance.transform.Find("OutPoint").position;
    }
    Vector3 Gun.GetLinePointPos() {
        return Info_Handler.Instance.transform.Find("LinePoint").position;
    }
    Vector3 Gun.GetParticle() {
        return Info_Handler.Instance.transform.Find("Particle").position;
    }
    Item Gun.GetPulloutMagazine() {
        if (this.ContainerState.Contents[0] == Items.Empty) {
            return Items.Empty;
        }
        return this.ContainerState.Contents[0];
    }
    public virtual Item GetPushinMagazine(Item magazine) {
        if (magazine.Type == ItemType.Magazine) {
            if ((((Magazine)magazine).Bullet).ID == this.BulletKind) {
                if (this.magazine!=Items.Empty) {
                    this.magazine = magazine;
                    return Items.Empty;
                }            
            }
        }
        return magazine;
    }
    public virtual void SetinitMagazine(Magazine magazine) {
        this.magazine = (Item)magazine;
        this.BulletKind = magazine.Bullet.ID;
    }
    //*****************************UIGUN*******************************************//
    public virtual float GetBulletRate() {
        if (this.ContainerState.Contents[0]!=Items.Empty) {
            return (float)((Container)this.magazine).GetContainerState().Contents[0].Held / (float)((Container)this.magazine).GetContainerState().Contents[0].Item_Held_Handler.HeldMax;
        }
        else {
            return 0;
        }
    }

    bool GunBool.IsLegalMagazine(Item magazine) {
        for(int i = 0; i < this.MagazineTypes.Count; i++) {
            if(this.MagazineTypes[i]== magazine.ID) {
                return true;
            }
        }
        return false;
    }
    private void Smoke() {
        int Mark = StaticPath.BulletSmokePool_Ram.__UsePoolByID(this.GunState.SmokeType);
        StaticPath.BulletSmokePool_Ram._GetGameObjectRef(this.GunState.SmokeType, Mark, out GameObject gameObject);
        if (gameObject.TryGetComponent<GunFireSmoke>(out GunFireSmoke GunFireSmoke)) {
            GunFireSmoke.ID = this.GunState.SmokeType;
            GunFireSmoke.Mark = Mark;
        }
        gameObject.transform.position = ((Gun)this).GetParticle();
        gameObject.GetComponent<VisualEffect>().Play();
        gameObject.SetActive(true);
    }
}










