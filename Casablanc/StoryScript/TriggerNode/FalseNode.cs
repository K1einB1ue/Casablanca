using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("假", menuName = "触发器逻辑节点/假",order = 4)]
public class FalseNode : NodeBase
{
    [Output] public bool @假 = false;

    public override object GetValue(NodePort port) {
        return false;
    }

}