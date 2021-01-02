using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

[AttributeUsage( AttributeTargets.Class)]
public class CharacterAttribute : Attribute
{
    public int CharacterID;
    public bool enable = true;

    public CharacterAttribute(int characterID) {
        this.CharacterID = characterID;
    }
    public CharacterAttribute(int characterID,bool enable) {
        this.CharacterID = characterID;
        this.enable = enable;
    }
}
public static class Characters
{
    public static void AddGenerator(int CharacterID,Type type) {
        if (Generators.TryGetValue(CharacterID, out var func)) {
            Debug.LogError("角色ID多次映射!   ID:" + CharacterID.ToString());
        }
        else {
            Generators.Add(CharacterID, () => {
                Character tmp = (Character)type.Assembly.CreateInstance(type.FullName);
                return tmp;
            });
        }
    }
    public static Dictionary<int, Func<Character>> Generators = new Dictionary<int, Func<Character>>();

    
    public static Character GetCharacterByCharacterID(int CharacterID) {
        if (CharacterID == -1) {
            Debug.LogError("未定义ID的角色");
        }
        else {
            if(Generators.TryGetValue(CharacterID,out var Func)) {
                return Func();
            }
        }
        return null;
    }
    public static Character GetCharacterByCharacterID_With_StaticProperties_And_PreInstanceProperties(int CharacterID,CharacterPreInstanceProperties.CharacterPreInstanceProperties characterPreInstanceProperties) {
        if (CharacterID == -1) {
            Debug.LogError("未定义ID的角色");
        }
        else {
            if (Generators.TryGetValue(CharacterID, out var Func)) {
                Character tmp = Func();
                tmp.Info_Handler.Binding(new Character_Property(CharacterID, characterPreInstanceProperties));
                return tmp;
            }
        }
        return null;
    }

}

public interface Character_Detail
{
    Character_INFO_Handler Info_Handler { get; set; }
    Character_Property GetCharacterProperty();

}
public interface Character: Character_Detail
{
    int ID { get; }
    void OnEnable();
    void Update();
    void OnDisable();
    void Destory();
}

public abstract class CharacterBase : Character
{
    public int ID => this.Info_Handler.Character_Property.CharacterStaticProperties.CharacterID;
    
    public Character_INFO_Handler Info_Handler {
        get {
            if (this.INFO_Handler == null) {
                this.INFO_Handler = new Character_INFO_Handle_Layer_Normal();
            }
            return this.INFO_Handler;
        }
        set {
            this.INFO_Handler = value;
        }  
    }
    public CharacterInfoStore CharacterInfoStore;

    Character_INFO_Handler INFO_Handler;


    void Character.OnEnable() {

        this.onEnable();
    }  
    void Character.OnDisable() {
        this.CharacterInfoStore.SaveItem(this);
        this.onDisbale();
    }
    void Character.Update() {
        this.update();
    }
    void Character.Destory() {

    }
    public virtual void update() {
    
    }

    public virtual void onDisbale() {
    
    }
    public virtual void onEnable() {
        
    }
    public Character_Property GetCharacterProperty() {
        return this.INFO_Handler.Character_Property;
    }

}



public interface Character_INFO_Handler: Character_Binds_Handler 
{
    Character_Property Character_Property { get; set; }
    bool Binded { get; }
    void Binding(Character_Property Binding);

}

public interface Character_Binds_Handler
{
    void Binds(Input_Interface Input);
    void Binds(Item PlayerBag);
}

public class Character_INFO_Handle_Layer_Normal: Character_INFO_Handle_Layer_Base
{
    public Character_INFO_Handle_Layer_Normal() { }

}
public abstract class Character_INFO_Handle_Layer_Base : Character_INFO_Handler
{
    public Character_Property Character_Property
    {
        get { return this.character_Property; }
        set { this.character_Property = value; }
    }
    public bool Binded => this.INFO_Handle_Temp.Binded;

    private Character_Property character_Property;
    private Character_INFO_Handle_Temp INFO_Handle_Temp = new Character_INFO_Handle_Temp();

    void Character_INFO_Handler.Binding(Character_Property Binding) {
        this.character_Property = Binding;
        this.INFO_Handle_Temp.Binded = true;
    }

    #region Binds
    private bool Input_Binded => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeBinds.RuntimeBinds_Input.Binded;
    private bool Bag_Binded => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeBinds.RuntimeBinds_Bag.Binded;
    private bool Dialog_Binded => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeBinds.RuntimeBinds_Dialog.Binded;
    void Character_Binds_Handler.Binds(Input_Interface Input) {
        this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeBinds.RuntimeBinds_Input.Input = Input;
    }
    void Character_Binds_Handler.Binds(Item PlayerBag) {
        this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeBinds.RuntimeBinds_Bag.PlayerBag = PlayerBag;
    }
    #endregion
}

