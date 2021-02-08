using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("非", menuName = "触发器逻辑节点/非", order = 6)]
public class NotNode : NodeStatic
{
    [Input(connectionType = ConnectionType.Override,backingValue = ShowBackingValue.Never)] public bool @触发输入;
    [Output] public bool @触发输出;

    public override object GetValue(NodePort port) {
        if (port.fieldName == "触发输出") {
            if (this.GetInputPort("触发输入").ConnectionCount > 0) {
                return !(bool)this.GetInputPort("触发输入").GetInputValue();
            }
            return false;
        }
        else {
            return null;
        }
    }
}
