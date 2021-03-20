using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogMachineNode : NodeStatic
{
    [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)] public Link_DialogMachine @¶Ô»°×´Ì¬»ú;
    public DialogMachine DialogMachine;

    public override object GetValue(NodePort port) {
        return this.DialogMachine;
    }
    public override void ReStruct() {
        base.ReStruct();
        if (this.DialogMachine != null) {
            this.DialogMachine.Dialog = null;
            this.DialogMachine.currentNode = null;
            this.DialogMachine.currentGroup = null;
        }
    }
}
