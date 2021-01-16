using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllBuilding
{
    
    [Item(ItemType.Building,1)]
    public class WoodenDoor : BuildingStatic<Door>
    {
        Item Lock { 
            get {
                if (this.ContainerState==null) {
                    this.ContainerState = new ContainerState(this.ContainerState.size);
                }
                return this.ContainerState.Contents[0];
            }
            set {
                ((ScriptContainer)this).SetItem(0, value);
            } 
        }
        /*
        static WoodenDoor() {
            Items.AddGen<WoodenDoor>(ItemType.Building, 1);
        }
        */
        public WoodenDoor() : base(1) { }
        /*
        public WoodenDoor() : base(5, 1, 1) {
            this.Display = true;

        }
        */
        public override void Use6(Item item,out Item itemoutEX) {
            if (this.Lock != null) {
                if (((ILock)this.Lock).Locking) {
                    itemoutEX = item;
                }
                else {
                    //((ItemScript)this).GetAnimator().
                    itemoutEX = item;
                }
            }
            else {
                //((ItemScript)this).GetAnimator().
                itemoutEX = item;
            }
            
        }
    }


    [Item(ItemType.Building, 2,false)]
    public class WoodenWindow: BuildingStatic<Window>
    {
        static WoodenWindow() {
            //Items.AddGen<WoodenWindow>(ItemType.Building, 2);
        }
        public WoodenWindow() : base(0) { }
    }


    [Item(ItemType.Building, 3)]
    public class IronDoor: BuildingStatic<Door>
    {
        /*
        static IronDoor() {
            Items.AddGen<IronDoor>(ItemType.Building, 3);
        }
        */
        public IronDoor() : base(1) {
            this.ItemIntro = new ItemIntro("讲道理这玩意怎么破坏嘛...\n十分坚硬的铁门");
        }
        /*
        public IronDoor() : base(5, 3, 1) {
            this.Display = true;         
        }
        */
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
        /*
        static DustBin() {
            Items.AddGen<DustBin>(ItemType.Building, 4);
        }
        */
        public DustBin() : base(1) { }
        /*
        public DustBin() : base(5, 4, 1) {
            this.Display = false;
        }
        */
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
    public class Gate : BuildingStatic<Door>
    {
        Item Lock
        {
            get {
                if (this.ContainerState == null) {
                    this.ContainerState = new ContainerState(this.ContainerState.size);
                }
                return this.ContainerState.Contents[0];
            }
            set {
                ((ScriptContainer)this).SetItem(0, value);
            }
        }
        /*
        static Gate() {
            Items.AddGen<Gate>(ItemType.Building, 5);
        }
        */
        public Gate() : base(1) { }
        /*
        public Gate() : base(5, 5, 1) {
            this.Display = true;

        }
        */
        public override void Use6(Item item, out Item itemoutEX) {
            itemoutEX = item;
            if (!((IDoor)this.Building).GetDoorState().Open) {
                ((ItemScript)this).GetAnimator().SetBool("Open", true);
                ((ItemScript)this).GetAnimator().SetTrigger("Change");
                ((IDoor)this.Building).GetDoorState().Open = true;
            }
            else {
                ((ItemScript)this).GetAnimator().SetBool("Open", false);
                ((ItemScript)this).GetAnimator().SetTrigger("Change");
                ((IDoor)this.Building).GetDoorState().Open = false;
            }
        }
    }


    [Item(ItemType.Building, 6)]
    public class IronBarrel : BuildingStatic<Drawer>
    {
        /*
        public IronBarrel() : base(5, 6, 10) {
            this.Display = false;
        }
        */
        /*
        static IronBarrel() {
            Items.AddGen<IronBarrel>(ItemType.Building, 6);
        }
        */
        public IronBarrel() : base(10){ }


    }
}