public class Character_INFO_Handle_Temp
{
    public bool Binded = false;
}





[AttributeUsage(AttributeTargets.Method,Inherited =true,AllowMultiple =true)]
public class InputRegisterAttribute : Attribute {
    public InputType InputType;
    InputRegisterAttribute(InputType inputType) {
        this.InputType = inputType;
    }
}
public enum InputType {
    MoveUp,
    MoveLeft,
    MoveRight,
    MoveDown,
    GetUpThingsInUpdateByRay,
    UseUpThingsInUpdateByRay,
    DropItem,
    Run,
    Use1,
    Use2,
    Use3,
    Use4,
    Use5,
    Use6,
    K1,
    K2,
    K3,
    K4,
    K5,
    K6,
    K7
}






public enum Moral_Axis
{
    Good,
    Neutral,
    Evil,
}
public enum Order_Horizontal
{
    Lawful,
    Neutral,
    Chaotic,
}

public class Character_Property
{
    public CharacterStaticProperties.CharacterStaticProperties CharacterStaticProperties;
    public CharacterRuntimeProperties.CharacterRuntimeProperties CharacterRuntimeProperties;

    public Character_Property() { }

    public Character_Property(int CharacterID,CharacterPreInstanceProperties.CharacterPreInstanceProperties characterPreInstanceProperties) {
        this.CharacterStaticProperties = StaticPath.CharacterLoad[CharacterID].CharacterStaticProperties;
        this.CharacterRuntimeProperties = new CharacterRuntimeProperties.CharacterRuntimeProperties(this.CharacterStaticProperties, characterPreInstanceProperties);
    }
    public Character_Property(CharacterRuntimeProperties.CharacterRuntimeProperties characterRuntimeProperties) {
        this.CharacterRuntimeProperties = characterRuntimeProperties;
        this.CharacterStaticProperties = StaticPath.CharacterLoad[this.CharacterRuntimeProperties.CharacterID].CharacterStaticProperties;

        this.CharacterRuntimeProperties.CharacterRuntimeTemps = new CharacterRuntimeProperties.CharacterRuntimeTemps();
        this.CharacterRuntimeProperties.CharacterRuntimeBinds = new CharacterRuntimeProperties.CharacterRuntimeBinds();
    }


}

namespace CharacterRuntimeProperties
{
    
    [Serializable]
    public class CharacterRuntimeValues
    {
        public RuntimeValues.RuntimeValues_Moral RuntimeValues_Moral = new RuntimeValues.RuntimeValues_Moral();
        public RuntimeValues.RuntimeValues_Shape RuntimeValues_Shape = new RuntimeValues.RuntimeValues_Shape();
        public RuntimeValues.RuntimeValues_State RuntimeValues_State = new RuntimeValues.RuntimeValues_State();
        public RuntimeValues.RuntimeValues_HeldState RuntimeValues_HeldState = new RuntimeValues.RuntimeValues_HeldState();

    }
    namespace RuntimeValues
    {
        [Serializable]
        public class RuntimeValues_State
        {
            public float HP_Curren_Max__Initial = 100;
            public float HP_Current__Initial = 100;
            public float DEF_Current__Initial = 0;
        }
        [Serializable]
        public class RuntimeValues_Moral
        {
            public Moral_Axis moral_Axis;
            public Order_Horizontal order_Horizontal;
        }
        [Serializable]
        public class RuntimeValues_Shape
        {
            public List<string> Names = new List<string>();
            public List<Transform> Shapes = new List<Transform>();

        }
        [Serializable]
        public class RuntimeValues_HeldState
        {
            public int HeldMark = 0;
            public int HeldMarkPre = 0;
        }
        
        

    }

    public class CharacterRuntimeTemps
    {
        public RuntimeTemps.RuntimeTemps_Bools RuntimeTemps_Bools = new RuntimeTemps.RuntimeTemps_Bools();
      
    }
    namespace RuntimeTemps
    {
        public class RuntimeTemps_Bools
        {
            public bool KeyChange = false;  
        }

    }
    public class CharacterRuntimeBinds
    {
        public RuntimeBinds.RuntimeBinds_Bag RuntimeBinds_Bag = new RuntimeBinds.RuntimeBinds_Bag();
        public RuntimeBinds.RuntimeBinds_Input RuntimeBinds_Input = new RuntimeBinds.RuntimeBinds_Input();
        public RuntimeBinds.RuntimeBinds_Dialog RuntimeBinds_Dialog = new RuntimeBinds.RuntimeBinds_Dialog();
    }
    namespace RuntimeBinds
    {
        public class RuntimeBinds_Bag : BindsInfo 
        {
            [RuntimeBind]
            public Item PlayerBag;
            public Item Held;
        }
        public class RuntimeBinds_Input : BindsInfo 
        {
            [RuntimeBind]
            public Input_Interface Input;
            public List<Action> ActionInput = new List<Action>();
        }
        public class RuntimeBinds_Dialog : BindsInfo
        {

        }


    }


