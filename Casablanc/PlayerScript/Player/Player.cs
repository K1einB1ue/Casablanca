using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Player_Instance 
{
    Player Instance { get; set; }

}


public interface UIPlayer
{
    bool GetRunable();
}
public interface ValuePlayer 
{
    void DecHP(float HPDmg);
    void DecVIT(float VITDmg);
    float GetHP();
    float GetVIT();
    Vector2 GetHPv2();
    Vector2 GetVITv2();
    float GetHPrate();
    float GetVITrate();
    
}
public interface ItemPlayer {
    Vector3 GetHanding();
}
public interface PlayerManage
{
    int HashCode { get; set; }
    InputInterface InputInterface { get; }
}
public interface Player
{
    int HeldOrder { get; }
    Item Held { get; set; }
    CharacterGameObjectState PlayerGameObjectState { get; set; }
    CharacterBoolLock PlayerBoolLock { get; set; }
    CharacterStateSetting __PlayerStateSetting { get; set; }

    
    void __SetHP(float now, float max);
    void __SetVIT(float now, float max);
    void __SetReborn(Vector3 Pos);
    void __ExploreSwitch();
    void __ExploreSwitch(bool boolean);
    void Gain(Item item);
    Vector3 GetPosition();
    Transform GetMainTransform();
    Transform GetHandTransform();
    void __SetBackPack(Container container);
    void SetController(CharacterController controller);
    void SetRigidbody(Rigidbody rigidbody);
    void Freezing();
    void _UseUpThings(Item Target,Item item,out Item itemoutEX);
    void _GetUpThings(Item item);
    void _GetUpThings(Collision collision);
    void _GetUpThings(Collider collider);
    void GetUpThingsInUpdateByRay(bool Trigger);
    void UseUpThingsInUpdateByRay(bool Trigger);
    void _DropThrewpre(Vector3 dir);
    void _DropThingsOnHand(bool Loop);
    void _DropThingsOnHand();
    void _DestroyThingsOnHand();
    void _ThrewButNotCleanBag(Item item);
    void LerpTo(Vector3 Pos, float index);
    Item GetStaticBag();
    void __fixedupdate();
    void __update();
    void __SetWalkVelocity(float Velocity);
    void __SetRunVelocity(float Velocity);
    /// <summary>
    /// 设置人物移动速度
    /// </summary>
    /// <param name="Velocity1"> 行走速度基数 </param>
    /// <param name="Velocity2"> 奔跑速度基数 </param>
    void __SetVelocity(float Velocity1, float Velocity2);
    void __SetGravity(float Gravity);
    void __SetCam(Camera camera);
    ItemNodeDynamic ItemInfoPackup();
    void Heldupdate();
}

public abstract class CharacterStatic : Player,ValuePlayer,UIPlayer, PlayerManage, ItemPlayer
{
    public CharacterGameObjectState PlayerGameObjectState { get; set; }
    public CharacterBoolLock PlayerBoolLock { get; set; }
    public CharacterStateSetting __PlayerStateSetting { get; set; }
    int Player.HeldOrder { get { return this.HeldMark; } }
    public Item Held { 
        get {
            if (this.PlayerStaticBag == null) {
                return Items.Empty;
            }
            else {
                return ((Container)this.PlayerStaticBag).GetContainerState().Contents[HeldMark];
            }
        }
        set {
            if (this.PlayerStaticBag != null) {
                ((ScriptContainer)this.PlayerStaticBag).SetItem(HeldMark, value);
            }
        } 
    }
    private int HeldMark1 = 0;
    private int HeldMark = 0;
    private bool Keychanged = false;


    public PlayerValueState PlayerValueState = new PlayerValueState();
    public Item PlayerStaticBag;
    //public Item Held2 = new Empty();

    public CharacterStatic(CharacterController controller) {
        this.Held = Items.Empty;
        this.__PlayerStateSetting = new CharacterStateSetting();
        this.PlayerBoolLock = new CharacterBoolLock();
        this.PlayerGameObjectState = new CharacterGameObjectState(controller);
        //this.InputInterface = inputInterface;
    }
    public CharacterStatic(Rigidbody rigidbody) {
        this.Held = Items.Empty;
        this.__PlayerStateSetting = new CharacterStateSetting();
        this.PlayerBoolLock = new CharacterBoolLock();
        this.PlayerGameObjectState = new CharacterGameObjectState(rigidbody);
        //this.InputInterface = inputInterface; 
    }

