using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


[NodeWidth(150)]
public class OneWayBindNode : BindNodeBase
{
    
    [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)] public Link_NodePatch @ÃüÁî¿é;


    public override bool IsGroup(NodeBase Target) {
        var OutPort = this.GetPort(nameof(°ó¶¨¿é));
        if (OutPort.GetConnections().Count > 0) {
            foreach (var con in OutPort.GetConnections()) {
                if (object.ReferenceEquals(Target, con.node)) {
                    return true;
                }
            }
        }
        return false;
    }
    public override void GroupAction(Action<NodeBase> action) {
        var OutPort = this.GetPort(nameof(°ó¶¨¿é));
        if (OutPort.GetConnections().Count > 0) {
            foreach (var con in OutPort.GetConnections()) {
                if (con.node is NodeBase) {
                    if (!BindNodeStatic.NodeActioned.Contains(((NodeBase)con.node)) && !(BindNodeStatic.NodeNoNeedAction.Contains(((NodeBase)con.node)))) {
                        action.Invoke(((NodeBase)con.node));
                        BindNodeStatic.NodeActioned.Add((NodeBase)con.node);
                    }
                }
            }
        }
    }
    
}


public abstract class BindNodeBase : ComponentNodeBase
{  
    [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)] public Link_NodePatch @°ó¶¨¿é;
    private static HashSet<NodeBase> NodeSet = new HashSet<NodeBase>();
    private static HashSet<NodeBase> NodeIgnore = new HashSet<NodeBase>();
    public abstract void GroupAction(Action<NodeBase> action);
    public abstract bool IsGroup(NodeBase Target);

    public virtual IEnumerable<NodeBase> GroupSelect(Action<HashSet<NodeBase>> IgnoreSet) {
        NodeSet.Clear();
        NodeIgnore.Clear();
        IgnoreSet.Invoke(NodeIgnore);
        var OutPort = this.GetPort(nameof(°ó¶¨¿é));
        if (OutPort.GetConnections().Count > 0) {
            foreach (var con in OutPort.GetConnections()) {
                if (con.node is NodeBase) {
                    if (!NodeSet.Contains((NodeBase)con.node) && !NodeIgnore.Contains((NodeBase)con.node)) {
                        NodeSet.Add(((NodeBase)con.node));
                        yield return (NodeBase)con.node;
                    }
                }
                else {
                    Debug.LogError("Á¬½Ó´íÎó");
                }
            }
        }
    }
}