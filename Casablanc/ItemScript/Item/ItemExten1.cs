using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
public static class Mode
{
    public static bool EditorMode = true;
}
public partial class Item_Property
{

    public object this[string Name] {
        get {
            Init();
            if (this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.StaticPackMap.TryGetValue(Name, out var Spack)) {
                switch (Spack.___Data) {
                    case 1: return this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.IntData[Spack.PosInList];
                    case 2: return this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.BoolData[Spack.PosInList];
                    case 3: return this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.FloatData[Spack.PosInList];
                    case 4: return this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.StringData[Spack.PosInList];
                    default:Debug.LogError("错误的元素");return null;
                }
            }
            else if(this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.RuntimePackMap.TryGetValue(Name,out var Rpack)) {
                switch (Rpack.___Data) {
                    case 1: return this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.IntData[Rpack.PosInList];
                    case 2: return this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.BoolData[Rpack.PosInList];
                    case 3: return this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.FloatData[Rpack.PosInList];
                    case 4: return this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.StringData[Rpack.PosInList];
                    default:Debug.LogError("错误的元素");return null;
                }
            }
            else {
                Debug.LogError("错误的元素"); return null;
            }
        }
        set {
            Init();
            if (this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.StaticPackMap.TryGetValue(Name, out var Spack)) {
                switch (Spack.___Data) {
                    case 1:  this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.IntData[Spack.PosInList]=(int)value;return;
                    case 2:  this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.BoolData[Spack.PosInList]=(bool)value;return;
                    case 3:  this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.FloatData[Spack.PosInList]=(float)value;return;
                    case 4:  this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.StringData[Spack.PosInList]=(string)value;return;
                    default: Debug.LogError("错误的元素"); return;
                }
            }
            else if (this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.RuntimePackMap.TryGetValue(Name, out var Rpack)) {
                switch (Rpack.___Data) {
                    case 1:  this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.IntData[Rpack.PosInList]=(int)value;return;
                    case 2:  this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.BoolData[Rpack.PosInList]=(bool)value;return;
                    case 3:  this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.FloatData[Rpack.PosInList]=(float)value;return;
                    case 4:  this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.StringData[Rpack.PosInList]=(string)value;return;
                    default: Debug.LogError("错误的元素"); return;
                }
            }
            else {
                Debug.LogError("错误的元素"); return;
            }
        }
    }

    public void Load(string Name,Type type,PropertyType propertyType) {
        if (!this.Contain(Name)) {
            var pack = new Context_Pack();

            pack.PropertyName = Name;
            if (type == typeof(int)) {
                pack.___Data = 1;
                if (propertyType == PropertyType.Runtime) {
                    this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.IntData.Add(default(int));
                    pack.PosInList = this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.IntData.Count - 1;
                }
                else if (propertyType == PropertyType.Static) {
                    this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.IntData.Add(default(int));
                    pack.PosInList = this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.IntData.Count - 1;
                }
            }
            else if (type == typeof(bool)) {
                pack.___Data = 2;
                if (propertyType == PropertyType.Runtime) {
                    this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.BoolData.Add(default(bool));
                    pack.PosInList = this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.BoolData.Count - 1;
                }
                else if (propertyType == PropertyType.Static) {
                    this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.BoolData.Add(default(bool));
                    pack.PosInList = this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.BoolData.Count - 1;
                }
            }
            else if (type == typeof(float)) {
                pack.___Data = 3;
                if (propertyType == PropertyType.Runtime) {
                    this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.FloatData.Add(default(float));
                    pack.PosInList = this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.FloatData.Count - 1;
                }
                else if (propertyType == PropertyType.Static) {
                    this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.FloatData.Add(default(float));
                    pack.PosInList = this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.FloatData.Count - 1;
                }
            }
            else if (type == typeof(string)) {
                pack.___Data = 4;
                if (propertyType == PropertyType.Runtime) {
                    this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.StringData.Add(default(string));
                    pack.PosInList = this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.StringData.Count - 1;
                }
                else if (propertyType == PropertyType.Static) {
                    this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.StringData.Add(default(string));
                    pack.PosInList = this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.StringData.Count - 1;
                }
            }
            else {
                Debug.LogError("失败的序列化类型");
            }
            if (propertyType == PropertyType.Runtime) {
                this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.RuntimePacks.Add(pack);
            }
            else if (propertyType == PropertyType.Static) {
                this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.StaticPacks.Add(pack);
            }
        }      
    }

