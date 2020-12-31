using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class AllGun
{
    [Item(ItemType.Gun, 1)]
    public class AK_47 : GunStatic
    {
        public AK_47() {
            ((Gun)this).SetinitState(1);


            this.GunState.BulletType = BulletType.B7_62;
            this.GunState.SmokeType = 1;
            this.GunState.MagazineTypes = new List<int>() { 2 };


            this.GunState.Firing_Dmg = 20;
            this.GunState.Firing_Rate = 650;
            this.GunState.Firing_Shift = 0.5f;
            this.GunState.Firing_Force = 2000;
            this.initializeTimer();

        }
       
    }


    [Item(ItemType.Gun, 2)]
    public class SACM_1935A : GunStatic
    {
        public SACM_1935A(){
            ((Gun)this).SetinitState(1);


            this.GunState.BulletType = BulletType.B7_62;
            this.GunState.SmokeType = 1;
            this.GunState.MagazineTypes = new List<int>() { 2 };


            this.GunState.Firing_Dmg = 33;
            this.GunState.Firing_Rate = 25;
            this.GunState.Firing_Shift = 0.5f;
            this.GunState.Firing_Force = 4000;
            this.initializeTimer();
        }

    }

}




public class GunState
{
    /// <summary>
    /// 也就是弹夹的ID
    /// </summary>
    public List<int> MagazineTypes;
    /// <summary>
    /// 子弹类型
    /// </summary>
    public BulletType BulletType = BulletType.B7_62;
    /// <summary>
    /// 烟雾类型
    /// </summary>
    public ShootMode ShootMode = ShootMode.OneByOne;

    public int ShootBulletNum = 1;
    public int SmokeType = 1;
    public float Firing_Rate = 600;
    public float Firing_Dmg = 20;
    public float Firing_Shift = 0.5f;
    public float Firing_Force = 200f;
}
public enum ShootMode
{
    OneByOne = 0,
    Muliti = 1,
    Liquid = 2
}