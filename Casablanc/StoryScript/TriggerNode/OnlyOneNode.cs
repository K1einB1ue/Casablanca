using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using XNode;
using XNodeEditor;

[CreateNodeMenu("唯一节点", menuName = "触发器逻辑节点/唯一节点", order = 7)]
public class OnlyOneNode : NodeStatic
{
    [Input(connectionType = ConnectionType.Override, backingValue = ShowBackingValue.Never,dynamicPortList = true)] public bool @触发输入;
    [Output(connectionType = ConnectionType.Multiple, dynamicPortList = true)]                                      public bool @触发输出;
    public int EnableNum = 1;
    public List<int> EnableList = new List<int>();
    public static StringBuilder stringBuilder = new StringBuilder();

    public override void ReStruct() {
        this.EnableList = new List<int>();
    }
    public override object GetValue(NodePort port) {
        foreach (var nodePort in this.Inputs) {
            if (nodePort.ConnectionCount > 0) {
                if (nodePort.GetInputValue<bool>()) {
                    if (EnableList.Count < EnableNum) {
                        EnableList.Add(int.Parse(nodePort.fieldName.Replace("触发输入 ", "")));
                    }
                }
            }
        }
        foreach (var v in EnableList) {
            stringBuilder.Clear();
            stringBuilder.Append("触发输出 ");
            stringBuilder.Append(v.ToString());
            if (port.fieldName == stringBuilder.ToString()) {
                return true;
            }
        }        
        return false;
    }
}

[CustomNodeEditor(typeof(OnlyOneNode))]
public class OnlyOneNodeEditor : NodeEditor {

}