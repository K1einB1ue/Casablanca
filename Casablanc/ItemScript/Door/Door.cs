using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Door : Container
{
    public DoorState DoorState { get; set; } 
}
public abstract class DoorBase : ContainerBase, Door
{
    public DoorState DoorState { get { doorState ??= new DoorState(this); return doorState; } set => doorState = value; }
    private DoorState doorState;

    public DoorBase(int size) : base(size) { }
    

}

public class DoorState : StateBase
{
    public DoorState(Item item) : base(item) { }

    [LoadProperties(PropertyType.Runtime)]
    public bool Fiexible { get => (bool)This.Get(nameof(Fiexible)); set => This.Set(nameof(Fiexible), value); }
    [LoadProperties(PropertyType.Runtime)]
    public bool Open { get => (bool)This.Get(nameof(Open)); set => This.Set(nameof(Open), value); }
    [LoadProperties(PropertyType.Static)]
    public bool HasPeepHoles { get => (bool)This.Get(nameof(HasPeepHoles)); set => This.Set(nameof(HasPeepHoles), value); }
    [LoadProperties(PropertyType.Static)]
    public bool HasPeepHolesBroken { get => (bool)This.Get(nameof(HasPeepHolesBroken)); set => This.Set(nameof(HasPeepHolesBroken), value); }
    

}
