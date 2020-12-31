using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musket : GunStatic
{


    public Musket(){
        this.magazine = new MagazineStatic();
        this.magazine.Outercontainer = this.Outercontainer;
        ((ScriptContainer)this).StoragMethod.storagMethod = StoragMethod.Ignore;
        ((ScriptContainer)this).StoragMethod.IgnoreMap = new List<int>(0);
    }


    public override void TryReload() {
        ((Magazine)this.magazine).TryLoad();
    }

    public override List<Item> FindMarchMagazine() {
        return null;
    }

    public override void Use6(Item item, out Item itemoutEX) {
        itemoutEX = item;
    }
}
