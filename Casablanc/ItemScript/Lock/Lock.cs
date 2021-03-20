using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class LockStatic : ItemBase, Lock
{
    public LockState LockState { get { lockState ??= new LockState(this); return lockState; } set => lockState = value; }
    private LockState lockState;

    public LockStatic() { }

}
public interface Lock
{
    public LockState LockState { get; set; }
}


public class LockState : StateBase
{
    public LockState(Item item) : base(item) { }

    [LoadProperties(PropertyType.Runtime)]
    public bool Locking { get => (bool)This.Get(nameof(Locking)); set => This.Set(nameof(Locking), value); }
    [LoadProperties(PropertyType.Runtime)]
    public int KeyHash { get => (int)This.Get(nameof(KeyHash)); set => This.Set(nameof(KeyHash), value); }
}