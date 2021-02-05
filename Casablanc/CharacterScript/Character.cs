using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;


[Character(0)]
public class Character1 : CharacterBase
{

}




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
    public static void AddGenerators(int CharacterID,Type type) {
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
    Character_UI_Handler Character_UI_Handler { get; }
    Character_Property GetCharacterProperty();

}
public interface Character: Character_Detail
{
    int ID { get; }
    Item Held { get; }
    Container Bag { get; }
    GameObject Instance { get; }
    void OnEnable();
    void Update();
    void FixedUpdate();
    void OnDisable();
    void Destory();
}

public abstract class CharacterBase : Character
{
    public int ID => this.Info_Handler.Character_Property.CharacterStaticProperties.CharacterID;
    public Item Held => this.Info_Handler.Held;
    public Container Bag => this.Info_Handler.Bag;
    public GameObject Instance => this.Info_Handler.Instance;
    public Character_UI_Handler Character_UI_Handler => Info_Handler;

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
        this.Info_Handler.Trigger_Binded(this._AfterItemProperty);
        ((AllContainer.CharacterStaticBag)this.Bag).Character = this;
        this.onEnable();
    }  
    void Character.OnDisable() {
        this.CharacterInfoStore.Save(this);
        this.onDisbale();
    }
    void Character.Update() {
        if (this.Info_Handler.Inited) {
            this.Info_Handler.Update();
            CharacterManager.Main = this;   //临时举措
        }
        this.update();
    }

    void Character.FixedUpdate() {
        this.Info_Handler.FixedUpdate();
    }
    void Character.Destory() {

    }
    public virtual void update() { }
    public virtual void onDisbale() { }
    public virtual void onEnable() { }
    public Character_Property GetCharacterProperty() {
        return this.INFO_Handler.Character_Property;
    }
    public virtual void _AfterItemProperty() { }

}



public interface Character_INFO_Handler: Character_Object_Handler, Character_Trigger_Handler, Character_Kinematic_Handler,
    Character_Container_Handler, Character_UI_Handler, Character_Values_Handler
{
    Character_Property Character_Property { get; set; }
    bool Binded { get; }
    bool Inited { get; }
    void Binding(Character_Property Binding);
    void FixedUpdate();
    void Update();
    void ReplaceInstance(GameObject Instance);

}
public interface Character_Object_Handler
{
    GameObject Instance { get; }
    Vector2 Heading { get; }
    Vector2 Handing { get; }
    void HeldUpdate();
}

public interface Character_Trigger_Handler
{
    void Trigger_Binded(Action action);
}
public interface Character_Kinematic_Handler
{
    bool IsGround { get; set; }
}
public interface Character_UI_Handler
{
    ItemOnTheGround ItemSelect { get; }
    int Heldnum { get; }
    float HPrate { get; }
    float PPrate { get; }
}
public interface Character_Container_Handler
{
    Item Held { get; set; }
    Container Bag { get; set; }
}
public interface Character_Values_Handler : Object_Values_Handler
{

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
    public bool Inited => this.INFO_Handle_Temp.Inited;

    private Character_Property character_Property;
    private Character_INFO_Handle_Temp INFO_Handle_Temp = new Character_INFO_Handle_Temp();


    private Input_Interface InputInterface => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Input.InputInterface;
    private InputRuntimeProperties.RuntimeValues.RuntimeValues_State State => InputInterface.Input_Property.InputRuntimeProperties.InputRuntimeValues.RuntimeValues_State;
    private CharacterRuntimeProperties.RuntimeValues.RuntimeValues_Kinematic Kinematic => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_Kinematic;
    public virtual void OnBinded() {
        this.ComponentsInit();
        this.HeldUpdate();

        this.InputInterface.RaycastMask = ~(1 << 11);   //无视前景

        this.InputInterface.RegisteInput(SwitchItem, InputType.K);
        this.InputInterface.RegisteInput(DropThingsOnHand, InputType.DropItem);
        this.InputInterface.RegisteInput(SwitchItemUp, InputType.WheelDown);
        this.InputInterface.RegisteInput(SwitchItemDown, InputType.WheelUp);
        this.InputInterface.RegisteInput(UseItem, InputType.Use);
        this.InputInterface.RegisteInput(GetUpThingsByRay, InputType.GetUpThingsByRay);
        this.InputInterface.RegisteInput(UseUpThingsByRay, InputType.UseUpThingsByRay);


        this.INFO_Handle_Temp.Inited = true;
    }

