using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("时间触发器", menuName = "触发器节点/时间触发器")]
public class TimeTriggerNode : NodeBase
{
    [Output] public bool @符合触发;
    public static int TimeH, TimeM;
    public override object GetValue(NodePort port) {
        if ((TimeH < 小时与分上限.x && TimeH > 小时与分下限.x) || (TimeH == 小时与分下限.x && TimeM >= 小时与分下限.y) || (TimeH == 小时与分上限.x && TimeM <= 小时与分上限.y)) {
            return true;
        }
        return false;
    }

    public Vector2 @小时与分上限;
    public Vector2 @小时与分下限;
}