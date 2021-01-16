using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ToolComponent
{
    public static void None() { }
    public static void contain(Container This, Item ItemIn, out Item ItemoutEX, Action Begin,Action Normal,Action Strange) {
        Begin();
        if (!object.Equals(ItemIn, This)) {
            if (This.AddItem(ItemIn)) {
                ItemIn.Item_Status_Handler.GetWays = GetWays.Hand;
                ItemoutEX = Items.Empty;
                Normal();
            }
            else {
                ItemoutEX = ItemIn;
            }
        }
        else {
            ItemoutEX = ItemIn;
            Strange();
        }
    }

    public static void threw(Item item) {
        threw(item, 1.0f);
    }

    public static void threw(Item item,float forceRate) {
        if (item.Outercontainer.ID == 0) {
            Character character = ((AllContainer.CharacterStaticBag)item.Outercontainer).Character;
            item.Drop(item.Instance.transform.position);
            item.Instance.GetComponent<Rigidbody>().AddForce(MathEx.normalize(CharacterManager.Main.Info_Handler.Handing) * 40000 * (MathEx.scalarization(CharacterManager.Main.Info_Handler.Handing) / 1000.0f) * forceRate);
            item.Item_Status_Handler.GetWays = GetWays.Hand;
            character.Bag.DelItem(item);
            character.Info_Handler.HeldUpdate();
            RotateLock(item);
        }     
    }

    public static void RotateLock(Item item) {
        item.Info_Handler.Instance.GetComponent<Rigidbody>().constraints =
        RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }

}