    public virtual void FixedUpdate() {
        this.Vector2Init();
        this.Move();
        this.Flip();
        this.HandRotate();
    }

    public virtual void Update() {
        this.SelctUpdate();
    }


    #region 运动问题

    public bool IsGround { get => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_Kinematic.IsGround; set => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_Kinematic.IsGround = value; }
    private void Move() {


        Kinematic.Vel = Vector3.zero;
        if (State.A) {
            Kinematic.Vel += Vector3.right;
        }
        if (State.D) {
            Kinematic.Vel += Vector3.left;
        }
        if (State.W) {
            Kinematic.Vel += Vector3.back;
        }
        if (State.S) {
            Kinematic.Vel += Vector3.forward;
        }
        Vector3 FinalVel;
        if (IsGround) {
            FinalVel = new Vector3(Kinematic.Vel.normalized.x * 5, this.Rigidbody.velocity.y, Kinematic.Vel.normalized.z * 5);
        }
        else {
            FinalVel = new Vector3(Kinematic.Vel.x, this.Rigidbody.velocity.y - 0.5f, Kinematic.Vel.z);
        }
        this.Rigidbody.velocity = FinalVel;
    }

    public Vector2 Heading { get => this.InputInterface.Input_Property.InputRuntimeProperties.InputRuntimeValues.RuntimeValues_Vector.Heading; set => this.InputInterface.Input_Property.InputRuntimeProperties.InputRuntimeValues.RuntimeValues_Vector.Heading = value; }
    public Vector2 Handing { get => this.InputInterface.Input_Property.InputRuntimeProperties.InputRuntimeValues.RuntimeValues_Vector.Handing; set => this.InputInterface.Input_Property.InputRuntimeProperties.InputRuntimeValues.RuntimeValues_Vector.Handing = value; }


