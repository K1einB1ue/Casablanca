using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogTriggerNode : DialogNodeStatic
{
    [Input(typeConstraint = TypeConstraint.Strict)] public Link_Dialog @ǰ�öԻ�;
    [Output(typeConstraint = TypeConstraint.Strict)] public bool @��ɴ���;

    public override object GetValue(NodePort port) {
        return this.GetUpdateType() == Dialog_UpdateType.Enable;
    }


}

