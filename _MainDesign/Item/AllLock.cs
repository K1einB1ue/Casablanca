using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllLock {


    [Item(ItemType.Tool, 1)]
    public class NormalLock : LockStatic
    {
        public NormalLock() { }

        public override bool CanGetNormally => !((this.Outercontainer is Door) && this.LockState.Locking);
        public override void Use6(Item item, out Item itemoutEX) {
            if (!this.LockState.Locking) {
                if (item == Items.Empty) {
                    this.LockState.Locking = true;
                }
            }
            else if (this.LockState.Locking) {
                Debug.Log("钥匙未实现");
            }
            itemoutEX = item;
        }

    }

}