    private void Vector2Init() {
        Plane Handplane = new Plane(Vector3.forward, this.Hand.position);
        Plane Headplane = new Plane(Vector3.forward, this.Head.position);
        Vector3 Hand_Toward_Mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Handplane.GetDistanceToPoint(Camera.main.transform.position)));
        Vector3 Head_Toward_Mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Headplane.GetDistanceToPoint(Camera.main.transform.position)));
        this.Handing = new Vector2(Hand_Toward_Mouse.x - Hand.position.x, Hand_Toward_Mouse.y - Hand.position.y);
        this.Heading = new Vector2(Head_Toward_Mouse.x - Head.position.x, Head_Toward_Mouse.y - Head.position.y);
    }
    private void ComponentsInit() {
        this.character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Unity.Rigidbody = this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Unity.Instance.GetComponent<Rigidbody>();
        this.character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Unity.Model = this.Instance.transform.FindSon("模型");
        this.character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Unity.Head = this.Instance.transform.FindSon("头部");
        this.character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Unity.Hand = this.Instance.transform.FindSon("手部");
        this.character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Unity.Flex = this.Instance.transform.FindSon("连接层");
    }


    private void Flip() {
        this.Flex.rotation = Quaternion.Euler(0, this.Heading.x > 0 ? 0 : 180, 0);
    }

    private void HandRotate() {
        Hand.rotation = Quaternion.Euler(0, this.Flex.rotation.eulerAngles.y, Mathf.Atan(Handing.y / Mathf.Abs(Handing.x)) * 180 / Mathf.PI);
    }

    #endregion

    #region INFO
    void Character_INFO_Handler.Binding(Character_Property Binding) {
        this.character_Property = Binding;
        this.INFO_Handle_Temp.Binded = true;
    }

    void Character_INFO_Handler.ReplaceInstance(GameObject Instance) {
        this.InstanceObj = Instance;
    }
    #endregion

    #region Object
    public Rigidbody Rigidbody => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Unity.Rigidbody;
    public GameObject Instance => InstanceObj;
    private GameObject InstanceObj { get => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Unity.Instance; set => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Unity.Instance = value; }
    private Transform Model => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Unity.Model;
    private Transform Head => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Unity.Head;
    private Transform Hand => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Unity.Hand;
    private Transform Flex => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Unity.Flex;
    #endregion
    #region Trigger
    void Character_Trigger_Handler.Trigger_Binded(Action action) {
        if (this.Binded) {
            if (!this.INFO_Handle_Temp.Trigger_Group.Trigger_Binded) {
                action.Invoke();
                OnBinded();
                this.INFO_Handle_Temp.Trigger_Group.Trigger_Binded = true;
            }
        }
    }

    #endregion

    #region Container
    public Item Held { get => Bag[HeldMark]; set => Bag[HeldMark] = value; }
    public Container Bag { get => (Container)this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Bags.Bag; set => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Bags.Bag = value; }

    private int HeldMark { get => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_HeldState.HeldMark; set => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_HeldState.HeldMark = value; }
    private void SwitchItem(int Heldnum) {
        if(Heldnum!= HeldMark) {
            Held.Destory();
            Held.Item_Status_Handler.GetWays = GetWays.Hand;
            HeldMark = Heldnum;
            HeldUpdate();           
        }
    }
    private void SwitchItemDown() {
        Held.Destory();
        Held.Item_Status_Handler.GetWays = GetWays.Hand;
        HeldMark--;
        if (HeldMark < 0) {
            HeldMark = 6;
        }
        HeldUpdate();
    }
    private void SwitchItemUp() {
        Held.Destory();
        Held.Item_Status_Handler.GetWays = GetWays.Hand;
        HeldMark++;
        if (HeldMark > 6) {
            HeldMark = 0;
        }
        HeldUpdate();
    }


    private void UseItem(int Usenum) {
        switch (Usenum) {
            case 1: Held.InterfaceUse1(); break;
            case 2: Held.InterfaceUse2(); break;
            case 3: Held.InterfaceUse3(); break;
            case 4: Held.InterfaceUse4(); break;
            case 5: Held.InterfaceUse5(); break;
        }

    }
    public void HeldUpdate() {
        if (Held != Items.Empty) {
            Held.Beheld(Hand);
            Held.Item_Status_Handler.GetWays = GetWays.Tool;
            if (Held.Item_Status_Handler.DisplayWays.Display_things) {
                ((Container)Held).UpdateDisplay();
            }
        }
        else {
            Held.Destory();
            Held = Items.Empty;
        }
    }
    
    ItemOnTheGround Character_UI_Handler.ItemSelect => OriginItem;
    int Character_UI_Handler.Heldnum => HeldMark;
    private ItemOnTheGround OriginItem { get => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Input.OriginItem; set => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Input.OriginItem = value; }
    private void SelctUpdate() {
        if (InputInterface.Hit) {
            if (InputInterface.RaycastHit.collider) {
                Collider collider = InputInterface.RaycastHit.collider;
                if (collider.gameObject.TryGetComponent<ItemOnTheGround>(out var itemOnTheGround)) {
                    if (OriginItem) {
                        OriginItem.selected = false;
                        OriginItem = null;
                    }
                    OriginItem = itemOnTheGround;
                    OriginItem.selected = true;
                }
                else if (collider.gameObject.TryGetComponent<ItemLeader>(out var itemLeader)) {
                    if (itemLeader.ITryGetComponent<ItemOnTheGround>(out var itemOnTheGround1)) {
                        if (OriginItem) {
                            OriginItem.selected = true;
                            OriginItem = null;
                        }
                        OriginItem = itemOnTheGround1;
                        OriginItem.selected = true;
                    }
                }
                else if (OriginItem) {
                    OriginItem.selected = false;
                    OriginItem = null;
                }
            }
        }
        
    }
    private void DropThingsOnHand() {
        if (Held != Items.Empty) {
            Held.Drop(this.Instance.transform.position);
            Held.Item_Status_Handler.GetWays = GetWays.Hand;
            Bag.DelItem(Held);
        }
    }
    private void GetUpThingsByRay() {
        if (OriginItem) {
            GetUpThings(OriginItem.itemOntheGround);
        }
    }
    private void UseUpThingsByRay() {
        if (OriginItem) {
            UseUpThings(OriginItem.itemOntheGround);
        }
    }


    private void UseUpThings(Item Target) {
        Target.Use6(Held, out var itemoutEx);
        Held = itemoutEx;
        HeldUpdate();
    }
    private void GetUpThings(Item item) {
        this.Bag.GetItemFormGround(item);
        HeldUpdate();
    }
    #endregion
    #region Values
    public virtual void BeDmged(float DMG) {
        this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_State.HP_Current__Initial -= DMG - this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_State.DEF_Current__Initial;
    }
    public virtual void BeFixed(float FIX) {
        this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_State.HP_Current__Initial += FIX;
    }

    #endregion

    #region UI
    public float HPrate => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_State.HP_Current__Initial / this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_State.HP_Curren_Max__Initial;
    public float PPrate => this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_State.PP_Current__Initial / this.Character_Property.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_State.PP_Curren_Max__Initial;
    #endregion
}