    private void BackPackSwitch() {
        if (InputInterface != null) {
            if (InputInterface.InputState.K != null) {
                for (int i = 0; i < 7; i++) {
                    if (InputInterface.InputState.K[i]) {
                        this.HeldMark1 = i;
                        this.Keychanged = true;
                    }
                }
            }
        }
        if ((this.HeldMark1 != this.HeldMark)||this.Keychanged) {
            Heldupdate();
            this.Keychanged = false;
        }
    }
    public void Heldupdate() {
        Held.Destory();
        Held.Item_Status_Handler.GetWays = GetWays.Hand;
        this.HeldMark = this.HeldMark1;
        if (Held!=Items.Empty) {
            Held.Beheld(((Player)this).GetHandTransform());
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
    private void Flip() {
        if (this.PlayerGameObjectState.GetGameObject().transform.rotation.y == 0) {
            this.PlayerGameObjectState.GetGameObject().transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (this.PlayerGameObjectState.GetGameObject().transform.rotation.y == 180) {
            this.PlayerGameObjectState.GetGameObject().transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    private void FlipTo(bool Input) {
            this.PlayerGameObjectState.GetGameObject().transform.rotation = Quaternion.Euler(0, Input ? 180 : 0, 0);
            this.PlayerGameObjectState.GetGameObject().transform.rotation = Quaternion.Euler(0, Input ? 180 : 0, 0);
    }
    private void FaceingFlip() {
        if (this.PlayerGameObjectState.GetMouseFlip()) {
            FlipTo(this.PlayerGameObjectState.GetFacingBoolean());
        }
    }
    private void TriggerConnect() { 
        if(this.InputInterface!=null){
            Held.TriggerLoop1(this.InputInterface.InputState.Use1);
            Held.TriggerLoop2(this.InputInterface.InputState.Use2);
            //Held.TriggerLoop3(this.InputInterface.InputState.Use3);
            //Held.TriggerLoop4(this.InputInterface.InputState.Use4);
            //Held.TriggerLoop5(this.InputInterface.InputState.Use5);
            Held.TriggerLoop3(this.InputInterface.InputState.R);
        }        
    }
    public virtual void Death() {
        if (this.__PlayerStateSetting.DropItem) {
            ((Container)this.PlayerStaticBag).DropAllDel(((Player)this).GetPosition());
        }
        if (this.__PlayerStateSetting.Rebornable) {
            this.PlayerGameObjectState.GetGameObject().transform.position = this.PlayerGameObjectState.GetRebornMark();
            this.PlayerValueState.Fullize();
        }
        if (this.__PlayerStateSetting.FalseWhenDeath) {
            this.PlayerGameObjectState.GetGameObject().SetActive(false);
        }
        //this.PlayerValueState.Fullize();
    }
    
    
    void Player.__fixedupdate() {
        if (InputInterface != null) {
            this.PlayerGameObjectState.update(InputInterface,this.__PlayerStateSetting.Main);        //务必最优先,涉及人物属性刷新               
        }
        this.TriggerConnect();
        this.VITrecover();
        this.RunStart();
        this.RunVITcost();
        this.PlayerStaticBag.update();
    }
    void Player.__update() {
        this.StateSwitch();
        if (this.InputInterface != null) {
            ((Player)this)._DropThingsOnHand(this.InputInterface.InputState.G);
        }
        if (this.PlayerValueState.DeathCheck()) {                                         
            this.Death();                                                                 
        }
    }

    void Player.__SetReborn(Vector3 Pos) {
        this.PlayerGameObjectState.SetRebornMark(Pos);
    }
    Vector3 Player.GetPosition() {
        return this.PlayerGameObjectState.GetGameObject().transform.position;
    }
    Transform Player.GetMainTransform() {
        if (this.PlayerGameObjectState.TypeController)
            return this.PlayerGameObjectState.GetController().transform;
        else
            return this.PlayerGameObjectState.GetRigidbody().transform;
    }
    void Player.__SetCam(Camera camera) {
        this.PlayerGameObjectState.Camera = camera;
    }
    void Player.SetController(CharacterController controller) {
        this.PlayerGameObjectState.SetCharacterController(controller);
        this.PlayerGameObjectState.TypeController = true;
    }
    void Player.SetRigidbody(Rigidbody rigidbody) {
        this.PlayerGameObjectState.SetRigidbody(rigidbody);
        this.PlayerGameObjectState.TypeController = false;
    }
    void Player.__SetBackPack(Container container) {
        this.PlayerStaticBag = (Item)container;
    }
    void Player.__SetWalkVelocity(float Velocity) {
        this.PlayerGameObjectState.SetWalkVelocity(Velocity);
    }
    void Player.__SetRunVelocity(float Velocity) {
        this.PlayerGameObjectState.SetRunVelocity(Velocity);
    }
    void Player.__SetVelocity(float Velocity1, float Velocity2) {
        this.PlayerGameObjectState.SetWalkVelocity(Velocity1);
        this.PlayerGameObjectState.SetRunVelocity(Velocity2);
    }
    void Player.__SetGravity(float Gravity) {
        this.PlayerGameObjectState.SetGravity(Gravity);
    }
    void Player.Freezing() {
            this.PlayerGameObjectState.Freezing();
    }
    void Player._UseUpThings(Item Target,Item item,out Item itemoutEX) {           //后续写入概率系统
        Target.Use6(item,out Item itemoutEx);
        ((Container)this.PlayerStaticBag).DelItem(item);
        itemoutEX = itemoutEx;
        Heldupdate();
    }
    void Player._GetUpThings(Collision collision) {
        if (collision.gameObject.TryGetComponent<ItemOnTheGround>(out ItemOnTheGround itemOnTheGround)) {
            if (itemOnTheGround.itemOntheGround != Held) {
                ((Container)PlayerStaticBag).GetItemFormGround(itemOnTheGround.itemOntheGround);
                Heldupdate();
            }
        }else if(collision.gameObject.TryGetComponent<ItemLeader>(out ItemLeader itemLeader)){
            if(collision.gameObject.TryGetComponent<ItemOnTheGround>(out ItemOnTheGround itemOnTheGround1)) {
                ((Container)PlayerStaticBag).GetItemFormGround(itemOnTheGround1.itemOntheGround);
                Heldupdate();
            }
        }
    }
    void Player._GetUpThings(Collider collider) {
        if (collider.gameObject.TryGetComponent<ItemOnTheGround>(out ItemOnTheGround itemOnTheGround)) {
            if (itemOnTheGround.itemOntheGround != Held) {
                ((Container)PlayerStaticBag).GetItemFormGround(itemOnTheGround.itemOntheGround);
                Heldupdate();
            }
        }
        else if (collider.gameObject.TryGetComponent<ItemLeader>(out ItemLeader itemLeader)) {
            if (collider.gameObject.TryGetComponent<ItemOnTheGround>(out ItemOnTheGround itemOnTheGround1)) {
                ((Container)PlayerStaticBag).GetItemFormGround(itemOnTheGround1.itemOntheGround);
                Heldupdate();
            }
        }
    }
    void Player._GetUpThings(Item item) {
        ((Container)PlayerStaticBag).GetItemFormGround(item);
        Heldupdate();
    }

    void Player.GetUpThingsInUpdateByRay(bool Trigger) {
        if (Trigger) {
            if (PlayerBoolLock.Origin != null) {
                if (PlayerBoolLock.HasHit) {
                    if (Math.Distance(PlayerBoolLock.Hit.transform.position, this.PlayerGameObjectState.GetGameObject().transform.position) <= this.__PlayerStateSetting.__ActiveRange) {
                        ((Player)this)._GetUpThings(PlayerBoolLock.Origin.itemOntheGround);
                    }
                }
            }
        }
    }
    void Player.UseUpThingsInUpdateByRay(bool Trigger) {
        if (Trigger) {
            if (PlayerBoolLock.Origin != null) {
                if (PlayerBoolLock.HasHit)
                    if (Math.Distance(PlayerBoolLock.Hit.transform.position, this.PlayerGameObjectState.GetGameObject().transform.position) <= this.__PlayerStateSetting.__ActiveRange) { 
                    ((Player)this)._UseUpThings(PlayerBoolLock.Origin.itemOntheGround, Held, out Item itemoutEX);
                    Held = itemoutEX;
                }
            }
        }
    }
    void Player._DropThrewpre(Vector3 dir) {
        if (!object.Equals(Held, Items.Empty)) {
            Held.Drop(Held.Instance.transform.position);
            Held.Instance.GetComponent<Rigidbody>().AddForce(dir);
            Held.Item_Status_Handler.GetWays = GetWays.Hand;
            ((Container)((Player)this).GetStaticBag()).DelItem(Held);
            Held = Items.Empty;
        }
    }
    void Player._DropThingsOnHand(bool Loop) {
        if(Loop&&Held!=null) {
            if (!object.Equals(Held, Items.Empty)) {
                Held.Drop(((Player)this).GetPosition());
                Held.Item_Status_Handler.GetWays = GetWays.Hand;
                ((Container)((Player)this).GetStaticBag()).DelItem(Held);
                Held = Items.Empty;
            }
        }
    }
    void Player._DropThingsOnHand() {
        ((Player)this)._DropThingsOnHand(true);
    }
    void Player._ThrewButNotCleanBag(Item item) {
        ((Container)((Player)this).GetStaticBag()).DelItem(item);
    }
    void Player._DestroyThingsOnHand() {
        if (!object.Equals(Held, Items.Empty)) {
            Held.Destory();
            Held.Item_Status_Handler.GetWays = GetWays.Hand;
            ((Container)((Player)this).GetStaticBag()).DelItem(Held);
            Held = Items.Empty;
        }
    }
    Item Player.GetStaticBag() {
        return this.PlayerStaticBag;
    }
    void Player.Gain(Item item) {
        Held.Item_Status_Handler.GetWays = GetWays.Hand;
        ((Container)this.PlayerStaticBag).AddItem(item);
    }
    Transform Player.GetHandTransform() {
        return this.PlayerGameObjectState.GetHandTransform();
    }
    void Player.__SetHP(float now, float max) {
        this.PlayerValueState.__SetHP(now, max);
    }
    void Player.__SetVIT(float now, float max) {
        this.PlayerValueState.__SetVIT(now, max);
    }
    void Player.__ExploreSwitch() {
        this.PlayerGameObjectState.Exploring();
    }
    void Player.__ExploreSwitch(bool boolean) {
        this.PlayerGameObjectState.Exploring(boolean);
    }
    void Player.LerpTo(Vector3 Pos, float index) {
        if (index < 1) {
            this.PlayerGameObjectState.GetGameObject().transform.position = new Vector3(
                Mathf.Lerp(this.PlayerGameObjectState.GetGameObject().transform.position.x, Pos.x, index),
                Mathf.Lerp(this.PlayerGameObjectState.GetGameObject().transform.position.y, Pos.y, index),
                Mathf.Lerp(this.PlayerGameObjectState.GetGameObject().transform.position.z, Pos.z, index)
                );
        }
    }
    //*********************************************************ValuePlayer*******************************************************************//
    void ValuePlayer.DecHP(float HPDmg) {
        if (!this.__PlayerStateSetting.HPInvincible){
            this.PlayerValueState.DecHP(HPDmg);
        }
    }
    void ValuePlayer.DecVIT(float VITDmg) {
        if (!this.__PlayerStateSetting.VITInvincible) {
            this.PlayerValueState.DecVIT(VITDmg);
        }
    }
    float ValuePlayer.GetHP() {
        return this.PlayerValueState.GetHP();
    }
    float ValuePlayer.GetVIT() {
        return this.PlayerValueState.GetVIT();
    }
    Vector2 ValuePlayer.GetHPv2() {
        return new Vector2(this.PlayerValueState.GetHP(), this.PlayerValueState.GetHPmax());
    }
    Vector2 ValuePlayer.GetVITv2() {
        return new Vector2(this.PlayerValueState.GetVIT(), this.PlayerValueState.GetVITmax());
    }
    float ValuePlayer.GetHPrate() {
        return this.PlayerValueState.GetHP() / this.PlayerValueState.GetHPmax();
    }
    float ValuePlayer.GetVITrate() {
        return this.PlayerValueState.GetVIT() / this.PlayerValueState.GetVITmax();
    }
    //**********************************************************UIPlayer*************************************************************************//
    bool UIPlayer.GetRunable() {
        return this.PlayerGameObjectState.GetRunable();
    }
    //**********************************************************PlayerManage********************************************************************//
    private PlayerManageInfo PlayerManageInfo = new PlayerManageInfo();
    int PlayerManage.HashCode { get { return this.PlayerManageInfo.Hash; } set { this.PlayerManageInfo.Hash = value; } }
    public InputInterface InputInterface { get { return this.PlayerManageInfo.InputInterface; } }
    //**********************************************************Private*************************************************************************//
    private void StateSwitch() {
        if (InputInterface != null) {
            if (this.__PlayerStateSetting.GetUpThingsInUpdateByRay) ((Player)this).GetUpThingsInUpdateByRay(this.InputInterface.InputState.GetUpThingsInUpdateByRay);
            if (this.__PlayerStateSetting.UseUpThingsInUpdateByRay) ((Player)this).UseUpThingsInUpdateByRay(this.InputInterface.InputState.UseUpThingsInUpdateByRay);
        }
        if (this.__PlayerStateSetting.RotateByFacing) this.PlayerGameObjectState.SetFacingRot(this.__PlayerStateSetting.RotateByFacing);
        if (this.__PlayerStateSetting.FlipByMouse) this.PlayerGameObjectState.SetMouseFlip(this.__PlayerStateSetting.FlipByMouse);
        if (this.__PlayerStateSetting.Control) this.PlayerGameObjectState.SetControl(this.__PlayerStateSetting.Control);
        if (this.__PlayerStateSetting.BackPackSwitch) this.BackPackSwitch();
        if (this.__PlayerStateSetting.SelectMark) this.ItemSelect();

    }
    private void ItemSelect() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << 11;
        layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, layerMask)) {
            this.PlayerBoolLock.Hit = hit;
            this.PlayerBoolLock.HasHit = true;
            if (hit.collider.gameObject.TryGetComponent<ItemOnTheGround>(out ItemOnTheGround itemOnTheGround)) {
                if (this.PlayerBoolLock.Origin != null) {
                    this.PlayerBoolLock.Origin.selected = false;
                    this.PlayerBoolLock.Origin = null;
                }
                this.PlayerBoolLock.Origin = itemOnTheGround;
                this.PlayerBoolLock.Origin.selected = true;
            }
            else if (hit.collider.gameObject.TryGetComponent<ItemLeader>(out ItemLeader itemLeader)) {
                if (itemLeader.ITryGetComponent<ItemOnTheGround>(out ItemOnTheGround itemOnTheGround1)) {
                    if (this.PlayerBoolLock.Origin != null) {
                        this.PlayerBoolLock.Origin.selected = false;
                        this.PlayerBoolLock.Origin = null;
                    }
                    this.PlayerBoolLock.Origin = itemOnTheGround1;
                    this.PlayerBoolLock.Origin.selected = true;
                }
            }
            else if (this.PlayerBoolLock.Origin!=null) {
                this.PlayerBoolLock.Origin.selected = false;
                this.PlayerBoolLock.Origin = null;
            }           
        }
    }
    private void RunVITcost() {
        if (this.InputInterface!= null) {
            if (this.InputInterface.InputState.Run && this.PlayerGameObjectState.GetRunable() && (
                this.InputInterface.InputState.W ||
                this.InputInterface.InputState.A ||
                this.InputInterface.InputState.S ||
                this.InputInterface.InputState.D

                )) {
                this.PlayerValueState.DecVIT(1.0f);
                if (this.PlayerValueState.GetVIT() <= 0) {
                    this.PlayerGameObjectState.SetRunable(false);
                }
            }
        }
    }
    private void VITrecover() {
        if (this.InputInterface != null) {
            if (!this.PlayerGameObjectState.GetRunable()) {
            this.PlayerValueState.IncVIT(0.2f);
            }
            else if (!InputInterface.InputState.Run||!(
                this.InputInterface.InputState.W &&
                this.InputInterface.InputState.A &&
                this.InputInterface.InputState.S &&
                this.InputInterface.InputState.D)) {
                this.PlayerValueState.IncVIT(0.5f);
            }
        }
    }

    Vector3 ItemPlayer.GetHanding() {
        if (this.InputInterface!=null) {
            return this.InputInterface.StateStore.Handing + 2 * Vector3.down * this.InputInterface.StateStore.Handing;
        }
        return Vector3.up;
    }
    private void RunStart() {
        if (((ValuePlayer)this).GetVITrate() >= 0.5f) {
            this.PlayerGameObjectState.SetRunable(true);
        }
    }

    public ItemNodeDynamic ItemInfoPackup() {
        ItemNodeDynamic All = new ItemNodeDynamic(((Player)this).GetStaticBag());
        All.ItemContain = ((ScriptContainer)((Player)this).GetStaticBag()).GetItemNodes();
        return All;
    }
}
public class PlayerManageInfo
{
    public InputInterface InputInterface { get { return PlayerManager.GetInputInterface(this.Hash); } }
    public int Hash = -1;
}

public class CharacterBoolLock
{
    public RaycastHit Hit = new RaycastHit();
    public bool HasHit = false;
    public ItemOnTheGround Origin = null;
    public bool _HP_Decmark = false;
    public bool _SwitchStay = false;
}
public class PlayerValueState
{
    private float HP;
    private float HPmax;
    private float VIT;
    private float VITmax;
    public bool RecoverableVIT = true;
    public bool RecoverableHP = true;
    public bool HP_Decmark = false;
     
    public bool DeathCheck() {
        if (HP <= 0) {
            return true;
        }
        return false;
    }
    public bool TiredCheck() {
        if (VIT <= 0) {
            return true;
        }
        return false;
    }
    public void Recover(float index) {
        if (RecoverableHP) {
            HP += index;
        }
    }
    public void __SetHP(float x, float y) {
        HP = x;
        HPmax = y;
    }
    public void __SetVIT(float x,float y) {
        VIT = x;
        VITmax = y;
    }
    public void DecHP(float x) {
        this.HP -= x;
        this.HP_Decmark = true;
    }
    public void DecVIT(float x) {
        this.VIT -= x;
    }
    public void IncHP(float x) {
        if (this.HP + x < this.HPmax) {
            this.HP += x;
        }
        else if (this.HP + x >= this.HPmax) {
            this.HP = this.HPmax;
        }
    }
    public void IncVIT(float x) {
        if (this.VIT + x < this.VITmax) {
            this.VIT += x;
        }
        else if (this.VIT + x >= this.VITmax) {
            this.VIT = this.VITmax;
        }
    }
    public float GetHP() {
        return this.HP;
    }
    public float GetVIT() {
        return this.VIT;
    }
    public float GetHPmax() {
        return this.HPmax;
    }
    public float GetVITmax() {
        return this.HPmax;
    }
    public void Fullize() {
        this.HP = this.HPmax;
        this.VIT = this.VITmax;
    }
}


public class CharacterGameObjectState
{
    private CharacterController controller;
    public GameObject Head;
    public GameObject Body;
    public GameObject Hand;
    private GameObject FeetPoint;
    private Rigidbody rigidbody;
    private Vector3 rebornMark = new Vector3();

    public bool TypeController;

    private float Vgravity = 1.0f;
    private float WalkVelocity = 0.3f;
    private float RunVelocity = 1.2f;
    private Vector3 Vel;
    private Vector3 FinalVel;

    private Vector2 Facing;
    private Vector2 Handing;
    private float RotateRate = 20;

    private bool MouseFlip = false;

    private bool FacingRot = false;

    private bool Frozen = false;

    private bool Gravity = true;

    private bool InControl = false;

    private bool Explore = false;

    private bool Runable = true;

    private bool IsGrounded = true;

    private bool IsTroubled = false;

    public Camera Camera;






    public CharacterGameObjectState(CharacterController controller) {
        this.controller = controller;

        this.Head = controller.transform.Find("Head").gameObject;
        this.Hand = controller.transform.Find("Hand").gameObject;
        this.FeetPoint = controller.transform.Find("FeetPoint").gameObject;

        /*
        this.Body = controller.transform.Find("Body").gameObject;
        */
        Vel = new Vector3(0, 0, 0);
        TypeController = true;
    }
    
    public CharacterGameObjectState(Rigidbody rigidbody) {
        this.rigidbody = rigidbody;

        this.Head = rigidbody.transform.Find("Head").gameObject;
        this.Hand = rigidbody.transform.Find("Hand").gameObject;
        this.FeetPoint = rigidbody.transform.Find("FeetPoint").gameObject;

        Vel = new Vector3(0, 0, 0);
        TypeController = false;
    }
    

    public void update(InputInterface inputInterface,bool Main) {
        if (!Frozen && TypeController && InControl) {
            this.GetIsGrounded();
            if (this.IsGrounded) {
                this.ResetVel();
                this.GetV2M(inputInterface.InputState.W, inputInterface.InputState.S, inputInterface.InputState.A, inputInterface.InputState.D);
            }
            this.FaceingFlip(); ;
            this.GetV2G();
            this.Move(inputInterface.InputState.Run);
            this.GetFacing(inputInterface.StateStore.Facing, inputInterface.StateStore.Handing);
            this.Vector2Rotate(Hand.transform, Handing);
            this.Vector2Rotate(Head.transform, Facing);
        }
        else if (!Frozen && !TypeController && InControl) {
            this.GetIsGrounded();
            this.GetIsTroubled();
            if (this.IsGrounded || !this.IsTroubled) {
                this.ResetVel();
                this.GetV2M(inputInterface.InputState.W, inputInterface.InputState.S, inputInterface.InputState.A, inputInterface.InputState.D);
            }
            this.FaceingFlip();
            this.MoveByRig(inputInterface.InputState.Run);
            this.GetFacing(inputInterface.StateStore.Facing, inputInterface.StateStore.Handing);
            this.Vector2Rotate(Hand.transform, Handing);
            this.Vector2Rotate(Head.transform, Facing); 
        }
    }
    private void FlipTo(bool Input) {
        this.GetGameObject().transform.rotation = Quaternion.Euler(0, Input ? 180 : 0, 0);
        this.GetGameObject().transform.rotation = Quaternion.Euler(0, Input ? 180 : 0, 0);
    }
    private void FaceingFlip() {
        if (this.MouseFlip) {
            FlipTo(this.GetFacingBoolean());
        }
    }
    public void MoveByRig(bool Run) {
        if (Run && IsGrounded && this.Runable) {
            FinalVel = new Vector3(Vel.normalized.x * 100 * WalkVelocity * RunVelocity, this.rigidbody.velocity.y, Vel.normalized.z * 100 * WalkVelocity * RunVelocity);
        }
        else if (IsGrounded) {
            FinalVel = new Vector3(Vel.normalized.x * 100 * WalkVelocity, this.rigidbody.velocity.y, Vel.normalized.z * 100 * WalkVelocity);
        }
        else {
            FinalVel = new Vector3(FinalVel.x, this.rigidbody.velocity.y - 0.5f, FinalVel.z);
        }
        this.rigidbody.velocity = FinalVel;
    }
    public GameObject GetGameObject() {
        if (TypeController)
            return this.controller.gameObject;
        if (!TypeController)
            return this.rigidbody.gameObject;
        return null;
    }
    public Rigidbody GetRigidbody() {
        return this.rigidbody;
    }
    public CharacterController GetController() {
        return this.controller;
    }
    public void SetCharacterController(CharacterController controller) {
        this.controller = controller;
    }
    public void SetRigidbody(Rigidbody rigidbody) {
        this.rigidbody = rigidbody;
    }
    public void SetGravity(float Gravity) {
        this.Vgravity = Gravity;
    }
    public void SetWalkVelocity(float Velocity) {
        this.WalkVelocity = Velocity;
    }
    public void SetRunVelocity(float RunVelocity) {
        this.RunVelocity = RunVelocity;
    }
    public void SetControl(bool index) {
        this.InControl = index;
    }
    public void Freezing() {
        if (!Frozen) {
            this.Frozen = true;
        }
        else {
            this.Frozen = false;
        }
    }
    public void Exploring(bool boolean) {
        this.Explore = boolean;
    }
    public void Exploring() {
        if (!Explore) {
            this.Explore = true;
        }
        else {
            this.Explore = false;
        }
    }
    public void SetRunable(bool boolean) {
        this.Runable = boolean;
    }
    public bool GetRunable() {
        return this.Runable;
    }
    public void SetFacingRot(bool index) {
        this.FacingRot = index;
    }
    public void SetMouseFlip(bool index) {
        this.MouseFlip = index;
    }
    public void SetRebornMark(Vector3 pos) {
        this.rebornMark = pos;
    }
    public Vector3 GetRebornMark() {
        return this.rebornMark;
    }
    public bool GetMouseFlip() {
        return this.MouseFlip;
    }
    private void Move(bool Run) {
        if (Run && this.Runable)  {
            this.controller.Move(Vel * WalkVelocity * RunVelocity);
        }
        else {
            this.controller.Move(Vel * WalkVelocity);
        }
    }
    private void GetV2M(bool W,bool S,bool A,bool D) {
        if (this.Explore) {
            if (W) {
                Vel += Vector3.back;
            }
            if (S) {
                Vel += Vector3.forward;
            }
        }
        if (A) {
            Vel += Vector3.right;
        }
        if (D) {
            Vel += Vector3.left;
        }
    }
    private void GetV2G() {
        if (this.Gravity) {
            if (!controller.isGrounded) {
                this.Vel += Vector3.down * Vgravity;
            }
        }
    }
    private void Vector2Rotate(Transform transform, Vector2 vector2) {
        if (this.FacingRot) {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, this.GetFacingBoolean() ? Mathf.Atan(vector2.y / vector2.x) * Mathf.PI / 2 * 44 : -Mathf.Atan(vector2.y / vector2.x) * Mathf.PI / 2 * 45);
            //float angle = transform.rotation.z > (this.GetFacingBoolean() ? Mathf.Atan(vector2.y / vector2.x) * Mathf.PI / 2 * 44 : -Mathf.Atan(vector2.y / vector2.x) * Mathf.PI / 2 * 45) ? RotateRate : -RotateRate;
            //if (angle < RotateRate) {
            //    angle = 0;
            //    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, this.GetFacingBoolean() ? Mathf.Atan(vector2.y / vector2.x) * Mathf.PI / 2 * 44 : -Mathf.Atan(vector2.y / vector2.x) * Mathf.PI / 2 * 45);
            //}
            //else {
            //    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + angle);
            //}
        }
    }
    private void ResetVel() {
        this.Vel = new Vector3(0, 0, 0);
    }
    private void GetFacing(Vector2 Facing,Vector2 Handing) {
        this.Facing = Facing;
        this.Handing = Handing;
    }
    private void GetIsGrounded() {
        if (TypeController) {
            this.IsGrounded = controller.isGrounded;
        }
        else if (!TypeController){            
            if (!Physics.Raycast(this.FeetPoint.transform.position, Vector3.down, 0.05f)) {
                this.IsGrounded = false;
            }
            else {
                this.IsGrounded = true;
            }
        }
    }
    private void GetIsTroubled() {
        if (rigidbody.velocity.x + rigidbody.velocity.z == 0) {
            this.IsTroubled = true;
        }
        else {
            this.IsTroubled = false;
        }
    }
    public bool GetFacingBoolean() {
        if (this.Facing.x > 0) {
            return false;
        }
        else {
            return true;
        }
    }
    public Transform GetHandTransform() {
        return this.Hand.transform;
    }
    public Transform GetHeadTransform() {
        return this.Head.transform;
    }
    public Transform GetBodyTransform() {
        return this.Body.transform;
    }
}

public class CharacterStateSetting 
{
    public bool Main = false;
    public bool Rebornable = false;
    public bool DropItem = false;
    public bool FalseWhenDeath = false;
    public bool HPInvincible = false;
    public bool VITInvincible = false;
    public bool UseUpThingsInUpdateByRay = false;
    public bool GetUpThingsInUpdateByRay = false;
    public bool FlipByMouse = false;
    public bool RotateByFacing = false;
    public bool Control = false;
    public bool BackPackSwitch = false;
    public bool SelectMark = false;

    public void __SetMainPlayer() {
        this.Main = true;
        this.Control = true;
        this.RotateByFacing = true;
        this.FlipByMouse = true;
        this.BackPackSwitch = true;
        this.UseUpThingsInUpdateByRay = true;
        this.GetUpThingsInUpdateByRay = true;
        this.SelectMark = true;
    }
    public void __OFF() {
        this.Main = false;
        this.Control = false;
        this.RotateByFacing = false;
        this.FlipByMouse = false;
        this.BackPackSwitch = false;
        this.UseUpThingsInUpdateByRay = false;
        this.GetUpThingsInUpdateByRay = false;
        this.SelectMark = false;
    }
    public float __ActiveRange = 10;
}

public class PlayerBuff 
{

}

public class NormalPlayer: CharacterStatic
{
    public NormalPlayer(CharacterController controller) : base(controller) { }
    public NormalPlayer(Rigidbody rigidbody) : base(rigidbody) { }
}


