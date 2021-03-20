using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class AllTool
{
    [Item(ItemType.Tool, 2)]
    public class Lighter : LighterStatic
    {
        public Lighter() {

        }
    }

    [Item(ItemType.Tool, 3)]
    public class Beer: ContainerStatic
    {
        public Beer() : base(1) {
            this.Use1Timer.SetTimer(1.0f);
            this.Use2Timer.SetTimer(0.2f);
        }


        public override void Use1() {
            ToolComponent.threw(this);
        }
        public override void Use2() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << 11;
            layerMask = ~layerMask;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, layerMask)) {
                if (hit.collider.gameObject.tag == "Water") {
                    if (MathEx.Distance(hit.point, this.Info_Handler.Instance.transform.position) <= 10) {
                        ((Container)this).AddItem(Items.GetItemByItemTypeAndItemIDStatic(ItemType.Materials, 1, 
                        (Item) => { 
                            ((Item_Detail)Item).Info_Handler.Item_Property.ItemRuntimeProperties.ItemRuntimeValues.RuntimeValues_Held.Held_Current__Initial = 1; 
                        }));
                    }
                }
            }
        }
        public override bool Displaycount() {
            return this.ContainerState.Contents[0] != Items.Empty;
        }
        public override int GetUIheld() {
            return this.ContainerState.Contents[0].Held;
        }

        public override void CollisionEnter(Collision collision) {
            this.Info_Handler.BeDmged(MathEx.scalarization(collision.relativeVelocity) * 5);
        }
    }


    [Item(ItemType.Tool, 4)]
    public class Brick : ItemBase {

        public Brick() { }
        public override void Use1() {
            ToolComponent.threw(this);
        }

        public override void CollisionEnter(Character character) {
            //((ValuePlayer)player).DecHP(20);
        }
        public override void CollisionEnter(Collision collision) {
            this.Info_Handler.BeDmged(MathEx.scalarization(collision.relativeVelocity)/2.0f);
        }
    }

}


















public interface Lighter
{
    Battery Battery { get; set; }
    void LightSwitch();
}

public abstract class LighterStatic:ContainerStatic,Lighter
{
    public Battery Battery { get; set; }
    public Timer ElectricityDec = new Timer(60f);

    public LighterStatic() : base(1) {
        this.Use2Timer.SetTimer(1);
    }
    public LighterState LighterState {
        get {
            if (this.lighterState == null) {
                this.lighterState = new LighterState();
            }
            return this.lighterState;
        } 
        set {
            this.lighterState = value;
        } 
    }

    private LighterState lighterState;
    public void LightSwitch() {
        if (this.Info_Handler.Instance) {
            this.Info_Handler.Instance.transform.Find("Light").GetComponent<Light>().enabled = !this.LighterState.light;
            this.LighterState.light = !this.LighterState.light;
        }
    }
    public override void update() {
        base.update();
        /*if (this.ContainerState.Contents[0] != Items.Empty) {
            if (this.lighterState.light) {
                ElectricityDec.TimeingLoop(ElectricityRun);
            }
        }
        */
    }
    public void ElectricityRun() {
        this.ContainerState.Contents[0].Item_Held_Handler.Decheld(1);
    }
    public override void Use1() {
        ToolComponent.threw(this);
    }
    public override void Use2() {
        this.LightSwitch();
    }

    public override void __SynchronizationBeforeInstance() {
        this.Info_Handler.Instance.transform.Find("Light").GetComponent<Light>().enabled = this.LighterState.light;

    }
}
public class LighterState
{
    public bool light = false;

}


public interface Battery
{

}
public abstract class BatteryStatic : ContainerStatic, Battery
{
    public BatteryStatic():base(1){ }
}

public class BatteryState
{

}
