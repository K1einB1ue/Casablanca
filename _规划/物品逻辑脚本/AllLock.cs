using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllLock {


    [Item(ItemType.Tool, 1)]
    public class DoorLock : LockStatic
    {
        /*
        public DoorLock() : base((int)ItemType.Tool, 1) {
            this.ItemStateBool.Displayable = true;
        }
        */
        /*
        static DoorLock() {
            Items.AddGen<DoorLock>(ItemType.Tool, 1);
        }
        */
        public DoorLock() {}

    }

}
