using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class LockStatic : ItemStatic, ILock
{
    LockState LockState = new LockState();
    List<int> ILock.Mapping { get => this.LockState.Mapping; set { this.LockState.Mapping = value; } }

    bool ILock.Locking { get; set; }
    public LockStatic() { }
    bool ILock.Match(Key Key) {
        if (((ILock)this).Mapping.Contains(Key.Mapping)) {
            return true;
        }
        else {
            return false;
        }
    }

    public override void Use2() {
    }
    void ILock.Lock(Item Target, out Item itemIn_, out Item itemOu_) {
        itemIn_ = this;
        itemOu_ = Items.Empty;
    }

}
public interface ILock
{
    List<int> Mapping { get; set; }
    bool Match(Key Key);
    void Lock(Item Target, out Item itemIn, out Item itemOu);
    bool Locking { get; set; }

}


public class LockState
{
    public List<int> Mapping = new List<int>();
}