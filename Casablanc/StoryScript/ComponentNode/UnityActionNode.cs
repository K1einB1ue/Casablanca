using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XNode;

public class ActionNode : ActionNodeBase
{
    

    public override void Invoke() {

    }

}


public abstract class ActionNodeBase : ComponentNodeBase
{

    [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)] public Link_NodePatch @ÃüÁî¿é;
    public abstract void Invoke();
}