    [Serializable]
    public class CharacterRuntimeProperties
    {
        public int CharacterID = -1;
        public bool inited = false;
        public CharacterRuntimeValues CharacterRuntimeValues = new CharacterRuntimeValues();
        public CharacterRuntimeTemps CharacterRuntimeTemps = new CharacterRuntimeTemps();
        public CharacterRuntimeBinds CharacterRuntimeBinds = new CharacterRuntimeBinds();

        public CharacterRuntimeProperties(CharacterStaticProperties.CharacterStaticProperties characterStaticProperties,CharacterPreInstanceProperties.CharacterPreInstanceProperties characterPreInstanceProperties) {
            if (!this.inited) {
                this.CharacterID = characterStaticProperties.CharacterID;




                RuntimeValues.RuntimeValues_Moral Init_Moral = this.CharacterRuntimeValues.RuntimeValues_Moral;
                CharacterStaticProperties.StaticValues.StaticValues_Moral staticValues_Moral = characterStaticProperties.StaticValues_Moral;
                Init_Moral.moral_Axis = staticValues_Moral.moral_Axis;
                Init_Moral.order_Horizontal = staticValues_Moral.order_Horizontal;

                RuntimeValues.RuntimeValues_Shape Init_Shape = this.CharacterRuntimeValues.RuntimeValues_Shape;
                CharacterStaticProperties.StaticValues.StaticValues_Shape staticValues_Shape = characterStaticProperties.StaticValues_Shape;
                Init_Shape.Names = staticValues_Shape.Names;
                Init_Shape.Shapes = staticValues_Shape.Shapes;

                RuntimeValues.RuntimeValues_State Init_State = this.CharacterRuntimeValues.RuntimeValues_State;
                CharacterStaticProperties.StaticValues.StaticValues_State staticValues_State = characterStaticProperties.StaticValues_State;
                Init_State.DEF_Current__Initial = staticValues_State.DEF_Origin;
                Init_State.HP_Current__Initial = staticValues_State.HP_Origin;
                Init_State.HP_Curren_Max__Initial = staticValues_State.HP_Origin_Max;

                RuntimeValues.RuntimeValues_HeldState Init_HeldState = this.CharacterRuntimeValues.RuntimeValues_HeldState;
                CharacterStaticProperties.StaticValues.StaticValues_HeldState staticValues_HeldState = characterStaticProperties.StaticValues_HeldState;
                Init_HeldState.HeldMark = staticValues_HeldState.HeldMark;
                Init_HeldState.HeldMarkPre = staticValues_HeldState.HeldMarkPre;

            }
            this.inited = true;
        }

    }
}



namespace CharacterStaticProperties
{

    [Serializable]
    public class CharacterStaticProperties
    {
        public int CharacterID = -1;
        public StaticValues.StaticValues_Moral StaticValues_Moral = new StaticValues.StaticValues_Moral();
        public StaticValues.StaticValues_Shape StaticValues_Shape = new StaticValues.StaticValues_Shape();
        public StaticValues.StaticValues_State StaticValues_State = new StaticValues.StaticValues_State();
        public StaticValues.StaticValues_HeldState StaticValues_HeldState = new StaticValues.StaticValues_HeldState();

    }
    namespace StaticValues
    {
        [Serializable]
        public class StaticValues_State
        {
            public float HP_Origin_Max = 100;
            public float HP_Origin = 100;
            public float DEF_Origin = 0;
        }
        [Serializable]
        public class StaticValues_Moral
        {
            public Moral_Axis moral_Axis;
            public Order_Horizontal order_Horizontal;
        }
        [Serializable]
        public class StaticValues_Shape
        {
            public List<string> Names = new List<string>();
            public List<Transform> Shapes = new List<Transform>();


        }
        [Serializable]
        public class StaticValues_HeldState
        {
            public int HeldMark = 0;
            public int HeldMarkPre = 0;
        }
    }

}
namespace CharacterPreInstanceProperties
{

    [Serializable]
    public class CharacterPreInstanceProperties
    {


    }
}





public class BindsInfo
{
    public bool Binded => binded();
    private bool binded() {
        FieldInfo[] fieldInfos = this.GetType().GetFields();
        bool flag = true;
        foreach(var info in fieldInfos) {
            if (info.GetCustomAttribute<RuntimeBindAttribute>() != null) {
                if (info.GetValue(this) == null) {
                    flag = false;
                }
            }
        }
        return flag;
    }
}
[AttributeUsage(AttributeTargets.Field, AllowMultiple =false,Inherited =false)]
public class RuntimeBindAttribute : Attribute { }