using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="剧情节点存储",menuName ="剧情/剧情节点存储")]
public class StoryNodeCurrent : NodeStoreBase, ISerializationCallbackReceiver
{
    public static HashGenerator StoryHash = new HashGenerator();
    public List<StoryBlock> ActiveBlock = new List<StoryBlock>();

    public void LoadStoryBlock(StoryBlock storyBlock) {
        storyBlock.Hash = StoryHash.GetHash();
        this.StoryBlocks[storyBlock.Hash] = storyBlock;
    }
    public void LoadoffStoryBlock(StoryBlock storyBlock) {
        this.StoryBlocks[storyBlock.Hash] = null;
        StoryHash.DisHash(storyBlock.Hash);
        storyBlock.Hash = -1;
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize() {
        this.StoryBlocks.Clear();
        foreach (var block in ActiveBlock) {
            block.Hash = StoryHash.GetHash();
            StoryBlocks[block.Hash] = block;
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize() {
        this.ActiveBlock.Clear();
        foreach (var block in StoryBlocks) {
            block.Value.Hash = -1;
            this.ActiveBlock.Add(block.Value);
        }
    }
}



public class StoryNodeStatic : ScriptableObject, StoryStoreSpace.StoryNode
{
    public StoryStaticInfoPackage StoryStaticInfoPackage;
    public StoryPreInstanceInfoPackage StoryPreInstanceInfoPackage = new StoryPreInstanceInfoPackage();
}
[Serializable]
public class StoryNodeDynamic : StoryStoreSpace.StoryNode 
{
    public StoryRuntimeInfoPackage StoryRuntimeInfoPackage;
    public StoryNodeDynamic(Story story) {
        this.StoryRuntimeInfoPackage = new StoryRuntimeInfoPackage(story);
    }
}
[Serializable]
public enum StoryStaticDescribeWays
{
    StoryStore,
    StoryID,
}
[Serializable]
public class StoryStaticInfoPackage
{
    public StoryStaticDescribeWays storyStaticDescribeWays = StoryStaticDescribeWays.StoryStore;
    public StoryStore StoryStore;
}

public class StoryRuntimeInfoPackage
{
    public StoryRuntimeProperties.StoryRuntimeProperties StoryRuntimeProperties;
    public StoryRuntimeInfoPackage(Story story) {
        
    }
}
public class StoryPreInstanceInfoPackage
{

}




namespace StoryStoreSpace
{
    public interface StoryNode
    {
        //Story GetStory();
    }
}


public interface Story
{

}
public abstract class StoryBase : Story 
{

}

namespace StoryStaticProperties
{
    [Serializable]
    public class StoryStaticProperties
    {

    }
}
namespace StoryRuntimeProperties
{
    [Serializable]
    public class StoryRuntimeProperties
    {

    }
}
namespace StoryPreInstanceProperties
{
    [Serializable]
    public class StoryPreInstanceProperties
    {

    }
}


public abstract class NodeStoreBase : ScriptableObject, INode {
    protected Dictionary<int, StoryBlock> StoryBlocks = new Dictionary<int, StoryBlock>();
    void INode.Update() {
        foreach (var block in StoryBlocks) {
            ((INode)block.Value).Update();
        }
    }
    void INode.NodeInit() {
        foreach (var block in StoryBlocks) {
            ((INode)block.Value).NodeInit();
        }
    }
    void INode.ReStruct() { }
    NodeType INode.GetNodeType() {
        return NodeType.Store;
    }
}
