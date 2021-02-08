using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using XNode;
using XNodeEditor;

[CreateNodeMenu("Ψһ�ڵ�", menuName = "�������߼��ڵ�/Ψһ�ڵ�", order = 7)]
public class OnlyOneNode : NodeStatic
{
    [Input(connectionType = ConnectionType.Override, backingValue = ShowBackingValue.Never,dynamicPortList = true)] public bool @��������;
    [Output(connectionType = ConnectionType.Multiple, dynamicPortList = true)]                                      public bool @�������;
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
                        EnableList.Add(int.Parse(nodePort.fieldName.Replace("�������� ", "")));
                    }
                }
            }
        }
        foreach (var v in EnableList) {
            stringBuilder.Clear();
            stringBuilder.Append("������� ");
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