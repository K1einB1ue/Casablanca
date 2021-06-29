using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


[CreateAssetMenu(fileName ="新故事图",menuName ="剧情/故事块")]
public class StoryBlock : NodeBlockBase
{
    /// <summary>
    /// 编辑器使用
    /// </summary>
    [HideInInspector]
    public bool Intest = false;






    private bool ContinueUpdate = false;
    [NonSerialized]
    public bool LoadEntry = false;

    public HashSet<StoryEntryNode> StoryEntryNodes=new HashSet<StoryEntryNode>();
    public HashSet<StoryExitNode> storyExitNodes=new HashSet<StoryExitNode>();











    public override void Update() {
        this.UpdateStoryNodes();
        base.Update();
    }
    public override void StaticUpdate() {
        BeforeUpdate();
        base.StaticUpdate();
    }

    public void UpdateStoryNodes() {
        BeforeUpdate();
        do {
            ContinueUpdate = false;
            foreach (var node in this.nodes) {
                if (((INode)node).GetNodeType() == NodeType.StoryNode) {
                    if (((IStory)node).GetUpdateType() == Story_UpdateType.PreEnable) {
                        ((IStory)node).Update(out bool tmp);
                        ContinueUpdate = ContinueUpdate || tmp;
                    }
                }
            }
        } 
        while (ContinueUpdate);
    }
    public override void ReStruct() {
        foreach (var node in this.nodes) {
            ((INode)node).ReStruct();
        }
    }
    private void BeforeUpdate() {
        if (!LoadEntry) {
            foreach (var nodeEntry in StoryEntryNodes) {
                ((INode)nodeEntry).Update();
            }
            foreach (var nodeExit in storyExitNodes) {
                ((INode)nodeExit).Update();
            }
            LoadEntry = true;
        }
    }
}
public interface StaticINode {
    void StaticUpdate();
}
public abstract class NodeBlockBase : NodeGraph,StaticINode
{ 
    protected Dictionary<int, StoryNode> StoryNodes = new Dictionary<int, StoryNode>();


    public virtual void Update() {
        foreach (var node in StoryNodes) {
            ((INode)node.Value).Update();
        }
    }

    public virtual void ReStruct() { }

    public virtual void StaticUpdate() {
        foreach (var node in nodes) {
            if (((INode)node).GetNodeType()==NodeType.StoryNode) {
                if (((IStory)node).GetUpdateType() == Story_UpdateType.PreEnable) {
                    ((INode)node).Update();
                }
            }
        }
    }
}
