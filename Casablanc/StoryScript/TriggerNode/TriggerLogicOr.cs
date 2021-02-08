using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("或", menuName = "触发器逻辑节点/或",order = 1)]
public class TriggerLogicOr : NodeStatic
{
    [Input(dynamicPortList = true)] public bool @触发输入;
    [Output] public bool @触发输出;

    public override object GetValue(NodePort port) {
        bool flag = false;
        foreach (var p in this.Inputs) {
            if (p.ConnectionCount > 0) {
                flag = (bool)p.GetInputValue() || flag;
            }
        }
        return flag;
    }
}