public class Character_INFO_Handle_Temp
{
    public Character_INFO_Handle_Temp_Trigger_Group Trigger_Group = new Character_INFO_Handle_Temp_Trigger_Group();
    public bool Binded = false;
    public bool Inited = false;
}
public class Character_INFO_Handle_Temp_Trigger_Group
{
    public bool Trigger_Binded = false;
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
    GetUpThingsByRay,
    UseUpThingsByRay,
    DropItem,
    Run,
    Use,
    Use1,
    Use2,
    Use3,
    Use4,
    Use5,
    Use6,
    K,
    K1,
    K2,
    K3,
    K4,
    K5,
    K6,
    K7,
    WheelUp,
    WheelDown,
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
        Binds();
    }
    public Character_Property(CharacterRuntimeProperties.CharacterRuntimeProperties characterRuntimeProperties) {
        this.CharacterRuntimeProperties = characterRuntimeProperties;
        this.CharacterStaticProperties = StaticPath.CharacterLoad[this.CharacterRuntimeProperties.CharacterID].CharacterStaticProperties;

        this.CharacterRuntimeProperties.CharacterRuntimeTemps = new CharacterRuntimeProperties.CharacterRuntimeTemps();
        Binds();      
    }

    private void Binds() {
        if (this.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_Bags.ItemInfoStore != null) {
            this.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Bags.Bag = this.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_Bags.ItemInfoStore.GetItem();
        }        
        else {
            throw new Exception("未发现物品存储");
        }


        if (this.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_Input.InputInterface != null) {
            this.CharacterRuntimeProperties.CharacterRuntimeTemps.RuntimeTemps_Input.InputInterface = this.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_Input.InputInterface;
        }       
        else {
            throw new Exception("未发现输入映射");
        }

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
        public RuntimeValues.RuntimeValues_Bags RuntimeValues_Bags = new RuntimeValues.RuntimeValues_Bags();
        public RuntimeValues.RuntimeValues_Input RuntimeValues_Input = new RuntimeValues.RuntimeValues_Input();
        public RuntimeValues.RuntimeValues_Kinematic RuntimeValues_Kinematic = new RuntimeValues.RuntimeValues_Kinematic();

    }
    namespace RuntimeValues
    {
        [Serializable]
        public class RuntimeValues_State
        {
            public float HP_Curren_Max__Initial = 100;
            public float HP_Current__Initial = 100;
            public float DEF_Current__Initial = 0;
            public float PP_Current__Initial = 100;
            public float PP_Curren_Max__Initial = 100;
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
            public List<Quaternion> Rotation = new List<Quaternion>();

        }
        [Serializable]
        public class RuntimeValues_HeldState
        {
            public int HeldMark = 0;
        }
        [Serializable]
        public class RuntimeValues_Bags
        {
            public ItemInfoStore ItemInfoStore;
        }
        [Serializable]
        public class RuntimeValues_Input
        {
            public InputBase InputInterface;
        }
        [Serializable]
        public class RuntimeValues_Kinematic
        {
            public float Vx=0, Vy=0, Vz=0;
            public bool IsGround = false;
            public Vector3 Vel = new Vector3();
        }



    }

    public class CharacterRuntimeTemps
    {
        public RuntimeTemps.RuntimeTemps_Bools RuntimeTemps_Bools = new RuntimeTemps.RuntimeTemps_Bools();
        public RuntimeTemps.RuntimeTemps_Bags RuntimeTemps_Bags = new RuntimeTemps.RuntimeTemps_Bags();
        public RuntimeTemps.RuntimeTemps_Input RuntimeTemps_Input = new RuntimeTemps.RuntimeTemps_Input();
        public RuntimeTemps.RuntimeTemps_Unity RuntimeTemps_Unity = new RuntimeTemps.RuntimeTemps_Unity();
    }
    namespace RuntimeTemps
    {
        public class RuntimeTemps_Bools
        {
            public bool KeyChange = false;
        }
        public class RuntimeTemps_Bags
        {
            public Item Bag;
        }
        public class RuntimeTemps_Input
        {
            public Input_Interface InputInterface;
            public ItemOnTheGround OriginItem;
        }
        public class RuntimeTemps_Unity
        {
            public Transform ParTransform;
            public GameObject Instance;
            public Rigidbody Rigidbody;
            public Transform Model;
            public Transform Hand;
            public Transform Head;
            public Transform Flex;
        }

    }



    [Serializable]
    public class CharacterRuntimeProperties
    {
        public int CharacterID = -1;
        public bool inited = false;
        public CharacterRuntimeValues CharacterRuntimeValues = new CharacterRuntimeValues();
        public CharacterRuntimeTemps CharacterRuntimeTemps = new CharacterRuntimeTemps();

        public CharacterRuntimeProperties(CharacterStaticProperties.CharacterStaticProperties characterStaticProperties,CharacterPreInstanceProperties.CharacterPreInstanceProperties characterPreInstanceProperties) {
            if (!this.inited) {
                this.CharacterID = characterStaticProperties.CharacterID;

                RuntimeValues.RuntimeValues_Moral Init_Moral = this.CharacterRuntimeValues.RuntimeValues_Moral;
                CharacterStaticProperties.StaticValues.StaticValues_Moral staticValues_Moral = characterStaticProperties.StaticValues_Moral;
                Init_Moral.moral_Axis = staticValues_Moral.moral_Axis;
                Init_Moral.order_Horizontal = staticValues_Moral.order_Horizontal;

                RuntimeValues.RuntimeValues_State Init_State = this.CharacterRuntimeValues.RuntimeValues_State;
                CharacterStaticProperties.StaticValues.StaticValues_State staticValues_State = characterStaticProperties.StaticValues_State;
                Init_State.DEF_Current__Initial = staticValues_State.DEF_Origin;
                Init_State.HP_Current__Initial = staticValues_State.HP_Origin;
                Init_State.HP_Curren_Max__Initial = staticValues_State.HP_Origin_Max;
                Init_State.PP_Current__Initial = staticValues_State.PP_Origin;
                Init_State.PP_Curren_Max__Initial = staticValues_State.PP_Origin_Max;

                RuntimeValues.RuntimeValues_HeldState Init_HeldState = this.CharacterRuntimeValues.RuntimeValues_HeldState;
                CharacterStaticProperties.StaticValues.StaticValues_HeldState staticValues_HeldState = characterStaticProperties.StaticValues_HeldState;
                Init_HeldState.HeldMark = staticValues_HeldState.HeldMark;

                RuntimeValues.RuntimeValues_Bags Binds_Bags = this.CharacterRuntimeValues.RuntimeValues_Bags;
                Binds_Bags.ItemInfoStore = characterPreInstanceProperties.PreInstance_Binds.ItemInfoStore;
              

                RuntimeValues.RuntimeValues_Input Binds_Input = this.CharacterRuntimeValues.RuntimeValues_Input;
                Binds_Input.InputInterface = characterPreInstanceProperties.PreInstance_Binds.InputInterface;


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
            public float PP_Origin = 100;
            public float PP_Origin_Max = 100;
        }
        [Serializable]
        public class StaticValues_Moral
        {
            public Moral_Axis moral_Axis;
            public Order_Horizontal order_Horizontal;
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
        public PreInstance.PreInstance_Binds PreInstance_Binds = new PreInstance.PreInstance_Binds();

    }


    namespace PreInstance
    {
        [Serializable]
        public class PreInstance_Binds
        {
            public ItemInfoStore ItemInfoStore;
            public InputBase InputInterface;
        }
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