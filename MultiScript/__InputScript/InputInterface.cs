using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllInput
{
    public static FalsesInput FalseInput = new FalsesInput();
    public class PlayerInput : InputStatic
    {
        public PlayerInput(Player player) : base(player) { }
        public override void awake() {
            this.StateStore.Facing = Vector2.up;
            this.StateStore.Handing = Vector2.up;
        }
        public override void update() {
            
        }
        public override void fixedupdate() {
            if (this.enable) {
                this.InputState.Control = true;
                this.InputState.K = KeyPress.K;
                this.InputState.Use1 = KeyPress.L_Down;
                this.InputState.Use2 = KeyPress.R_Down;
                this.InputState.Use3 = null;
                this.InputState.Use4 = null;
                this.InputState.Use5 = null;
                this.InputState.GetUpThingsInUpdateByRay = KeyPress.F;
                this.InputState.UseUpThingsInUpdateByRay = KeyPress.T;
                this.InputState.G = KeyPress.G;
                this.InputState.Run = KeyPress._Shift;
                this.InputState.W = KeyPress.W;
                this.InputState.A = KeyPress.A;
                this.InputState.S = KeyPress.S;
                this.InputState.D = KeyPress.D;
                this.InputState.R = KeyPress.R;
                if (Player != null) {
                    this.StateStore.Facing = new Vector2(Camera.main.WorldToScreenPoint(Player.PlayerGameObjectState.Head.transform.position).x, Camera.main.WorldToScreenPoint(Player.PlayerGameObjectState.Head.transform.position).y) - MouseTracker.MousePos;
                    this.StateStore.Handing = new Vector2(Camera.main.WorldToScreenPoint(Player.PlayerGameObjectState.Hand.transform.position).x, Camera.main.WorldToScreenPoint(Player.PlayerGameObjectState.Hand.transform.position).y) - MouseTracker.MousePos;
                }
            }
        }

    }

    public class AIInput : InputStatic
    {
        public AIInput(Player player) : base(player) { }
        public override void update() {

        }
    }
    public class FalsesInput : InputStatic
    {
        public FalsesInput() : base(null) { }
        public override void awake() {
            this.StateStore.Facing = Vector2.up;
            this.StateStore.Handing = Vector2.up;
        }
        public override void update() {
            
        }
        public override void fixedupdate() {
            if (this.enable) {
                this.InputState.Control = false;
                this.InputState.K = new bool[KeyPress.K.Length];
                this.InputState.Use1 = false;
                this.InputState.Use2 = false;
                this.InputState.Use3 = null;
                this.InputState.Use4 = null;
                this.InputState.Use5 = null;
                this.InputState.GetUpThingsInUpdateByRay = false;
                this.InputState.UseUpThingsInUpdateByRay = false;
                this.InputState.G = false;
                this.InputState.Run = false;
                this.InputState.W = false;
                this.InputState.A = false;
                this.InputState.S = false;
                this.InputState.D = false;
                this.InputState.R = false;
            }
            this.enable = false;
        }
    }
} 




public abstract class InputStatic: MonoStatic ,InputInterface
{
    private Player player;
    public InputStatic(Player player) {
        this.player = player;
    }
    public Player Player
    {
        get {
            return this.player;
        }
        set {
            this.player = value;
        }
    }
    public bool enable { get; set; }
    public InputState InputState { 
        get {
            if (this.inputState == null) {
                this.inputState = new InputState();
                this.enable = true;
            }
            return this.inputState;
        }
        set {
            this.inputState = value;
        } 
    }

    public StateStore StateStore
    {
        get {
            if (this.stateStore == null) {
                this.stateStore = new StateStore();
                this.enable = true;
            }
            return this.stateStore;
        }
        set {
            this.stateStore = value;
        }
    }
    public StateStore stateStore;

    public InputState inputState;
   
}

public class StateStore
{
    public Vector2 Facing = Vector2.up;
    public Vector2 Handing = Vector2.up;
}
public class InputState
{
    public bool Control;
    public bool Run;
    public bool Use1;
    public bool Use2;
    public bool? Use3;
    public bool? Use4;
    public bool? Use5;
    public bool GetUpThingsInUpdateByRay;
    public bool UseUpThingsInUpdateByRay;
    public bool W;
    public bool A;
    public bool S;
    public bool D;
    public bool G;
    public bool R;
    public bool[] K;
    //public Vector2 Facing = Vector2.up;
    //public Vector2 Handing = Vector2.up;
}






public interface InputInterface
{
    bool enable { get; set; }
    Player Player { get; set; }
    InputState InputState { get; set; }
    StateStore StateStore { get; set; }
}


