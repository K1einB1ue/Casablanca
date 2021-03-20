using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllBuilding
{

    [Item(ItemType.Building, 1)]
    public class NormalDoor : DoorStatic
    {
        Item Lock { get => this.Ex_GetItem(0); set => this.Ex_SetItem(0, value); }
        public NormalDoor() : base(1) { }

        public override void Use6(Item item, out Item itemoutEX) {
            this.ActiveLikeADoorWithALock(0, item, out Item itemoutex);
            itemoutEX = itemoutex;
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

            UseBind(6, ((Door)this).ActiveLikeADoor);
        }



    }


    [Item(ItemType.Building, 2,false)]
    public class WoodenWindow: BuildingStatic<Window>
    {
        public WoodenWindow() : base(0) { }
    }


    [Item(ItemType.Building, 3)]
    public class IronDoor: DoorStatic //BuildingStatic<Door>
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
    public class DustBin : BuildingStatic<Drawer>
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
    public class Gate : DoorStatic
    {
        Item Lock { get => this.Ex_GetItem(0); set => this.Ex_SetItem(0, value); }

        public Gate() : base(1) { }

        public override void Use6(Item item, out Item itemoutEX) {
            this.ActiveLikeADoorWithALock(0, item, out Item itemoutex);
            itemoutEX = itemoutex;
        }
        public override void __SynchronizationAfterItemPropertyConstructor() {       
            this.Animator.SetBool("Open", this.DoorState.Open);
            this.Animator.SetTrigger("Init");

            UseBind(6, ((Door)this).ActiveLikeADoor);
        }
    }


    [Item(ItemType.Building, 6)]
    public class IronBarrel : BuildingStatic<Drawer>
    {

        public IronBarrel() : base(10){ }


    }
}
