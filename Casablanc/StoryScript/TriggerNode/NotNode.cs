using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("��", menuName = "�������߼��ڵ�/��", order = 6)]
public class NotNode : NodeStatic
{
    [Input(connectionType = ConnectionType.Override,backingValue = ShowBackingValue.Never)] public bool @��������;
    [Output] public bool @�������;

    public override object GetValue(NodePort port) {
        if (port.fieldName == "�������") {
            if (this.GetInputPort("��������").ConnectionCount > 0) {
                return !(bool)this.GetInputPort("��������").GetInputValue();
            }
            return false;
        }
        else {
            return null;
        }
    }
}
