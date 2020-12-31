using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateAssetMenu(fileName ="新故事图",menuName ="剧情/故事块")]
public class StoryBlock : NodeBlockBase
{
    [HideInInspector]
    public bool Intest = false;
    private bool ContinueUpdate = false;
    [HideInInspector]
    public bool LoadEntry = false;
    [HideInInspector]
    public int Hash = -1;
    public static HashGenerator NodeHash = new HashGenerator();
    public List<StoryNode> ActiveNode = new List<StoryNode>();

    public HashSet<StoryEntryNode> StoryEntryNodes=new HashSet<StoryEntryNode>();
    public HashSet<StoryExitNode> storyExitNodes=new HashSet<StoryExitNode>();






    public void LoadStoryNode(StoryNode storyNode) {
        storyNode.Hash = NodeHash.GetHash();
        this.StoryNodes[storyNode.Hash] = storyNode;
    }
    public void LoadoffStoryNode(StoryNode storyNode) {
        this.StoryNodes[storyNode.Hash] = null;
        NodeHash.DisHash(storyNode.Hash);
        storyNode.Hash = -1;
    }
    /*
    void ISerializationCallbackReceiver.OnAfterDeserialize() {
        this.StoryNodes.Clear();
        foreach (var node in ActiveNode) {
            node.Hash = NodeHash.GetHash();
            StoryNodes[node.Hash] = node;
        }
    }
    void ISerializationCallbackReceiver.OnBeforeSerialize() {
        this.ActiveNode.Clear();
        foreach (var node in StoryNodes) {
            node.Value.Hash = -1;
            this.ActiveNode.Add(node.Value);
        }
    }
    */
    public override void Update() {
        BeforeUpdate();
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
public abstract class NodeBlockBase : NodeGraph, INode,StaticINode
{
    protected Dictionary<int, StoryNode> StoryNodes = new Dictionary<int, StoryNode>();

    NodeType INode.GetNodeType() {
        return NodeType.StoryBlockNode;
    }

    public virtual void Update() {
        foreach (var node in StoryNodes) {
            ((INode)node.Value).Update();
        }
    }
    void INode.NodeInit() {
        foreach (var node in StoryNodes) {
            ((INode)node.Value).NodeInit();
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
