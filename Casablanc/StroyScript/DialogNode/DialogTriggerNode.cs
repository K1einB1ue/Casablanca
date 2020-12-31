using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogTriggerNode : DialogNodeStatic
{
    [Input(typeConstraint = TypeConstraint.Strict)] public Link_Dialog @前置对话;
    [Output(typeConstraint = TypeConstraint.Strict)] public bool @完成触发;

    public override object GetValue(NodePort port) {
        return this.GetUpdateType() == Dialog_UpdateType.Enable;
    }


}

