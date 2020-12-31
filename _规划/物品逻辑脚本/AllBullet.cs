using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BulletType
{
    Empty = -1,
    buckshot = 0,
    B7_62 = 1,
    B_50 = 2,
    B_40 = 3,
    B5_56 = 4,
    B12_7 = 5,
}
public static class AllBullet
{
    /// <summary>
    /// 注意!:
    /// 1.子弹ID需要与BulletType形成配套
    /// </summary>
    [Item(ItemType.Bullet, -1)]
    public class Bullet_Empty:BulletStatic
    {
        /*
        public Bullet_Empty() : base(2, -1) { }
        */
        /*
        static Bullet_Empty() {
            Items.AddGen<Bullet_Empty>(ItemType.Bullet, -1);
        }
        */
        public Bullet_Empty() { }

    }


    [Item(ItemType.Bullet, 1)]
    public class Bullet_7dot62x39mm : BulletStatic 
    {
        /*
        public Bullet_7dot62x39mm() : base(2, 1) {
            //this.itemGraph.NoRotateX = true;   
        }
        */
        /*
        static Bullet_7dot62x39mm() {
            Items.AddGen<Bullet_7dot62x39mm>(ItemType.Bullet, 1);
        }
        */
        public Bullet_7dot62x39mm() { }
        public override void Use1() {
            ToolComponent.threw(this,0.4f);
        }
        
        
    }
}
