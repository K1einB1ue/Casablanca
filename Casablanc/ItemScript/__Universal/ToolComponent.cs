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
                ((Item_Detail)ItemIn).Info_Handler.Item_Property.ItemRuntimeProperties.GetWays__Initial = ItemStaticProperties.GetWays.Hand;
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
        item.threw(Math.normalize(((ItemPlayer)PlayerManager.Main).GetHanding()) * 1000 * (Math.scalarization(((ItemPlayer)PlayerManager.Main).GetHanding()) / 1000.0f) * forceRate);
        RotateLock(item);
    }

    public static void RotateLock(Item item) {
        ((Item_Detail)item).Info_Handler.Instance.GetComponent<Rigidbody>().constraints =
             RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }

}
