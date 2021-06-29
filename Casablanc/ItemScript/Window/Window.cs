using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Window
{
    public WindowState WindowState { get; set; }
}
public class WindowBase : ContainerBase
{
    public WindowState WindowState { get { windowState ??= new WindowState(this); return windowState; } set => windowState = value; }
    private WindowState windowState;
    public WindowBase(int size) : base(size) { }




}


public class WindowState : StateBase
{
    public WindowState(Item item) : base(item) { }

    [LoadProperties(PropertyType.Runtime)]
    public bool Fiexible { get => (bool)This.Get(nameof(Fiexible)); set => This.Set(nameof(Fiexible), value); }
    [LoadProperties(PropertyType.Runtime)]
    public bool Open { get => (bool)This.Get(nameof(Open)); set => This.Set(nameof(Open), value); }
}
