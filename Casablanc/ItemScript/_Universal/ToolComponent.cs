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

    public static void ActiveLikeADoor(this Door door) {
        AnimatorStateInfo info = door.Animator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= 0.1f) {
            if (!door.Animator.GetBool("Open")) {
                door.Animator.SetBool("Open", true);
                door.DoorState.Open = true;
            }
            else {
                door.Animator.SetBool("Open", false);
                door.DoorState.Open = false;
            }
        }
    }

    public static void ActiveLikeADoorWithALock(this Door door,int LockPos, Item item, out Item itemoutEX) {
        

        if (item is Lock) {
            if (!((Lock)item).LockState.Locking) {
                ((Container)door).Ex_SetItem(LockPos, item);
                itemoutEX = Items.Empty;
            }
            else {
                Debug.Log("这把锁 已经锁上了 无法套上去");
                itemoutEX = item;
            }
        }
        else {
            if (((Container)door).Ex_GetItem(LockPos) != Items.Empty) {
                if (((Lock)((Container)door).Ex_GetItem(LockPos)).LockState.Locking) {
                    itemoutEX = item;
                    
                }
                else {
                    door.UseInvoke(6);
                    itemoutEX = item;
                }
            }
            else {
                door.UseInvoke(6);
                itemoutEX = item;
            }
        }
    }

}

public static class ContainerEx
{
    public static void Ex_SetItem(this Container container, int Pos, Item item) {
        if (item.Instance) {
            item.Destory();
        }
        container.SetItem(Pos, item);
    }
    public static Item Ex_GetItem(this Container container, int Pos) {
        if (container.GetContainerState() == null) {
            ((ScriptContainer)container).SetContainerState(new ContainerState(container.Size));
        }
        return container[Pos];
    }
}