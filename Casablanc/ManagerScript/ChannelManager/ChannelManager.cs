using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;




public interface ChannelListenerUnit
{
    int Hash { get; set; }
    bool IsEnable { get; }
    void ChannelMassage(object Channel, Func<string, Func<object, object>> FieldGets);
    void Exit();
}


public abstract class ChannelManagerBase<ChannelManager,Channel> : SingletonMono<ChannelManager> where Channel: InfoChannel  where ChannelManager : ChannelManagerBase<ChannelManager, Channel> 
{
    private  Dictionary<string, Func<object, object>> Field_Gets = new Dictionary<string, Func<object, object>>();
    public  Channel InfoChannel { get => _infochannel; }
    private  Channel _infochannel = null;
    private  LinkedList<ChannelListenerUnit> UnitInUse = new LinkedList<ChannelListenerUnit>();
    private  Dictionary<int, LinkedListNode<ChannelListenerUnit>> UnitDic = new Dictionary<int, LinkedListNode<ChannelListenerUnit>>();
    private  HashGenerator HashGenerator = new HashGenerator();

    protected ChannelManagerBase(Channel channel) {
        _infochannel ??= channel;
    }

    protected virtual void Awake() {
        List<FieldInfo> Fields = new List<FieldInfo>(typeof(Channel).GetFields());
        for (int i = 0; i < Fields.Count; i++) {
            if (Fields[i].IsPublic && Fields[i].GetCustomAttributes(typeof(ChannelMessageAttribute), false).Length > 0 && (Fields[i].FieldType == typeof(int) || Fields[i].FieldType == typeof(float))) {
                var Field = Fields[i];
                Field_Gets.Add(Field.Name, (Target) => {
                    return Field.GetValue(Target);
                });
            }
        }
    }

    public void Enter(ChannelListenerUnit unit) {
        if (unit.Hash == -1) {
            LinkedListNode<ChannelListenerUnit> Node = new LinkedListNode<ChannelListenerUnit>(unit);
            int hash = HashGenerator.GetHash();
            UnitDic.Add(hash, Node);
            unit.Hash = hash;
            UnitInUse.AddLast(Node);
            unit.ChannelMassage(InfoChannel,(param)=> {
                if (Field_Gets.TryGetValue(param, out var func)) {
                    return func;
                }
                else {
                    Debug.LogError("Œ¥’“µΩ”≥…‰∫Ø ˝");
                    return null;
                }
            });
        }
        else {
            if (!UnitDic.TryGetValue(unit.Hash, out var linkedListNode)) {
                LinkedListNode<ChannelListenerUnit> Node = new LinkedListNode<ChannelListenerUnit>(unit);
                int hash = HashGenerator.GetHash();
                UnitDic.Add(hash, Node);
                unit.Hash = hash;
                UnitInUse.AddLast(Node);
                Debug.LogWarning("≥¢ ‘–ﬁ∏¥±Ì¥ÌŒÛ");
            }
        }
    }
    public void Exit(ChannelListenerUnit unit) {
        HashGenerator.DisHash(unit.Hash);
        if (UnitDic.TryGetValue(unit.Hash, out var linkedListNode)) {
            UnitInUse.Remove(linkedListNode);
            UnitDic.Remove(unit.Hash);
            unit.Hash = -1;
        }
        else {
            Debug.LogError("±Ì¥ÌŒÛ");
        }
    }
    protected virtual void FixedUpdate() {
        if (InfoChannel.ContinuousChange) {
            InfoChannel.Change = false;
            this.ChannelUpdate();
            this.OnChange();
        }
        else {
            if (InfoChannel.Change) {
                InfoChannel.Change = false;
                this.ChannelUpdate();
                this.OnChange();
            }
        }

    }

