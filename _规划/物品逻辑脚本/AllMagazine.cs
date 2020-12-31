using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllMagazine
{
    [Item(ItemType.Magazine, 1)]
    public class Magazine_dot35 : MagazineStatic
    {
        public Magazine_dot35() {
            this.bullet= new AllBullet.Bullet_Empty();
            this.magazineState.BulletType = BulletType.Empty;
        }
        public override void __SynchronizationAfterItemPropertyConstructor() {
            this.SetMagazinesize(30);
        }
    }

    [Item(ItemType.Magazine, 2)]
    public class Magazine_7dot62x39 :MagazineStatic
    {
        public Magazine_7dot62x39() {
            this.bullet = new AllBullet.Bullet_7dot62x39mm();
            this.magazineState.BulletType = BulletType.B7_62;
        }
        public override void __SynchronizationAfterItemPropertyConstructor() {
            this.SetMagazinesize(30);
        }
    }


}   


