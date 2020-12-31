using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Key
{
    int Mapping{ get; set; }
}


public abstract class KeysStatic : ItemStatic, Key
{
    KeyState KeyState = new KeyState();
    public int Mapping { get => this.KeyState.Mapping; set { this.KeyState.Mapping = value; } }
}
public class KeyState
{
    public int Mapping;
}