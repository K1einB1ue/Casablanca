using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllMaterial
{

    [Item(ItemType.Materials, 1)]
    public class Water : ItemBase {
        public Water(){}


        public override void CollisionEnter(Collision collision) {
            this.Info_Handler.BeDmged(100);
        }
        public override void CollisionEnter(Item item) {
            
        }
    }


    [Item(ItemType.Materials, 2)]
    public class Fire: ItemBase
    {

        public Fire() { }


    }
    [Item(ItemType.Materials,5)]
    public class Steam : ItemBase
    {
        public Steam() {

        }
    }
}






