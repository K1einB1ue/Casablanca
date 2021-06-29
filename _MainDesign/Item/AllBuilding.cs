using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllBuilding
{

    [Item(ItemType.Building, 1)]
    public class NormalDoor : DoorBase
    {
        public static string AnimationParam = "Open";
        Item Lock { get => this.Container_GetItem(0); set => this.Container_SetItem(0, value); }
        public NormalDoor() : base(1) { }

        public override void Use6(Item item, out Item itemoutEX) {
            ((ItemTimer)this).Use6Timer.ActiveLikeWithLock(this, 0, item, out var itemoutEx);
            itemoutEX = itemoutEx;
        }

        public override void RenderUpdate() {
            if (Instance) {
                for (int i = 0; i < this.Item_UI_Handler.MaterialPack.materials.Count; i++) {
                    if (this.Lock != Items.Empty) {
                        this.Item_UI_Handler.MaterialPack.materials[i].SetFloat("CantGet", ((Lock)this.Lock).LockState.Locking ? 1 : 0);
                    }
                    else {
                        this.Item_UI_Handler.MaterialPack.materials[i].SetFloat("CantGet", 0);
                    }
                    this.Item_UI_Handler.MaterialPack.materials[i].SetFloat("Rate", 1.0f - this.Item_UI_Handler.HPrate);
                    this.Item_UI_Handler.MaterialPack.materials[i].SetFloat("Cover", this.Outestcontainer is AllContainer.CharacterStaticBag ? 1.0f : 0.0f);
                    if (Input.GetKey(KeyCode.P) || this.Item_Logic_Handler.BeSelected) {
                        this.Item_UI_Handler.MaterialPack.materials[i].SetFloat("Framed", 1.0f);
                    }
                    else {
                        this.Item_UI_Handler.MaterialPack.materials[i].SetFloat("Framed", 0.0f);
                    }
                }
            }
        }


        public override void __SynchronizationAfterItemPropertyConstructor() {
            this.Animator.SetBool("Open", this.DoorState.Open);
            this.Animator.SetTrigger("Init");

            UseBind(6, () => { this.DoorState.Open = this.AcitveLikeTwoState(AnimationParam); });
        }



    }

    [Item(ItemType.Building, 3)]
    public class IronDoor: DoorBase //BuildingStatic<Door>
    {
        public IronDoor() : base(1) {
            this.ItemIntro = new ItemIntro("讲道理这玩意怎么破坏嘛...\n十分坚硬的铁门");
        }

        public override void Use6(Item item,out Item itemoutEX) {
            Debug.Log("拜托,这已经不在我的业务范围内了");
            itemoutEX = item;
        }
        public override void Death() {
            base.Death();
            Debug.Log("Wow 还是造成了些效果的 吧..");
        }
    }


    [Item(ItemType.Building, 4)]
    public class DustBin : DrawerBase
    {
        public DustBin() : base(1) { }

        public override void Use6(Item item,out Item itemoutEX) {
            if (item == Items.Empty) {
                if (this.ContainerState.Contents[0] != null) {
                    itemoutEX = this.ContainerState.Contents[0];
                    this.ContainerState.Contents[0].Outercontainer.DelItem(this.ContainerState.Contents[0]);
                }
                else {
                    itemoutEX = Items.Empty;
                }
            }
            else {
                if(this.ContainerState.Contents[0] == null) {
                    ((Container)this).AddItem(item);
                    itemoutEX = Items.Empty;
                    
                }
                else {
                    itemoutEX = item;
                }
            }
        }


    }


    [Item(ItemType.Building, 5)]
    public class Gate : DoorBase
    {
        public static string AnimationParam = "Open";
        Item Lock { get => this.Container_GetItem(0); set => this.Container_SetItem(0, value); }

        public Gate() : base(1) { }

        public override void Use6(Item item, out Item itemoutEX) {
            ((ItemTimer)this).Use6Timer.ActiveLikeWithLock(this, 0, item, out var itemoutEx);
            itemoutEX = itemoutEx;
        }
        public override void __SynchronizationAfterItemPropertyConstructor() {       
            this.Animator.SetBool(AnimationParam, this.DoorState.Open);
            this.Animator.SetTrigger("Init");

            UseBind(6, () => { this.DoorState.Open = this.AcitveLikeTwoState(AnimationParam); });
        }
    }


    [Item(ItemType.Building, 6)]
    public class IronBarrel : DrawerBase
    {

        public IronBarrel() : base(10){ }


    }


    [Item(ItemType.Building,7)]
    public class NormalWindow : WindowBase
    {
        public static string AnimationParam = "Open";
        Item Lock { get => this.Container_GetItem(0); set => this.Container_SetItem(0, value); }

        public NormalWindow() : base(1) { }

        public override void Use6(Item item, out Item itemoutEX) {
            ((Item)this).Use6Timer.ActiveLikeWithLock(this, 0, item, out var itemoutEx);
            itemoutEX = itemoutEx;
        }

        public override void __SynchronizationAfterItemPropertyConstructor() {
            this.Animator.SetBool(AnimationParam, this.WindowState.Open);
            this.Animator.SetTrigger("Init");

            this.UseBind(6, () => { this.WindowState.Open = this.AcitveLikeTwoState(AnimationParam); });
        }

    }
}
