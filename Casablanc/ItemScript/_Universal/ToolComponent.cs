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

    public static bool ActiveLikeWithLock(this ActionTimer actionTimer, Container This,int LockPos, Item item, out Item itemoutEX) {
    #region 加钥匙的情况

        if (item is Lock) {
            if(This.Container_GetItem(LockPos) == null || This.Container_GetItem(LockPos) == Items.Empty) {
                This.Container_SetItem(LockPos, item);
                itemoutEX = Items.Empty;
                return false;              
            }
            else {
                goto NormalPipeLine;
            }
        }

    #endregion

    NormalPipeLine:

    #region 状态切换的情况

        itemoutEX = item;
        if (This.Container_GetItem(LockPos) is Lock) {
            if (((Lock)This.Container_GetItem(LockPos)).LockState.Locking) {
                return false;
            }
            else {
                actionTimer.Invoke();
                return true;
            }
        }
        else {
            actionTimer.Invoke();
            return true;
        }

    #endregion
    }
    public static bool AcitveLikeTwoState(this Item item, string AnimationParam) {
        AnimatorStateInfo info = item.Animator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= 0.1f) {
            if (!item.Animator.GetBool(AnimationParam)) {
                item.Animator.SetBool(AnimationParam, true);
                return true;
            }
            else {
                item.Animator.SetBool(AnimationParam, false);
                return false;
            }
        }
        return item.Animator.GetBool(AnimationParam);
    }

}

public static class ContainerEx
{
    /// <summary>
    /// 对背包位置赋值,在赋值前需要绑定ItemProperty
    /// </summary>
    /// <param name="container">容器体</param>
    /// <param name="Pos">需要放置物品的位置</param>
    /// <param name="item">放置的物品</param>
    public static void Container_SetItem(this Container container, int Pos, Item item) {
        if (item.Instance) {
            item.Destory();
        }
        container.SetItem(Pos, item);     
    }
    /// <summary>
    /// 获得特定背包位置的道具.
    /// </summary>
    /// <param name="container">容器体</param>
    /// <param name="Pos">需要获取的物品的位置</param>
    /// <returns></returns>
    public static Item Container_GetItem(this Container container, int Pos) {
        if (container.GetContainerState() == null) {
            ((ScriptContainer)container).SetContainerState(new ContainerState(container.Size));
        }
        return container[Pos];
    }
}