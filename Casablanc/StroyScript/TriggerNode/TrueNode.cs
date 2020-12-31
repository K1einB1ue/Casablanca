using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("真", menuName = "触发器逻辑节点/真",order = 3)]
public class TrueNode : NodeStatic
{
    [Output] public bool @真 = true;
    public override object GetValue(NodePort port) {
        return true;
    }

}