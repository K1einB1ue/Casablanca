using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllMaterial
{

    [Item(ItemType.Materials, 1)]
    public class Water : ItemStatic {
        public Water(){}


        public override void Collision(Collision collision) {
            this.Info_Handler.BeDmged(100);
        }
        public override void Collision(Item item) {
            
        }
    }


    [Item(ItemType.Materials, 2)]
    public class Fire: ItemStatic
    {

        public Fire() { }


    }
    [Item(ItemType.Materials,5)]
    public class Steam : ItemStatic
    {
        public Steam() {

        }
    }
}






