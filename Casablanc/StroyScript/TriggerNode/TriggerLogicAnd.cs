using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("与", menuName = "触发器逻辑节点/与",order = 2)]
public class TriggerLogicAnd : NodeStatic
{
    [Input(dynamicPortList = true)] public bool @触发输入;
    [Output] public bool @触发输出;

    public override object GetValue(NodePort port) {
        bool flag = true;
        foreach (var p in this.Ports) {
            flag = (bool)p.GetInputValue() && flag;
        }
        return flag;
    }
}
