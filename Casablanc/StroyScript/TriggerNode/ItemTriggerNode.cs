using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("物品触发器",menuName ="触发器节点/物品触发器")]
public class ItemTriggerNode : TriggerNodeStatic
{
    [Output] public bool @完成触发;

    public override object GetValue(NodePort port) {
        this.Update();
        return this.GetTrigger();
    }

}




