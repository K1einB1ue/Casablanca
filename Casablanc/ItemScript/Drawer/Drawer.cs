using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Drawer : Container 
{
    DrawerState DrawerState { get; set; }
}
public class DrawerBase : ContainerBase, Drawer
{
    
    public DrawerState DrawerState { get { drawerState ??= new DrawerState(this); return drawerState; } set => drawerState = value; }
    private DrawerState drawerState;

    public DrawerBase(int size) : base(size) { }
}

public class DrawerState : StateBase
{
    public DrawerState(Item item) : base(item) { }


    [LoadProperties(PropertyType.Runtime)]
    public bool Open { get => (bool)This.Get(nameof(Open)); set => This.Set(nameof(Open), value); }
}


[Load]
public class StateBase
{
    protected Item This;
    public StateBase(Item item) {
        this.This = item;
    }
}