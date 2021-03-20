using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[Serializable]
public class Pair<T,B>
{
    public T First;
    public B Second;
}
