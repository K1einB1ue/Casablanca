using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllMaterial
{

    [Item(ItemType.Materials, 1)]
    public class Water : ItemStatic {
        /*
        static Water() {
            Items.AddGen<Water>(ItemType.Materials, 1);
        }
        */
        public Water(){}
        /*
        public Water() : base(7, 1) { }
        */

        public override void Collision(Collision collision) {
            this.Info_Handler.BeDmged(100);
        }
        public override void Collision(Item item) {
            
        }
    }


    [Item(ItemType.Materials, 2)]
    public class Fire: ItemStatic
    {
        /*
        static Fire() {
            Items.AddGen<Fire>(ItemType.Materials, 2);
        }
        */
        public Fire() { }
        /*
        public Fire() : base(7, 2) { }
        */

        public override void Trigger(Player player) {
            ((ValuePlayer)player).DecHP(0.1f);
        }
    }
    [Item(ItemType.Materials,5)]
    public class Steam : ItemStatic
    {
        public Steam() {

        }
    }
}






