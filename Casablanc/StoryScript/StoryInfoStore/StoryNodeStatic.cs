using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



/*
public class StoryNodeStatic : ScriptableObject, StoryStoreSpace.StoryNode
{
    public StoryStaticInfoPackage StoryStaticInfoPackage;
    public Story GetStory() {

    }
}
[Serializable]
public class StoryNodeDynamic : StoryStoreSpace.StoryNode 
{
    public StoryRuntimeInfoPackage StoryRuntimeInfoPackage;
    public StoryNodeDynamic(Story story) {
        this.StoryRuntimeInfoPackage = new StoryRuntimeInfoPackage(story);
    }
    public Story GetStory() {

    }
}
[Serializable]
public class StoryStaticInfoPackage
{
    public List<StoryBlock> StoryBlocks;
    public List<StoryNode> storyNodes;
    public List<DialogNode> DialogNodes;
}

public class StoryRuntimeInfoPackage
{
    public StoryRuntimeProperties.StoryRuntimeProperties StoryRuntimeProperties;
    public StoryRuntimeInfoPackage(Story story) {
        
    }
}

public class DialogMap
{
    private Dictionary<DialogMachine, Dialog> Dialogs = new Dictionary<DialogMachine, Dialog>();
    public Dialog this[DialogMachine dialogMachine] {
        get {
            if (Dialogs.TryGetValue(dialogMachine, out var dialog)) {
                return dialog;
            }
            Debug.Log("错误");
            return null;
        }
    }
}


namespace StoryStoreSpace
{
    public interface StoryNode
    {
        Story GetStory();
    }
}


public class Story
{
    public Story() {

    }
    private DialogMap DialogMap = new DialogMap();


}



namespace StoryStaticProperties
{
    [Serializable]
    public class StoryStaticProperties
    {
        public StoryStaticValues StoryStaticValues = new StoryStaticValues();
    }

    [Serializable]
    public class StoryStaticValues
    {
        public StaticValues.StaticValues_Time StaticValues_Time = new StaticValues.StaticValues_Time();
    }
    namespace StaticValues {

        [Serializable]
        public class StaticValues_Time
        {
            public List<TimePoint> timePoints = new List<TimePoint>();
        }
    }

}
namespace StoryRuntimeProperties
{
    [Serializable]
    public class StoryRuntimeProperties
    {
        [HideInInspector]
        public bool init = false;

        public StoryRuntimeValues StoryRuntimeValues = new StoryRuntimeValues();
        public StoryRuntimeTemps StoryRuntimeTemps = new StoryRuntimeTemps();

        public StoryRuntimeProperties(StoryStaticProperties.StoryStaticProperties storyStaticProperties) {
            if (!this.init) {




            }
            this.init = true;
        }
    }


    [Serializable]
    public class StoryRuntimeValues
    {

    }


    namespace RuntimeValues
    {

        [Serializable]
        public class RuntimeValue_Story
        {
            public List<StoryNode> StoryNodePreEnable = new List<StoryNode>();
        }



    }

    public class StoryRuntimeTemps
    {
        public RuntimeTemps.RuntimeTemps_Event RuntimeTemps_Event = new RuntimeTemps.RuntimeTemps_Event();


    }

    namespace RuntimeTemps
    {
        public class RuntimeTemps_Event
        {
            public StoryEvent StoryEvent = new StoryEvent();
        }

        public class RuntimeTemps_Story
        {
            public BigRootHeap<TimePoint> TimeQuene = new BigRootHeap<TimePoint>();
            public Dictionary<TimePoint, StoryNode> Mapping = new Dictionary<TimePoint, StoryNode>();
            
        }



    }


}






public class StoryEvent : UnityEvent { }

*/