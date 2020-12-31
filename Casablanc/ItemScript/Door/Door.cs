using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDoor
{
    DoorState GetDoorState();
}
public class Door : Building, IDoor
{
    public DoorState doorState = new DoorState();
    private Item_INFO_Handler INFO_Handler;
    void Building.Init(Item_INFO_Handler INFO_Handler) {
        this.INFO_Handler = INFO_Handler;
    }
    public virtual void BuildingUse1() {

    }
    public virtual void BuildingUse2() {

    }
    public virtual void BuildingUse3() {

    }
    public virtual void Use1() {
    }
    public virtual void Use2() {
    }
    public virtual void Use3() {
    }
    public virtual void Use4() {
    }
    public virtual void Use5() {
    }
    public virtual void Use6(Item item,out Item itemoutEX) {
        itemoutEX = Items.Empty;
    }

    DoorState IDoor.GetDoorState() {
        return this.doorState;
    }
}
public class DoorState
{
    public bool Open = false;
    public bool HasPeepholes = false;
    public bool HasPeepholesBroken = false;
    public int HardnessToPicklock = 0;
    public int Locknum = 0;
    public List<ILock> Locks = new List<ILock>();
}