    public void Init() {
        if (this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.StaticPackMap == null) {
            this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.StaticPackMap = new Dictionary<string, Context_Pack>();
            foreach (var pack in this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.StaticPacks) {
                this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.StaticPackMap.Add(pack.PropertyName, pack);
            }
        }
        if (this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.RuntimePackMap == null) {
            this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.RuntimePackMap = new Dictionary<string, Context_Pack>();
            foreach (var pack in this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.RuntimePacks) {
                this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.RuntimePackMap.Add(pack.PropertyName, pack);
            }
        }
        while (this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.IntData.Count < this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.IntSizeR) {
            this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.IntData.Add(default(int));
        }
        while (this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.BoolData.Count < this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.BoolSizeR) {
            this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.BoolData.Add(default(bool));
        }
        while (this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.FloatData.Count < this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.FloatSizeR) {
            this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.FloatData.Add(default(float));
        }
        while (this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.StringData.Count < this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.StringSizeR) {
            this.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.StringData.Add(default(string));
        }
    }
    private bool Contain(string Name) {
        for(int i = 0; i < this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.RuntimePacks.Count; i++) {
            if (this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.RuntimePacks[i].PropertyName == Name) {
                return true;
            }
        }
        for (int i = 0; i < this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.StaticPacks.Count; i++) {
            if (this.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.StaticPacks[i].PropertyName == Name) {
                return true;
            }
        }
        return false;
    }
}


public abstract class ItemContext
{
    public List<int> IntData = new List<int>();
    public List<bool> BoolData = new List<bool>();
    public List<float> FloatData = new List<float>();
    public List<string> StringData = new List<string>();

}
[Serializable]
public class ItemContextMapping
{
    public List<Context_Pack> StaticPacks = new List<Context_Pack>();
    public List<Context_Pack> RuntimePacks = new List<Context_Pack>();
    [NonSerialized]
    public Dictionary<string, Context_Pack> StaticPackMap;
    [NonSerialized]
    public Dictionary<string, Context_Pack> RuntimePackMap;
    public int IntSizeR = 0, BoolSizeR = 0, FloatSizeR = 0, StringSizeR = 0;
    public bool Inited = false;
}



[Serializable]
public class Context_Pack
{
    public string PropertyName;
    public int ___Data, PosInList;
}



public enum PropertyType
{
    Static,
    Runtime,
}

public static class SerializeEx
{
    public static object Get(this Item item, string name) {
        return item.Info_Handler.Item_Property[name];
    }
    public static void Set(this Item item, string name, object value) {
        try {
            item.Info_Handler.Item_Property[name] = value;
        }
        catch(Exception e) {
            Debug.LogError(e.Message + item.GetType().Name + " " + name);
        }
    }

    public static void __StableContext(this Item item) {
        EditorUtility.SetDirty(StaticPath.ItemLoad[item.Info_Handler.Item_Property.ItemStaticProperties.ItemType, item.Info_Handler.Item_Property.ItemStaticProperties.ItemID]);       
    }
    public static void __LoadContext(this Item item) {
        if (!item.Info_Handler.Item_Property.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.Inited) {
            Type type = item.GetType();
            var properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++) {
                if (properties[i].PropertyType.GetCustomAttributes(typeof(LoadAttribute), true).Length > 0) {
                    var pros = properties[i].PropertyType.GetProperties();
                    for (int j = 0; j < pros.Length; j++) {
                        var att = pros[j].GetCustomAttributes(typeof(LoadPropertiesAttribute), false);
                        if (att.Length > 0) {
                            for (int z = 0; z < att.Length; z++) {
                                LoadPropertiesAttribute load = att[z] as LoadPropertiesAttribute;
                                item.Info_Handler.Item_Property.Load(pros[j].Name, pros[j].PropertyType, load.LoadType);
                            }
                        }
                    }
                }
                var atts = properties[i].GetCustomAttributes(typeof(LoadPropertiesAttribute), false);
                if (atts.Length > 0) {
                    for (int z = 0; z < atts.Length; z++) {
                        LoadPropertiesAttribute load = atts[z] as LoadPropertiesAttribute;
                        item.Info_Handler.Item_Property.Load(properties[i].Name, properties[i].PropertyType, load.LoadType);
                    }
                }
            }


            var fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++) {
                if (fields[i].FieldType.GetCustomAttributes(typeof(LoadAttribute), true).Length > 0) {
                    var pros = fields[i].FieldType.GetProperties();
                    for (int j = 0; j < pros.Length; j++) {
                        var atts = pros[j].GetCustomAttributes(typeof(LoadPropertiesAttribute), false);
                        if (atts.Length > 0) {
                            for (int z = 0; z < atts.Length; z++) {
                                LoadPropertiesAttribute load = atts[z] as LoadPropertiesAttribute;
                                item.Info_Handler.Item_Property.Load(pros[j].Name, pros[j].PropertyType, load.LoadType);
                            }
                        }
                    }
                }
            }
        }
    }


    public static void __UploadContext(this Item item) {
        if (!item.Info_Handler.Item_Property.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.Inited) {
            item.Info_Handler.Item_Property.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.Inited = true;
            item.Info_Handler.Item_Property.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.IntSizeR = item.Info_Handler.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.IntData.Count;
            item.Info_Handler.Item_Property.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.BoolSizeR = item.Info_Handler.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.BoolData.Count;
            item.Info_Handler.Item_Property.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.FloatSizeR = item.Info_Handler.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.FloatData.Count;
            item.Info_Handler.Item_Property.ItemStaticProperties.ItemStaticValues.ItemStaticContext.ItemContextMapping.StringSizeR = item.Info_Handler.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.ItemRuntimeContext.StringData.Count;           
        }
    }

}

