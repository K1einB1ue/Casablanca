using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class NodePipelineInstance : PipelineInstance<NodeBase>
{
    public NodePipelineInstance(params NodePipeline[] pipelines) : base(pipelines) { }   
}
public class NodePipe_Filter: Pipe_Filter<NodeBase>, NodePipeline
{
    public NodePipe_Filter(Func<NodeBase, bool> Filter):base(Filter) { }
}
public class NodePipe_Action : Pipe_Action<NodeBase>, NodePipeline
{
    public NodePipe_Action(Action<NodeBase> action):base(action) { }
}
public class NodePipe_InfoFunc<Info> : Pipe_InfoFunc<NodeBase,Info>, NodePipeline
{
    public NodePipe_InfoFunc(Func<NodeBase,Info> InfoFunc) : base(InfoFunc) { }
}
public class NodePipe_SourceSwitch : Pipe_SourceSwitch<NodeBase> , NodePipeline
{
    public NodePipe_SourceSwitch(Func<NodeBase,IEnumerable<NodeBase>> Gain) : base(Gain) { }
}
public class NodePipe_SourceInput : Pipe_SourceInput<NodeBase>, NodePipeline
{
    public NodePipe_SourceInput(Func<NodeBase, IEnumerable<NodeBase>> Gain) : base(Gain) { }
    public NodePipe_SourceInput() : base() { }
    public NodePipe_SourceInput(NodeBase Addtion) : base(Addtion) { }
}

public interface NodePipeline: MassivePipeline<NodeBase> { }
public class PipelineInstance<Production>
{
    private Func<Production, IEnumerable<Production>> Source;
    private List<MassivePipeline<Production>> Pipelines = new List<MassivePipeline<Production>>();
    public Func<Production, Func<Production, IEnumerable<Production>>, IEnumerable<Production>> Convert;

    public PipelineInstance(params MassivePipeline<Production>[] pipelines) {
        for(int i = 0; i < pipelines.Length; i++) {
            Pipelines.Add(pipelines[i]);
        }
    }
    public void ConvertTo(PipelineInstance<Production> NextFactory) {
        Convert = NextFactory.Process;
    }
    public PipelineInstance<Production> Build(MassivePipeline<Production> pipeline, int Pos = -1) {
        if (Pos == -1) {
            Pipelines.Add(pipeline);
        }
        else {
            Pipelines.Insert(Pos, pipeline);
        }
        return this;
    }
    public void Destory(MassivePipeline<Production> pipeline) {
        Pipelines.Remove(pipeline);
    }

    public void BindSource(Func<Production, IEnumerable<Production>> Source) {
        this.Source = Source;
    }

    public IEnumerable<Production> Process(Production Subject) {
        this.Object = Subject;
        if (Source != null) {
            IEnumerable<Production> productions = this.Source.Invoke(this.Object);
            List<Production> Ienum = new List<Production>(productions);
            for (int i = 0; i < Pipelines.Count; i++) {
                productions = Pipelines[i].Process(this.Object, productions);
                ///注意! 不要删除此行! 该代码是为了修复C#编译器无法正确地调用yield return!警告!
                ///再次检查到此处的BUG的可能性微乎其微!
                Ienum = new List<Production>(productions);
            }

            ///为了后续工厂使用前一个工厂的材料 而不依赖于Func Source
            if (Convert != null) {
                Convert.Invoke(Subject, (subject) => { return productions; });
            }
            return productions;
        }
        else {
            Debug.LogError("未绑定Source!");
            return null;
        }
    }

    public IEnumerable<Production> Process(Production Subject,Func<Production,IEnumerable<Production>> Source) {
        this.Object = Subject;
        IEnumerable<Production> productions = Source.Invoke(this.Object);
        List<Production> Ienum = new List<Production>(productions);
        for (int i = 0; i < Pipelines.Count; i++) {
            productions = Pipelines[i].Process(this.Object, productions);
            ///注意! 不要删除此行! 该代码是为了修复C#编译器无法正确地调用yield return!警告!
            ///再次检查到此处的BUG的可能性微乎其微!
            Ienum = new List<Production>(productions);
        }

        ///为了后续工厂使用前一个工厂的材料 而不依赖于Func Source
        if (Convert != null) {
            Convert.Invoke(Subject, (subject) => { return productions; });
        }
        return productions;
    }

