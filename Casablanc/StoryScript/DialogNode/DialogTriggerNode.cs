using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogTriggerNode : DialogNodeBase
{
    [Input(typeConstraint = TypeConstraint.Strict)] public string @ǰ�öԻ�;
    [Output(typeConstraint = TypeConstraint.Strict)] public bool @��ɴ���;

    public override object GetValue(NodePort port) {
        return this.GetUpdateType() == Dialog_UpdateType.Enable;
    }


}

