using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class AllGun
{
    [Item(ItemType.Gun, 1)]
    public class AK_47 : GunStatic
    {
        public AK_47() {
            this.MagazineTypes = new List<int>() { 2 };



        }
       
    }



    [Item(ItemType.Gun, 2)]
    public class SACM_1935A : GunStatic
    {
        public SACM_1935A(){
            this.MagazineTypes = new List<int>() { 2 };


        }

    }

}




public class GunState : StateBase
{
    public GunState(Item item) : base(item) { }




    public ShootMode ShootMode = ShootMode.OneByOne;
    [LoadProperties(PropertyType.Static)]
    public BulletType BulletType { get => (BulletType)This.Get(nameof(BulletType)); set => This.Set(nameof(BulletType), (int)value); }
    [LoadProperties(PropertyType.Static)]
    public int ShootBulletNum { get => (int)This.Get(nameof(ShootBulletNum)); set => This.Set(nameof(ShootBulletNum), value); }
    [LoadProperties(PropertyType.Static)]
    public int SmokeType { get => (int)This.Get(nameof(SmokeType)); set => This.Set(nameof(SmokeType), value); }
    [LoadProperties(PropertyType.Static)]
    public float Firing_Rate { get => (float)This.Get(nameof(Firing_Rate)); set => This.Set(nameof(Firing_Rate), value); }
    [LoadProperties(PropertyType.Static)]
    public float Firing_Dmg { get => (float)This.Get(nameof(Firing_Dmg)); set => This.Set(nameof(Firing_Dmg), value); }
    [LoadProperties(PropertyType.Static)]
    public float Firing_Shift { get => (float)This.Get(nameof(Firing_Shift)); set => This.Set(nameof(Firing_Shift), value); }
    [LoadProperties(PropertyType.Static)]
    public float Firing_Force { get => (float)This.Get(nameof(Firing_Force)); set => This.Set(nameof(Firing_Force), value); }
}
public enum ShootMode
{
    OneByOne = 0,
    Muliti = 1,
    Liquid = 2
}