    public Production Object {
        get {
            return obj;
        }
        set {
            obj = value;
        }
    }

    private Production obj;
}

public interface MassivePipeline<Production>
{
    public IEnumerable<Production> Process(Production Subject,IEnumerable<Production> inputs);
    
}
public abstract class MassivePipelineSectionBase<Production> : MassivePipeline<Production>
{
    public abstract IEnumerable<Production> Process(Production Subject, IEnumerable<Production> inputs);
}

public class Pipe_Filter<Production> : MassivePipelineSectionBase<Production>
{
    public Pipe_Filter(Func<Production, bool> Filter) {
        this.Filter = Filter;
    }
    private Func<Production, bool> Filter;
    public override IEnumerable<Production> Process(Production Subject, IEnumerable<Production> inputs) {
        foreach(var input in inputs) {
            if (Filter.Invoke(input)) {
                yield return input;
            }
        }
    }
}
public class Pipe_Action<Production> : MassivePipelineSectionBase<Production>
{
    public Pipe_Action(Action<Production> Action) {
        this.Action = Action;
    }
    private Action<Production> Action;
    public override IEnumerable<Production> Process(Production Subject, IEnumerable<Production> inputs) {
        foreach (var input in inputs) {
            Action.Invoke(input);
            yield return input;
        }
    }
}
public class Pipe_InfoFunc<Production,Info> : MassivePipelineSectionBase<Production>
{
    public Pipe_InfoFunc(Func<Production, Info> InfoFunc) {
        this.InfoFunc = InfoFunc;
    }
    private Func<Production, Info> InfoFunc;
    private UnityEvent<Info> Event = new UnityEvent<Info>();
    public void Regist(UnityAction<Info> action) {
        this.Event.AddListener(action);
    }
    public override IEnumerable<Production> Process(Production Subject, IEnumerable<Production> inputs) {
        foreach (var input in inputs) {
            Event.Invoke(InfoFunc.Invoke(input));
            yield return input;           
        }
    }
}
public class Pipe_SourceSwitch<Production> : MassivePipelineSectionBase<Production>
{
    public static HashSet<Production> Once = new HashSet<Production>();
    public Pipe_SourceSwitch(Func<Production, IEnumerable<Production>> Gain) {
        this.Gain = Gain;
    }
    private Func<Production, IEnumerable<Production>> Gain;
    public override IEnumerable<Production> Process(Production Subject, IEnumerable<Production> inputs) {
        Once.Clear();
        foreach (var input in inputs) {
            if (Gain.Invoke(input) != null) {
                foreach (var gain in Gain.Invoke(input)) {
                    if (Once.Add(gain)) {                       
                        yield return gain;
                    }
                }
            }          
        }
    }
}
public class Pipe_SourceInput<Production> : MassivePipelineSectionBase<Production>
{
    public static HashSet<Production> Once = new HashSet<Production>();
    public Pipe_SourceInput(Func<Production, IEnumerable<Production>> Gain) {
        this.Gain = Gain;
    }
    public Pipe_SourceInput() { }
    public Pipe_SourceInput(Production Addition) { }


    private Production Addition;
    private Func<Production,IEnumerable<Production>> Gain;
    public override IEnumerable<Production> Process(Production Subject, IEnumerable<Production> inputs) {
        if (Gain != null) {
            Once.Clear();
            foreach (var input in inputs) {
                if (Once.Add(input)) {
                    yield return input;
                }
            }
            foreach (var input2 in Gain.Invoke(Subject)) {
                if (Once.Add(input2)) {
                    yield return input2;
                }
            }
        }
        else if (Addition != null) {
            foreach (var input in inputs) {
                if (Once.Add(input)) {
                    yield return input;
                }
            }
            yield return Addition;
        }
        else {
            foreach (var input in inputs) {
                if (Once.Add(input)) {
                    yield return input;
                }
            }
            yield return Subject;
        }
    }
}
