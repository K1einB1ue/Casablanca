using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;





public interface Building
{
    void BuildingUse1();
    void BuildingUse2();
    void BuildingUse3();
    void Use1();
    void Use2();
    void Use3();
    void Use4();
    void Use5();
    void Use6(Item item,out Item itemoutEX);
    void Init(Item_INFO_Handler INFO_Handler);
}


public class Window : Building
{
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
    public virtual void Use6(Item item, out Item itemoutEX) {
        itemoutEX = Items.Empty;
    }
}
public class Drawer : Building
{
    Container container;
    Item_INFO_Handler INFO_Handler;

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
    public virtual void Use6(Item item, out Item itemoutEX) {
        itemoutEX = Items.Empty;
    }
}




public abstract class BuildingStatic<T> : ContainerStatic where T:Building,new()
{
    public Building Building;

    public override void Use1() {
        this.Building.Use1();
    }
    public override void Use2() {
        this.Building.Use2();
    }
    public override void Use3() {
        this.Building.Use3();
    }
    public override void Use4() {
        this.Building.Use4();
    }
    public override void Use5() {
        this.Building.Use5();
    }
    public override void Use6(Item item, out Item itemoutEX) {
        this.Building.Use6(item, out Item itemoutEx);
        itemoutEX = itemoutEx;
    }
    public BuildingStatic(int size) : base(size) {
        this.Building = new T();
        this.Building.Init(this.Info_Handler);
    }
}