    private void ChannelUpdate() {
        List<ChannelListenerUnit> LoadOff = null;
        foreach (var unit in UnitInUse) {
            if (unit.IsEnable) {
                unit.ChannelMassage(InfoChannel, (param) => {
                    if (Field_Gets.TryGetValue(param, out var func)) {
                        return func;
                    }
                    else {
                        Debug.LogError("Œ¥’“µΩ”≥…‰∫Ø ˝");
                        return null;
                    }
                });
            }
            else {
                LoadOff ??= new List<ChannelListenerUnit>();
                LoadOff.Add(unit);
            }
        }
        if (LoadOff != null) {
            int i = LoadOff.Count - 1;
            while (LoadOff.Count != 0) {
                LoadOff[i].Exit();
                LoadOff.RemoveAt(i--);
            }
        }
    }
    protected virtual void OnChange() {

    }
}

public abstract class ChannelListenerUnitBase<ChannelManager,Channel> : MonoBehaviour, ChannelListenerUnit where ChannelManager: ChannelManagerBase<ChannelManager,Channel> where Channel:InfoChannel
{
    public List<AdjustmentPack> AdjustMents = new List<AdjustmentPack>();
    private static Action<ChannelListenerUnit> exit, enter;
    int ChannelListenerUnit.Hash { get => hash; set => hash = value; }
    private int hash = -1;
    public abstract bool IsEnable { get; }
    public abstract void ChannelMassage(object Channel, Func<string, Func<object, object>> FieldGets);

    void ChannelListenerUnit.Exit() {
        if (exit == null) {
            var instance = typeof(ChannelManager).BaseType.BaseType.GetField("Instance", BindingFlags.Static | BindingFlags.Public).GetValue(null);
            var ChannelManager = instance as ChannelManagerBase<ChannelManager, Channel>;
            exit = (obj) => { ChannelManager.Exit(obj); };  
        }
        exit.Invoke(this);
    }
    protected void Enter() {
        if (enter == null) {
            var instance = typeof(ChannelManager).BaseType.BaseType.GetField("Instance", BindingFlags.Static | BindingFlags.Public).GetValue(null);
            var ChannelManager = instance as ChannelManagerBase<ChannelManager, Channel>;
            enter = (obj) => { ChannelManager.Enter(obj); };
        }
        enter.Invoke(this);
    }
}
















[AttributeUsage(AttributeTargets.Field)]
public class ChannelMessageAttribute : Attribute { }




public enum ParameterType
{
    INT,
    FLOAT,
}
[Serializable]
public class AdjustmentPack
{
    public ParameterType ParameterType;
    public string WeatherParameter;
    public string ObjectParameter;
    public AnimationCurve Curve;
    public float Min, Max;
    public int GetIntValue(object Channel, Func<string, Func<object, object>> Gets) {
        return Convert.ToInt32(Curve.RemapTo(Convert.ToSingle(Gets(WeatherParameter)(Channel)), Min, Max));
    }
    public float GetFloatValue(object Channel, Func<string, Func<object, object>> Gets) {
        return Curve.RemapTo(Convert.ToSingle(Gets(WeatherParameter)(Channel)), Min, Max);
    }
}




[CustomPropertyDrawer(typeof(AdjustmentPack))]
public class AdjustmentPackEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        Type ChannelInfoType = property.serializedObject.targetObject.GetType().BaseType.GetTypeInfo().GetGenericArguments()[1];
        int value = 0;
        string Select = property.FindPropertyRelative("WeatherParameter").stringValue;
        List<string> Gets = new List<string>();
        var Fields = ChannelInfoType.GetFields();
        for (int i = 0; i < Fields.Length; i++) {
            if (Fields[i].IsPublic && Fields[i].GetCustomAttributes(typeof(ChannelMessageAttribute), false).Length > 0 && (Fields[i].FieldType == typeof(int) || Fields[i].FieldType == typeof(float))) {
                Gets.Add(Fields[i].Name);
                if (Gets[Gets.Count - 1] == Select) {
                    value = Gets.Count - 1;
                }
            }
        }

        using (new EditorGUI.PropertyScope(position, label, property)) {
            Rect Next = position;


            Next = property.NewPropertyGroup(Next, new string[] { "ParameterType", "ObjectParameter", "Curve", "Min", "Max" });

            Next = EditorEx.GetNormalRect(Next);
            value = EditorGUI.Popup(Next, value, Gets.ToArray());


            property.FindPropertyRelative("WeatherParameter").stringValue = Gets[value];
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorEx.NormalHeightByNum(6);
    }
}
