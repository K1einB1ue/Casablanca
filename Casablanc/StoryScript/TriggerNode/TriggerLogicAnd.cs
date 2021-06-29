using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("与", menuName = "触发器逻辑节点/与",order = 2)]
public class TriggerLogicAnd : NodeBase
{
    [Input(dynamicPortList = true)] public bool @触发输入;
    [Output] public bool @触发输出;

    public override object GetValue(NodePort port) {
        bool flag = true;
        foreach (var PIN in this.DynamicInputs) {
            if (PIN.fieldName.Contains("触发输入")) {
                flag = (bool)PIN.GetInputValue() && flag;
            }
        }
        return flag;
    }
}
